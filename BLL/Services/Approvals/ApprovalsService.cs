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
using System.Globalization;
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
                { "pIsDismissed", filter.FilterCriteria.IsDissmissed },
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
        public async Task<int> SaveOverTimeWorkEmployee(SaveOverTimeWorkEmployee saveOverTimeWorkEmployee)
        {
            var parameters = new Dictionary<string, object>
            {
                { "pEmployeeID", saveOverTimeWorkEmployee.EmployeeID },
                { "pTypeID", 0/*saveOverTimeWorkEmployee.TypeID??0*/ },
                { "pAttendanceDate", saveOverTimeWorkEmployee.AttendanceDate.DateToIntValue() },
                { "pSystemtimeinminutes", saveOverTimeWorkEmployee.SystemTimeInMinutes==null?string.Empty:saveOverTimeWorkEmployee.SystemTimeInMinutes.TimeStringToIntValue() },
                { "pApprovedtimeinminutes",saveOverTimeWorkEmployee.ApprovedTimeInMinutes==null?string.Empty:saveOverTimeWorkEmployee.ApprovedTimeInMinutes.TimeStringToIntValue() },
                { "pCreatedBy", _userId },
                { "pStatusID", 0/*saveOverTimeWorkEmployee.StatusID??2*/ },
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

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertEmployeeApprovales", parameters, outputParameters);

            //check the Over Time if Already Exists
            if (outputValues.TryGetValue("pError", out var value))
            {
                if (Convert.ToInt32(value) == -3)
                {
                    throw new UnauthorizedAccessException("AlreadyExists");
                }
            }
            return result;
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
                { "pTypeID", inputModel.TypeID??0 },
                { "pFromDate", inputModel.FromDate==null?null:inputModel.FromDate.DateToIntValue() },
                { "pToDate", inputModel.ToDate==null?null:inputModel.ToDate.DateToIntValue() },
                { "pProjectID", _projectId },
                { "pPageNo", inputModel.PageNo },
                { "pPageSize", inputModel.PageSize },
                { "pLoginUserID", _userId },
                { "pLanguageID", inputModel.LanguageID==0?1:inputModel.LanguageID},
                { "pemployeeapprovalid" ,inputModel.ApprovalID },
                { "pOnlyApprovals", inputModel.OnlyApprovals==null?null:inputModel.OnlyApprovals },

            };

            var outputParameters = new Dictionary<string, object>
            {
                { "prowcount", "int" }

            };
            var (ResponseOverTime, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetOverTimeWorkEmployeeOutputModel>("dbo.GetEmployeeApprovales", inputParameters, outputParameters);
            var getOverTimeWorkEmployeeReturnModel = ResponseOverTime.Select(x => new GetOverTimeWorkEmployeeReturnModel
            {
                EmployeeID=x.EmployeeID,
                EmployeeName=x.EmployeeName,
                EmployeeNumber=x.EmployeeNumber,
                EmployeeApprovalID=x.EmployeeApprovalID,
                AttendanceDate=x.AttendanceDate.IntToDateValue(),
                WorkingHours=(TimeSpan.FromMinutes((double)x.ToTime) - TimeSpan.FromMinutes((double)x.FromTime)).ToString(@"hh\:mm"),
                DayDesc= inputModel.LanguageID ==1 ? x.AttendanceDate.IntToDateValue().Value.ToString("dddd", new CultureInfo("ar-SA")) :
                    x.AttendanceDate.IntToDateValue().Value.DayOfWeek.ToString(),
                CheckIn= x.CheckIn.ConvertFromMinutesToTimeString(),
                CheckOut=x.CheckOut.ConvertFromMinutesToTimeString(),
                FromTime=x.FromTime.ConvertFromMinutesToTimeString(),
                ToTime=x.ToTime.ConvertFromMinutesToTimeString(),
                Notes=x.Notes,
                StatusID=x.StatusID,
                StatusDesc=x.StatusDesc,
                SystemTimeInMinutes=x.SystemTimeInMinutes,
                ApprovedTimeInMinutes=x.ApprovedTimeInMinutes,
                TypeID=x.TypeID,
                ActionTypeID=x.ActionTypeID,
                CreatedBy=x.CreatedBy,
                CreationDate=x.CreationDate,
                ModifiedBy=x.ModifiedBy,
                ModificationDate=x.ModificationDate
            }).ToList();
            return PublicHelper.CreateResultPaginationObject(inputModel, getOverTimeWorkEmployeeReturnModel, outputValues);
        }

        public async Task<int> UpdateOverTimeWorkEmployee(UpdateOverTimeWorkEmployee updateOverTimeWorkEmployee)
        {
            var parameters = new Dictionary<string, object>
            {
               { "pEmployeeID", updateOverTimeWorkEmployee.EmployeeID },
                { "pTypeID", updateOverTimeWorkEmployee.TypeID??0 },
                { "pAttendanceDate", updateOverTimeWorkEmployee.AttendanceDate.DateToIntValue() },
                { "pSystemtimeinminutes", updateOverTimeWorkEmployee.SystemTimeInMinutes==null?string.Empty:updateOverTimeWorkEmployee.SystemTimeInMinutes.TimeStringToIntValue() },

                { "pApprovedtimeinminutes", updateOverTimeWorkEmployee.ApprovedTimeInMinutes==null?string.Empty:updateOverTimeWorkEmployee.ApprovedTimeInMinutes.TimeStringToIntValue() },

                { "pCreatedBy", _userId },
                { "pStatusID", updateOverTimeWorkEmployee.StatusID??0 },
                { "pToTime", updateOverTimeWorkEmployee.ToTime.ConvertFromTimeStringToMinutes() },
                { "pActionTypeID", updateOverTimeWorkEmployee.ActionTypeID},
                { "pEmployeeApprovalID", updateOverTimeWorkEmployee.EmployeeApprovalID},
                { "pNotes", updateOverTimeWorkEmployee.Notes??string.Empty },
                { "pFromTime", updateOverTimeWorkEmployee.FromTime.ConvertFromTimeStringToMinutes() }

            };
           
            var outputParameters = new Dictionary<string, object>
            {
                { "pError", "int" },
               
                // Add other output parameters based on your stored procedure
            };

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.UpdateEmployeeApprovales", parameters, outputParameters);
            //check the Over Time if Already Exists
            if (outputValues.TryGetValue("pError", out var value))
            {
                if (Convert.ToInt32(value) == -3)
                {
                    throw new UnauthorizedAccessException("AlreadyExists");
                }
            }
            return result;
        }
    }
}
