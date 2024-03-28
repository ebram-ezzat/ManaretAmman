using BusinessLogicLayer.Services.Lookups;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Lookup;
using ManaretAmman.MiddleWare;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly ILookupsService _lookupsService;
        public LookupsController(ILookupsService lookupsService)
        => _lookupsService = lookupsService;

        [HttpGet]
        public async Task<IApiResponse> Get(string tableName, string columnName)
        {
            var result = await _lookupsService.GetLookups(tableName, columnName);
            return ApiResponse<IList<LookupDto>>.Success("data has been retrieved succussfully",result);
        }
        [HttpGet("GetShifts")]
        public async Task<IApiResponse> GetShifts()
        {
            var result = await _lookupsService.GetShifts();
            return ApiResponse<IList<EmployeeShiftDTO>>.Success("data has been retrieved succussfully", result);
        }
        [HttpGet("DownloadFileAsBase64")]
        public async Task<IApiResponse> DownloadFileAsBase64(string FilePath)
        {
            var result = await _lookupsService.GetFileBase64ByFtpPath(FilePath);
            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        [HttpGet("DownloadFileAsIFormFile")]
        public async Task<IApiResponse> DownloadFileAsIFormFile(string FilePath)
        {
            var result = await _lookupsService.GetFileAsFormFileByFtpPath(FilePath);
            return ApiResponse<IFormFile>.Success("data has been retrieved succussfully", result);
        }

        /// <summary>
        /// you should send {Accept-Language} Via header request "ar" For Arabic and "en" For English ,
        /// And you should send ProjectId from DDL not from Header 
        /// </summary>
        /// <param name="getLookupTableDataInput"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [SkipHeaderFilter]//Skip ProjectID From Header
        [HttpGet("GetLookupData")]
        public async Task<IApiResponse> GetLookupData([FromQuery]GetLookupTableDataInput getLookupTableDataInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _lookupsService.GetLookupData(getLookupTableDataInput);
            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
        /// <summary>
        /// This Api depends on ProjectId From DDL Not From Header
        /// </summary>
        /// <param name="getTableAndColumnOfProject"></param>
        /// <returns></returns>
        [SkipHeaderFilter]
        [HttpGet("GetTableNamesWithColumnNames")]
        public async Task<IApiResponse> GetTableNamesWithColumnNames([FromQuery]GetTableAndColumnOfProject getTableAndColumnOfProject)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _lookupsService.GetTableNamesColumnNamesByProjectId(getTableAndColumnOfProject);
            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
        /// <summary>
        /// This Api depends on ProjectId From DDL Not From Header
        /// </summary>
        /// <param name="deleteLookup"></param>
        /// <returns></returns>
        [SkipHeaderFilter]//Skip ProjectID From Header
        [HttpDelete("DeleteLookup")]
        public async Task<IApiResponse> DeleteLookup([FromQuery]DeleteLookup deleteLookup)
        {
            var result = await _lookupsService.DeleteLookup(deleteLookup);
            if(result==-1)//error
            {
                return ApiResponse.Failure("delete Failed");
            }
            return ApiResponse<int>.Success("data has been deleted succussfully", result);
        }
        /// <summary>        
        /// All Fields is required ,The ProjectID is seleted from DDL       
        /// </summary>
        /// <param name="insertLookup"></param>
        /// <returns></returns>
        [SkipHeaderFilter]//Skip ProjectID From Header
        [HttpPost("InsertLookup")]
        public async Task<IApiResponse> InsertLookup(InsertLookup insertLookup)
        {
            var result = await _lookupsService.InsertLookup(insertLookup);
            if (result == -1)//error
            {
                return ApiResponse.Failure("insert Failed");
            }
            return ApiResponse<int>.Success("data has been inserted succussfully", result);
        }
    }
}
