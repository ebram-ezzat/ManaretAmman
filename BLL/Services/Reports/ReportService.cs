using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Reports;
using DataAccessLayer.DTO.WorkFlow;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;


namespace BusinessLogicLayer.Services.Reports
{
    public class ReportService:IReportService
    {
        private readonly IMapper _mapper;
        IProjectProvider _projectProvider;
        IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;

        public ReportService(IMapper mapper, IProjectProvider projectProvider, IAuthService authService, PayrolLogOnlyContext payrolLogOnlyContext, IConfiguration configuration)
        {
            _mapper = mapper;
            _projectProvider = projectProvider;
            _authService = authService;
            _payrolLogOnlyContext = payrolLogOnlyContext;
            _configuration = configuration;
        }
       public async Task<object> GetEmployeeSalaryReport(GetEmployeeSalaryReportRequest getEmployeeSalaryReportRequest)
        {
            getEmployeeSalaryReportRequest.ProjectID = _projectProvider.GetProjectId();
            getEmployeeSalaryReportRequest.loginuserid = _projectProvider.UserId();
            var parameters = PublicHelper.GetPropertiesWithPrefix<GetEmployeeSalaryReportRequest>(getEmployeeSalaryReportRequest, "p");
            
            var (getEmployeeSalaryReportResponse, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeSalaryReportResponse>("[dbo].[GetEmployeeSalary]", parameters, null);
            return getEmployeeSalaryReportResponse.FirstOrDefault()?.EmailContent;
        }

        public async Task<object> GetEmployeeAttendanceDailyReport(GetEmployeeAttendanceDailyRequest getEmployeeAttendanceDailyRequest)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pFromDate",getEmployeeAttendanceDailyRequest.FromDate==null
                ?null:getEmployeeAttendanceDailyRequest.FromDate.DateToIntValue()},
                {"pToDate",getEmployeeAttendanceDailyRequest.ToDate==null
                ?null:getEmployeeAttendanceDailyRequest.ToDate.DateToIntValue()},
                {"pProjectID",_projectProvider.GetProjectId()},
                {"pFlag",1},
                {"pLanguageID",_projectProvider.LangId()}
            };

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures()
                .ExecuteStoredProcedureAsync<GetEmployeeAttendanceDailyResponse>("dbo.GetEmployeeAttendanceDaily", inputParams, null);
            return result;
        }
    }
}
