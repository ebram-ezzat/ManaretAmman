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
        public static bool UploadFileToFtp(string ftpUrl, string userName, string password, Stream fileStream, string fileName)
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
        public async static Task<dynamic> GetFileBase64ByFtpPath(string fullPath, string ftpUsername, string ftpPassword)
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

        public static object BuildRdlcReportWithDataSourc<T>(List<T> DataSource, string PathRdlc, string DSName)
        {
            if (File.Exists(PathRdlc))
            {
                LocalReport rpt = new LocalReport();
                rpt.ReportPath = Path.GetFullPath(PathRdlc);
                rpt.EnableExternalImages = true;

                rpt.DataSources.Clear();
                rpt.DataSources.Add(new ReportDataSource(DSName, DataSource));

                byte[] Bytes = rpt.Render(format: "PDF", deviceInfo: "");
                rpt.Dispose();

                var base64 =  Convert.ToBase64String(Bytes);
                return base64;
            }
            else
            {
                return null;
            }
        }
    }
}
