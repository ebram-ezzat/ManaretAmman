using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Services.Lookups
{
    public interface ILookupsService
    {
        Task<IList<LookupDto>> GetLookups(string tableName, string columnName);
        Task<string> GetDescription(string tableName, string columnName, int columnValue);
        Task<string> GetDescriptionAr(string tableName, string columnName, int columnValue);
        Task<object> GetFileBase64ByFtpPath(string fullPath);
        Task<IFormFile> GetFileAsFormFileByFtpPath(string fullPath);
    }
}
