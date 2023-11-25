using BusinessLogicLayer.Common;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Notification
{
    public class NotificationsService : INotificationsService
    {
        private IProjectProvider _projectProvider;
        private readonly ILookupsService _lookupsService;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        readonly IAuthService _authService;
        readonly int _userId;
        readonly int _projectId;

        public NotificationsService(IProjectProvider projectProvider, ILookupsService lookupsService, PayrolLogOnlyContext payrolLogOnlyContext, IAuthService authService)
        {
            _projectProvider = projectProvider;
            _lookupsService = lookupsService;
            _payrolLogOnlyContext = payrolLogOnlyContext;
            _authService = authService;
            _userId = _projectProvider.UserId();
            _projectId = _projectProvider.GetProjectId();
        }

        public async Task<int?> AcceptOrRejectNotificationsAsync(AcceptOrRejectNotifcationInput model)
        {
            if (model.CreatedBy == 0) model.CreatedBy = _userId;
            //int? pError = null;
            var result = await _payrolLogOnlyContext.GetProcedures()
                .ChangeEmployeeRequestStatusAsync(model.EmployeeId, model.CreatedBy, model.ApprovalStatusId, model.ApprovalPageID, _projectId, model.Id, model.PrevilageType, 0, null, true, null, null);
            Console.WriteLine(result);
            // OutputParameter<int?> pErrorr = new OutputParameter<int?>(pError);
            return result;
        }

        public async Task<object> GetNotificationsAsync(PaginationFilter<GetEmployeeNotificationInput> filter)
        {

            //var result = await _payrolLogOnlyContext.GetProcedures()
            //            .GetRemindersAsync(_projectId, filter.FilterCriteria.EmployeeID, 1, filter.FilterCriteria.LanguageId, filter.FilterCriteria.Fromdate.DateToIntValue(),
            //            filter.FilterCriteria.ToDate.DateToIntValue(), null, _userId, null);

            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pProjectID", _projectId },
                { "pEmployeeID", filter.FilterCriteria.EmployeeID },
                { "pFlag", 1 },
                { "pLanguageID", filter.FilterCriteria.LanguageId },
                { "pFromDate",filter.FilterCriteria.Fromdate.DateToIntValue() },
                { "pToDate", filter.FilterCriteria.ToDate.DateToIntValue() },
                { "pTypeID", filter.FilterCriteria.TypeID },
                { "pUserID", _userId },
                { "pUserTypeID", null },
                { "pIsDissmissed", filter.FilterCriteria.IsDissmissed },
                { "pPageNo", filter.FilterCriteria.PageNo },
                { "pPageSize", filter.FilterCriteria.PageSize },
                { "pPageTypeID", filter.FilterCriteria.PageTypeID },
            };

            // Define output parameters (optional)
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "prowcount","int" }
            };

            var (NotificationResponse, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<RemiderOutput>("[dbo].[GetReminders]", inputParams, outputParams);

            return PublicHelper.CreateResultPaginationObject(filter.FilterCriteria, NotificationResponse, outputValues);

        }
    }
}
