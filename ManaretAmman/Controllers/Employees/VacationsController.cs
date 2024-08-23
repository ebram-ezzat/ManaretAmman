using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.EmployeeVacations;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeVacations;
using DataAccessLayer.DTO.Locations;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees;

[Route("api/Employees/[controller]")]
[ApiController]
public class VacationsController : ControllerBase
{
    private readonly IEmployeeVacationService _employeeService;

    public VacationsController(IEmployeeVacationService employeeService)
    => _employeeService = employeeService;

    [HttpGet("GetPage")]
    public async Task<IApiResponse> GetPage([FromQuery] PaginationFilter<EmployeeVacationFilter> filter)
    {
        var result = await  _employeeService.GetPage(filter);
        return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);

        //return ApiResponse<BusinessLogicLayer.Common.PagedResponse<EmployeeVacationOutput>>.Success("data has been retrieved succussfully", result);
    }

    [HttpGet]
    public async Task<IApiResponse> Get(int id)
    {
        var result = await _employeeService.Get(id);

        return ApiResponse<EmployeeVacationOutput>.Success("data has been retrieved succussfully", result);
    }

    [HttpPost]
    public async Task<IApiResponse> Create([FromForm]EmployeeVacationInput employee)
    {
        await _employeeService.Create(employee);

        return ApiResponse.Success();
    }

    [HttpPut]
    public async Task<IApiResponse> Update([FromForm] EmployeeVacationsUpdate employee)
    {
        await _employeeService.Update(employee);

        return ApiResponse.Success();
    }


    [HttpDelete]
    public async Task<IApiResponse> Delete(int employeeVacationId)
    {
        await _employeeService.Delete(employeeVacationId);
        return ApiResponse.Success();
    }

    #region العطل الرسمية 
    [HttpDelete("DeleteOfficialVacation")]
    public async Task<IApiResponse> DeleteOfficialVacation([FromQuery]DeleteOfficialVacation deleteOfficialVacation)
    {
        if (!ModelState.IsValid)
        {
            // Model validation failed based on data annotations including your custom validation
            // Retrieve error messages
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
        }
        await _employeeService.DeleteOfficialVacation(deleteOfficialVacation);
        return ApiResponse.Success();
    }
    [HttpPost("SaveOrUpdateOfficialVacation")]
    public async Task<IApiResponse> SaveOrUpdateOfficialVacation([FromBody] OfficialVacationSaveData officialVacationSaveData)
    {
        if (!ModelState.IsValid)
        {
            // Model validation failed based on data annotations including your custom validation
            // Retrieve error messages
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
        }
        await _employeeService.SaveOfficialVacation(officialVacationSaveData);
        return ApiResponse.Success();
    }
    [HttpGet("GetOfficialVacation")]
    public async Task<IApiResponse> GetOfficialVacation([FromQuery] OfficialVacationGetInput officialVacationGetInput)
    {
        if (!ModelState.IsValid)
        {
            // Model validation failed based on data annotations including your custom validation
            // Retrieve error messages
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
        }
        var result = await _employeeService.GetOfficialVacation(officialVacationGetInput);

        return ApiResponse<object>.Success("data has been retrieved succussfully", result);
    }
    #endregion
}

