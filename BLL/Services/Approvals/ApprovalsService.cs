﻿using BusinessLogicLayer.Common;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Approvals
{
    public class ApprovalsService : IApprovalsService
    {
        private IProjectProvider _projectProvider;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        readonly IAuthService _authService;
        readonly int _userId;
        readonly int _projectId;

        public ApprovalsService(IProjectProvider projectProvider, PayrolLogOnlyContext payrolLogOnlyContext, IAuthService authService)
        {
            _projectProvider = projectProvider;
            _payrolLogOnlyContext = payrolLogOnlyContext;
            _authService = authService;
            _userId = _projectProvider.UserId();
            _projectId = _projectProvider.GetProjectId();
        }

        public async Task<object> GetVacationApprovalsAsync(PaginationFilter<GetEmployeeNotificationInput> filter)
        {

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
                { "pPageTypeID", 2 },
            };

            // Define output parameters (optional)
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "prowcount","int" }
            };

            var (NotificationResponse, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<RemiderOutput>("[dbo].[GetReminders]", inputParams, outputParams);

            return PublicHelper.CreateResultPaginationObject(filter.FilterCriteria, NotificationResponse, outputValues);

        }

        public async Task<int> SaveWorkEmployeeApprovals(WorkEmployeeApprovals workEmployeeApprovals)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeID", workEmployeeApprovals.EmployeeID },
            //{ "pTypeID", workEmployeeApprovals.TypeID },
            { "pTypeID", 0 },
            { "pAttendanceDate", workEmployeeApprovals.AttendanceDate.DateToIntValue() },
            { "pSystemtimeinminutes", workEmployeeApprovals.Systemtimeinminutes.TimeStringToIntValue() },
            { "pApprovedtimeinminutes", workEmployeeApprovals.Approvedtimeinminutes.TimeStringToIntValue() },
            { "pCreatedBy ", _userId },
            { "pStatusID  ", workEmployeeApprovals.StatusID },
            {"pToTIME","" },
            {"pActionTypeID","" },
            {"pEmployeeApprovalID","" },
            {"pnotes","" },
            {"pFromTime","" },
        };

            // Define output parameters (optional)
            Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" }, // Assuming the output parameter "pError" is of type int
            // Add other output parameters as needed

        };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.UpdateEmployeeApprovales", inputParams, outputParams);


            int pErrorValue = (int)outputValues["pError"];

            return result;
        }

    }
}
