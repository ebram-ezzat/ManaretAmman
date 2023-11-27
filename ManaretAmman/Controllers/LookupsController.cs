using BusinessLogicLayer.Services.Lookups;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.Employees;
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
    }
}
