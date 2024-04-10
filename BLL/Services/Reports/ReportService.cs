using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Reports;
using DataAccessLayer.DTO.WorkFlow;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;


namespace BusinessLogicLayer.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly IMapper _mapper;
        IProjectProvider _projectProvider;
        IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ReportService(IMapper mapper, IProjectProvider projectProvider, IAuthService authService, PayrolLogOnlyContext payrolLogOnlyContext, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _mapper = mapper;
            _projectProvider = projectProvider;
            _authService = authService;
            _payrolLogOnlyContext = payrolLogOnlyContext;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
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
                {"pEmployeeID",getEmployeeAttendanceDailyRequest.EmployeeID??Convert.DBNull },
                {"pFromDate",getEmployeeAttendanceDailyRequest.FromDate==null?Convert.DBNull:getEmployeeAttendanceDailyRequest.FromDate.DateToIntValue() },
                {"pToDate",getEmployeeAttendanceDailyRequest.ToDate==null
                ?Convert.DBNull:getEmployeeAttendanceDailyRequest.ToDate.DateToIntValue()},
                {"pProjectID",_projectProvider.GetProjectId()},
                {"pFlag",1},
                {"pLanguageID",_projectProvider.LangId()},
                {"pCreatedBy",Convert.DBNull },
                {"pDepartmentID" ,Convert.DBNull}
            };

            //var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures()
            //    .ExecuteStoredProcedureAsync<GetEmployeeAttendanceDailyResponse>("dbo.GetEmployeeAttendanceDaily", inputParams, null);
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures()
               .ExecuteReportStoredProcedureAsyncByADO("dbo.GetEmployeeAttendanceDaily", inputParams, null);

            if (result == null || result.Rows.Count == 0)
                return null;

            string reportPath = _hostingEnvironment.ContentRootPath + Path.Combine("Reports\\DailyAttendanceReportdetailsdefault.rdlc");
            return PublicHelper.BuildRdlcReportWithDataSourc(result, reportPath, "DsMain");
        }
    }
}
