using BusinessLogicLayer.Services.Employees;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.Models;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("GetList")]
        public async Task<IApiResponse> GetList() 
        {
            var result = await _employeeService.GetEmployeesProc();

            return ApiResponse<List<EmployeeLookup>>.Success("data has been retrieved succussfully", result);
        }

        [HttpPost("SaveAttendanceByUser")]
        public async Task<IApiResponse> SaveAttendanceByUser(SaveAttendance saveAttendance)
        {
            await _employeeService.SaveAttendanceByUser(saveAttendance);

            return ApiResponse.Success("data has been retrieved succussfully");
        }
        [HttpGet("Documents/GetEmployeePaper")]
        public async Task<IApiResponse> GetEmployeePaper([FromQuery] GetEmployeePaperRequest getEmployeePaperRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeePaperProc(getEmployeePaperRequest);



            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        [HttpDelete("Documents/DeleteEmployeePaper")]
        public async Task<IApiResponse> DeleteEmployeePaper([FromQuery]  DeleteEmployeePaper model)
        {
            if (model.EmployeeId <= 0 || model.DetailId <= 0)
            {

                return ApiResponse.Failure(" An unexpected error on validation occurred you should path each parameter bigger than 0",null);
            }
            var result = await _employeeService.DeleteEmployeePaperProc(model.EmployeeId, model.DetailId);



            return ApiResponse<int>.Success("data has been retrieved succussfully", result);
        }
        [HttpPost("Documents/SaveEmployeePaper")]
        public async Task<IApiResponse> SaveEmployeePaper([FromForm]SaveEmployeePaper saveEmployeePaper)
        {
            //var formCollection = await Request.ReadFormAsync();
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
           var result = await _employeeService.SaveEmployeePaperProc(saveEmployeePaper);



            return ApiResponse<int>.Success("data has been retrieved succussfully", result);
        }

        [HttpGet("EmployeeProfile")]
        public async Task<IApiResponse> EmployeeProfile(int EmployeeId)
        {
            var result = await _employeeService.EmployeeProfile(EmployeeId);

            return ApiResponse<List<EmplyeeProfileVModel>>.Success("data has been retrieved succussfully", result);
        }
        #region شاشة خدمات شوون الموظفين
        /// <summary>
        ///{StatusID} Should be sent by 1 
        /// </summary>
        /// <param name="saveEmployeeAffairsService"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("SaveEmployeeAffairsService")]
        public async Task<IApiResponse> SaveEmployeeAffairsService(SaveEmployeeAffairsServices saveEmployeeAffairsService)
        {
           var result= await _employeeService.SaveEmployeeAffairsService(saveEmployeeAffairsService);
            if(result ==-1)//error
            {
                throw new Exception("Error on Save Operation");
            }
            return ApiResponse.Success("data has been saved succussfully");
        }
        #endregion
    }
}
