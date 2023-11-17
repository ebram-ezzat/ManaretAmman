using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

    }
}
