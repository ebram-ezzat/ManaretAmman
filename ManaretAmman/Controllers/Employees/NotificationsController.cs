using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.Employees;
using BusinessLogicLayer.Services.Notification;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationService;

        public NotificationsController(INotificationsService notificationService)
        => _notificationService = notificationService;

       
        [HttpGet("GetPage")]
        public async Task<IApiResponse> GetPage([FromQuery] PaginationFilter<GetEmployeeNotificationInput> filter)
        {
            var result = await _notificationService.GetNotificationsAsync(filter);

            if (result == null)
                return ApiResponse<BusinessLogicLayer.Common.PagedResponse<RemiderOutput>>.Failure("No data", null);
            //if (result.Result.Count == 0)
            //    return ApiResponse<object>.Success("No data", result);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }

        [HttpPost("AcceptOrRejectNotifications")]
        public async Task<IApiResponse> AcceptOrRejectNotifications(AcceptOrRejectNotifcationInput model)
        {
            var result = await _notificationService.AcceptOrRejectNotificationsAsync(model);

            if (result == null || result == 0)
            {
                List<ChangeEmployeeRequestStatusResult> res = new List<ChangeEmployeeRequestStatusResult>();

                res.Add(new ChangeEmployeeRequestStatusResult());

                return ApiResponse<List<ChangeEmployeeRequestStatusResult>>.Failure(res, null);
            }
            return ApiResponse<int?>.Success(result);
        }

        [HttpPost("UpdateNotification")]
        public async Task<IApiResponse> UpdateNotification([FromBody] UpdateNotificationModel model)
        {
            if (model.NotificationId <= 0)
            {

                return ApiResponse.Failure(" An unexpected error on validation occurred you should path each parameter bigger than 0", null);
            }
            var result = await _notificationService.UpdateNotification(model.NotificationId);

            return ApiResponse<int>.Success("data has been retrieved succussfully", result);
        }
    }
}
