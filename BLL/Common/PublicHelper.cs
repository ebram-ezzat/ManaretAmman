using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;

using System.Dynamic;

using System.Net;
using System.Reflection;

using DataAccessLayer.DTO;
using Microsoft.Reporting.NETCore;

namespace BusinessLogicLayer.Common
{
    internal class PublicHelper
    {
        public static Dictionary<string, object> GetPropertiesWithPrefix<T>(T obj, string prefix)
        {
            var parameters = new Dictionary<string, object>();

            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string parameterName = prefix + property.Name;
                parameters.Add(parameterName, property.GetValue(obj));
            }

            return parameters;
        }
        public static bool UploadFileToFtp(string ftpUrl,string userName,string password, Stream fileStream, string fileName)
        {
            bool IsComplete = false;
            // Create FTP request
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
            ftpRequest.Credentials = new NetworkCredential(userName, password);
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

            // Set content type
            //ftpRequest.ContentType = "application/octet-stream";

            // Copy the file stream to the request stream
            using (Stream requestStream = ftpRequest.GetRequestStream())
            {
                fileStream.CopyTo(requestStream);
            }

            // Get the FTP response
            using (FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
            {
                IsComplete = true;
                //Console.WriteLine($"Upload File Complete, status {ftpResponse.StatusDescription}");
            }
            return IsComplete;
        }

        private async Task UploadFileAsync(string url, Stream fileStream, string paramName, string contentType, string fileName)
        {
            using (var httpClient = new HttpClient())
            using (var form = new MultipartFormDataContent())
            using (var fileContent = new StreamContent(fileStream))
            {
                fileContent.Headers.Add("Content-Type", contentType);
                form.Add(fileContent, paramName, fileName);

                var response = await httpClient.PostAsync(url, form);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error uploading file. Status code: {response.StatusCode}");
                    // Handle error accordingly
                }
                else
                {
                    Console.WriteLine("File uploaded successfully!");
                }
            }
        }
        public async static Task <object> GetFileBase64ByFtpPath(string fullPath,string ftpUsername, string ftpPassword)
        {
            // FTP server details

            string ftpUrl = Convert.ToString(fullPath);
            // Create the FTP request
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            //request.Credentials = new NetworkCredential("administrator", "o36GOR56euIWywTpyu933");
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
            request.UsePassive = true;

           
                // Get the FTP response
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                if (response.StatusCode == FtpStatusCode.CommandOK || response.StatusCode == FtpStatusCode.DataAlreadyOpen 
                    || response.StatusCode == FtpStatusCode.ClosingData || response.StatusCode == FtpStatusCode.OpeningData)
                {
                    //Get the response stream
                    using (Stream ftpStream = response.GetResponseStream())
                    {
                        // Create a memory stream to store the file content
                        MemoryStream memoryStream = new MemoryStream();

                        // Copy the FTP stream to the memory stream
                        ftpStream.CopyTo(memoryStream);

                        // Convert the file content to a Base64 string
                        string base64Content = Convert.ToBase64String(memoryStream.ToArray());
                        //result.Base64FileContent = base64Content;
                        // Return the Base64 string as IActionResult
                        return new { Base64Content = base64Content };
                    }
                    //using (Stream ftpStream = response.GetResponseStream())
                    //{
                    //    // Read the directory listing
                    //    using (StreamReader reader = new StreamReader(ftpStream))
                    //    {
                    //        string directoryListing = reader.ReadToEnd();
                    //        List<string> fileList = directoryListing.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    //        // You now have a list of files and directories in the specified FTP directory
                    //        return fileList;
                    //    }
                    //}
                }
                else
                {
                    // Handle FTP errors
                    return ($"FTP Error: {response.StatusDescription}");
                }
            }

        }
        public async static Task<IFormFile> GetFileAsFormFileByFtpPath(string fullPath, string ftpUsername, string ftpPassword)
        {

            // Create the FTP request
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{fullPath}");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            // Get the FTP response
            using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
            {
                if (response.StatusCode == FtpStatusCode.CommandOK || response.StatusCode == FtpStatusCode.DataAlreadyOpen)
                {
                    // Get the response stream
                    using (Stream ftpStream = response.GetResponseStream())
                    {
                        // Create a memory stream to store the file content
                        MemoryStream memoryStream = new MemoryStream();

                        // Copy the FTP stream to the memory stream
                        await ftpStream.CopyToAsync(memoryStream);

                        // Convert the memory stream to an array of bytes
                        byte[] fileBytes = memoryStream.ToArray();
                        string filename = Path.GetFileName(fullPath);
                        // Create an IFormFile from the byte array
                        IFormFile formFile = new FormFile(new MemoryStream(fileBytes), 0, fileBytes.Length, "file", filename);


                        // Return the IFormFile
                        return formFile;
                    }
                }
                else
                {
                    // Handle FTP errors
                    return default;
                }
            }

        }
        public static object CreateResultPaginationObject<TRequest, TResponse>(
         TRequest request,
         List<TResponse> response,
         Dictionary<string, object> outputValues)
                where TRequest : PageModel
        {
            dynamic obj = new ExpandoObject();

            if (outputValues.TryGetValue("prowcount", out var totalRecordsObj) && totalRecordsObj is int totalRecords)
            {
                var totalPages = ((double)totalRecords / (double)request.PageSize);
                int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

                obj.totalPages = roundedTotalPages;
                obj.result = response;
                obj.pageIndex = request.PageNo;
                obj.offset = request.PageSize;
            }

            return obj;
        }
        public static object BuildRdlcReportWithDataSourc<T>(List<T> DataSource,string PathRdlc,string DSName)
        {
            try
            {
                if (Directory.Exists(Path.GetDirectoryName(PathRdlc)))
                {
                    Console.WriteLine("Directory exists.");

                    // Check if the file exists
                    if (File.Exists(PathRdlc))
                    {
                        Console.WriteLine("File exists and is correct.");
                    }
                    else
                    {
                        Console.WriteLine("File does not exist.");
                    }
                }
                else
                {
                    Console.WriteLine("Directory does not exist.");
                }
            
                LocalReport rpt = new LocalReport();
                rpt.ReportPath = Path.GetFullPath(PathRdlc);
                //var ds = new ReportDataSource();
                //ds.Name = DSName;
                //ds.Value = DataSource;
                //rpt.DataSources.Add(ds);

                rpt.DataSources.Add(new ReportDataSource("UsersDataSet", new List<object>()));
                // rpt.DataSources.Add(new ReportDataSource("DSName", DataSource));

                //var reportParameter = new List<ReportParameter>()
                //            {
                //    new ReportParameter("StartDate", DateTime.Now.Date.ToString()),
                //    new ReportParameter("EndDate", DateTime.Now.Date.ToString())
                //};
                //rpt.SetParameters(reportParameter);

                byte[] Bytes = rpt.Render(format: "PDF", deviceInfo: "");
                // var exportPath = Path.Combine(_hostEnvironment.ContentRootPath, "Export");
                //if (!Directory.Exists(exportPath))
                //    Directory.CreateDirectory(exportPath);
                //exportPath = Path.Combine(exportPath, "Reports");
                //if (!Directory.Exists(exportPath))
                //    Directory.CreateDirectory(exportPath);
                //exportPath = Path.Combine(exportPath, $"Work Situation By governorates According Education With Gender.pdf");
                //using (FileStream stream = new FileStream(PdfPath, FileMode.Create))
                //{
                //    stream.Write(Bytes, 0, Bytes.Length);
                //}
                rpt.Dispose();
                var PdfByte = File.ReadAllBytes(PathRdlc);

                var Base64 = Convert.ToBase64String(Bytes);
                //var exportPathasd = Path.Combine(_hostEnvironment.ContentRootPath, "Export");
                //Directory.Delete(PdfPath, true);

                return Base64;
            }
            catch (Exception ex)
            {

                throw;
            }
           
           
            
        }
    }
}
