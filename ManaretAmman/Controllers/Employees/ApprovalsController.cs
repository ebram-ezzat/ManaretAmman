using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.Approvals;
using BusinessLogicLayer.Services.Notification;
using DataAccessLayer.DTO.Notification;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class ApprovalsController : ControllerBase
    {
        private readonly IApprovalsService _approvalsService;

        public ApprovalsController(IApprovalsService approvalsServic)
        => _approvalsService = approvalsServic;

        [HttpGet("Vacation/GetPage")]
        public async Task<IApiResponse> VacationGetPage([FromQuery] PaginationFilter<GetEmployeeNotificationInput> filter)
        {
            var result = await _approvalsService.GetVacationApprovalsAsync(filter);

            if (result == null)
                return ApiResponse<BusinessLogicLayer.Common.PagedResponse<RemiderOutput>>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
    }
}
