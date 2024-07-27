using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.EmployeeLoans;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeLoans;
using DataAccessLayer.DTO.Employees;
using ManaretAmman.MiddleWare;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IEmployeeLoansService _employeeService;

        public LoansController(IEmployeeLoansService employeeService)
        => _employeeService = employeeService;

        [HttpGet("GetPage")]
        public async Task<IApiResponse> GetPage([FromQuery] PaginationFilter<EmployeeLoanFilter> filter)
        {
            var result = await  _employeeService.GetPage(filter);

            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }

        [HttpGet]
        public async Task<IApiResponse> Get(int id)
        {
            var result = await _employeeService.Get(id);

            return ApiResponse<EmployeeLoansOutput>.Success("data has been retrieved succussfully", result);
        }

        [HttpPost]
        public async Task<IApiResponse> Create(EmployeeLoansInput employee)
        {
            await _employeeService.Create(employee);

            return ApiResponse.Success();
        }

        [HttpPut]
        public async Task<IApiResponse> Update(EmployeeLoansUpdate employee)
        {
            await _employeeService.Update(employee);

            return ApiResponse.Success();
        }


        [HttpDelete]
        public async Task<IApiResponse> Delete(int employeeLoanId)
        {
            await _employeeService.Delete(employeeLoanId);
            return ApiResponse.Success();
        }

        /// <summary>
        /// <para>{IsPaid} 0 : not Paid </para>
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        [HttpPost("CreateScheduledLoans")]
        public async Task<IApiResponse> CreateScheduledLoans([FromBody]SchededuledLoansInput employees)
        {
            await _employeeService.CreateScheduledLoans(employees);

            return ApiResponse.Success();
        }
        [HttpPost("UpdateScheduledLoans")]
        public async Task<IApiResponse> UpdateScheduledLoans([FromBody] SchededuledLoansInput employees)
        {
            await _employeeService.UpdateScheduledLoans(employees);

            return ApiResponse.Success();
        }
        /// <summary>
        /// <para>{EmployeeLoanID} you can send this to delete one loan from popup</para>
        /// <para>{LoanSerial} and {EmployeeID } or you can send this to delete all loans from Gird</para>
        /// <para>{CanDelete} should be checked to returned from getAPi with value 1 to can delete </para>
        /// </summary>
        /// <param name="deleteSchededuledLoansInput"></param>
        /// <returns></returns>
        [HttpDelete("DeleteScheduledLoans")]
        public async Task<IApiResponse> DeleteScheduledLoans([FromQuery] DeleteSchededuledLoansInput deleteSchededuledLoansInput)
        {
            await _employeeService.DeleteScheduledLoans(deleteSchededuledLoansInput);

            return ApiResponse.Success();
        }

        /// <summary>        
        /// <para>{Flag} is 1 </para>
        /// <para>you can send {Accept-Language} Via header request to get the correct description "ar" For Arabic and "en" For English</para>
        /// </summary>       
        /// <param name="getSchededuledLoansInput"></param>
        /// <returns></returns>
        [AddLanguageHeaderAttribute]
        [HttpGet("GetScheduledLoans")]
        public async Task<IApiResponse> GetScheduledLoans([FromQuery] EmployeeLoanParameters getSchededuledLoansInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetScheduledLoan(getSchededuledLoansInput);

            return ApiResponse<dynamic>.Success("data has been returned succussfully", result);
        }
    }

}

