using BusinessLogicLayer.Common;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.EmployeeAttendance;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.EmployeeAttendance
{
    public class EmployeeAttendanceService: IEmployeeAttendanceService
    {
        private IProjectProvider _projectProvider;
        private readonly ILookupsService _lookupsService;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        readonly IAuthService _authService;
        readonly int _userId;
        readonly int _projectId;
        public EmployeeAttendanceService(IProjectProvider projectProvider, ILookupsService lookupsService, PayrolLogOnlyContext payrolLogOnlyContext, IAuthService authService) 
        {
            _projectProvider = projectProvider;
            _lookupsService = lookupsService;
            _payrolLogOnlyContext = payrolLogOnlyContext;
            _authService = authService;
            _userId = _projectProvider.UserId();
            _projectId = _projectProvider.GetProjectId();
        }

        public async Task<PagedResponse<EmployeeAttendanceOutput>> GetEmployeeAttendance(PaginationFilter<EmployeeAttendanceInput> filter)
        {

            var _filter = filter.FilterCriteria;
            var result = await _payrolLogOnlyContext.GetProcedures().GetEmployeeAttendanceAsync(_filter.EmployeeID, _filter.FromDate.DateToIntValue(), _filter.ToDate.DateToIntValue(), _projectId, _filter.YearId, null, _filter.Flag, _filter.DepartmentID, _filter.LanguageID, null,_filter.ShiftID, _userId, _filter.ApprovalTypeID, null);           

            var totalRecords = result.Count;

            var approvals = await _lookupsService.GetLookups(Constants.Approvals, string.Empty);
            var returnedData = result.Skip((filter.PageIndex - 1) * filter.Offset).Take(filter.Offset)
                             .Select(item => new EmployeeAttendanceOutput
                             {
                                 EmployeeID = item.EmployeeID,
                                 EmployeeName = item.EmployeeName,
                                 AttendanceDate = item.AttendanceDate.IntToDateValue(),
                                 DayDesc = item.DayDesc,
                                 EmployeeNumber = item.EmployeeNumber,
                                 //EndTime = item.EndTime.ConvertFromMinutesToTimeString(),
                                 EndTime = item.CheckOut.ConvertFromMinutesToTimeString(),
                                 Notes = item.Notes,
                                 ShiftName = item.ShiftName,
                                 //StartTime = item.StartTime.ConvertFromMinutesToTimeString(),
                                 StartTime = item.CheckIn.ConvertFromMinutesToTimeString(),
                                 Workhours = (TimeSpan.FromMinutes((double)item.EndTime) - TimeSpan.FromMinutes((double)item.StartTime)).ToString(@"hh\:mm"),
                                 ShiftWithTimes = $"{item.StartTime.ConvertFromMinutesToTimeString()} | {item.EndTime.ConvertFromMinutesToTimeString()} | {item.ShiftName}",
                                 Systemtimeinminutes = item.Systemtimeinminutes,
                                 Approvedtimeinminutes = item.Approvedtimeinminutes,
                                 ApprovedStatusID = item.ApprovedStatusID,
                                 EmployeeImage=item.EmployeeImage,
                                 JobTitleName=item.JobTitleName,
                                 AnyWhere=item.AnyWhere,
                                 ShiftID = item.ShiftID
                             }).ToList();

          
            
            //var test3= (returnedData.CreatePagedReponse<EmployeeAttendanceOutput>(filter.PageIndex, filter.Offset, totalRecords)).Result.Where(x => x.Approvedtimeinminutes == "05:00").ToList();
            return returnedData.CreatePagedReponse<EmployeeAttendanceOutput>(filter.PageIndex, filter.Offset, totalRecords);
        }

        public async Task<List<EmployeeAttendanceTreatmentOutput>> GetEmployeeAttendanceTreatment(EmployeeAttendanceInput employeeAttendanceInput)
        {
            var settingResult = await _lookupsService.GetSettings();

            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", employeeAttendanceInput.EmployeeID ??Convert.DBNull },
                { "pFromDate", employeeAttendanceInput.FromDate.DateToIntValue() ?? Convert.DBNull },
                { "pToDate", employeeAttendanceInput.ToDate.DateToIntValue() ?? Convert.DBNull },
                { "pLoginUserID", employeeAttendanceInput.LoginUserID ?? Convert.DBNull },
                { "pFlag", 3 },
                { "pDepartmentID", employeeAttendanceInput.DepartmentID ?? Convert.DBNull },
                { "pLanguageID", _projectProvider.LangId() },
                { "pProjectID", _projectProvider.GetProjectId() },
                { "pApprovalTypeID", employeeAttendanceInput.ApprovalTypeID ?? Convert.DBNull },
                { "pShiftID", employeeAttendanceInput.ShiftID ?? Convert.DBNull },
                { "pYearID", employeeAttendanceInput.YearId ?? Convert.DBNull },
                { "pVacationTypeID", settingResult?.PersonalVacationID ?? Convert.DBNull },

            };

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures()
                .ExecuteStoredProcedureAsync<EmployeeAttendanceTreatmentOutput>("dbo.GetEmployeeAttendance", inputParams, null);

            return result;
        }
    }
}
