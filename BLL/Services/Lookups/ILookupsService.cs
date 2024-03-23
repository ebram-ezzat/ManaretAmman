using DataAccessLayer.DTO;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Lookup;
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
        Task<List<EmployeeShiftDTO>> GetShifts();
        Task<GetSettingsResult> GetSettings(int Falg = 1);
        Task<int> InsertLookup(InsertLookup insertLookup);
        Task<int> DeleteLookup(DeleteLookup deleteLookup);
        Task<List<GetTableNamesWithColumnNames>> GetTableNamesColumnNamesByProjectId(GetTableAndColumnOfProject getTableAndColumnOfProject);
        Task<List<GetLookupTableDataOutput>> GetLookupData(GetLookupTableDataInput getLookupTableDataInput);

    }
}
