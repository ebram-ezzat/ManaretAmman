using BusinessLogicLayer.Common;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
        public async Task<(int, Dictionary<string, object>)> SaveOverTimeWorkEmployee(SaveOverTimeWorkEmployee saveOverTimeWorkEmployee)
        {
            var parameters = new Dictionary<string, object>
            {
                { "pEmployeeID", saveOverTimeWorkEmployee.EmployeeID },
                { "pTypeID", saveOverTimeWorkEmployee.TypeID??0 },
                { "pAttendanceDate", saveOverTimeWorkEmployee.AttendanceDate.DateToIntValue() },
                { "pSystemtimeinminutes", saveOverTimeWorkEmployee.SystemTimeInMinutes==null?string.Empty:saveOverTimeWorkEmployee.SystemTimeInMinutes.TimeStringToIntValue() },
                { "pApprovedtimeinminutes", saveOverTimeWorkEmployee.ApprovedTimeInMinutes==null?string.Empty:saveOverTimeWorkEmployee.ApprovedTimeInMinutes.TimeStringToIntValue() },
                { "pCreatedBy", _userId },
                { "pStatusID", saveOverTimeWorkEmployee.StatusID??2 },
                { "pFromTime", saveOverTimeWorkEmployee.FromTime.ConvertFromTimeStringToMinutes() },
                { "pToTime", saveOverTimeWorkEmployee.ToTime.ConvertFromTimeStringToMinutes() },
                { "pNotes", saveOverTimeWorkEmployee.Notes??string.Empty },

            };

            var outputParameters = new Dictionary<string, object>
            {
                { "pError", "int" },
                { "pEmployeeApprovalID", "int" }
                // Add other output parameters based on your stored procedure
            };

            return await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertEmployeeApprovales", parameters, outputParameters);
        }

        public async Task<int> DeleteOverTimeWorkEmployee(DeleteOverTimeWorkEmployee deleteOverTimeWorkEmployee)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
          {
            { "pEmployeeApprovalID", deleteOverTimeWorkEmployee.EmployeeApprovalID },
            { "pProjectID", _projectId }           

          };

           
            Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" }, // Assuming the output parameter "pError" is of type int
            
        };

            // Call the ExecuteStoredProcedureAsync function
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeeApprovales", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];

            
            return pErrorValue;
        }

        public async Task<object> GetOverTimeWorkEmployee(GetOverTimeWorkEmployeeInputModel inputModel)
        {
            var inputParameters = new Dictionary<string, object>
            {
                { "pEmployeeID", inputModel.EmployeeID },
                { "pTypeID", inputModel.TypeID },
                { "pFromDate", inputModel.FromDate==null?string.Empty:inputModel.FromDate.DateToIntValue() },
                { "pToDate", inputModel.ToDate==null?string.Empty:inputModel.ToDate.DateToIntValue() },
                { "pProjectID", _projectId },
                { "pPageNo", inputModel.PageNo },
                { "pPageSize", inputModel.PageSize },
                { "pLoginUserID", _userId },
                { "pLanguageID", inputModel.LanguageID==0?1:inputModel.LanguageID}
        
            };

            var outputParameters = new Dictionary<string, object>
            {
                { "pRowCount", "int" }
               
            };
            var (ResponseOverTime, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetOverTimeWorkEmployeeOutputModel>("dbo.GetEmployeeApprovales", inputParameters, outputParameters);

            return PublicHelper.CreateResultPaginationObject(inputModel, ResponseOverTime, outputValues);
        }
    }
}
