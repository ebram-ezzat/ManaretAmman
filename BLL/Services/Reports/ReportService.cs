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
                {"pDepartmentID" ,getEmployeeAttendanceDailyRequest.DepartmentID??Convert.DBNull}
            };

            //var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures()
            //    .ExecuteStoredProcedureAsync<GetEmployeeAttendanceDailyResponse>("dbo.GetEmployeeAttendanceDaily", inputParams, null);
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures()
               .ExecuteReportStoredProcedureAsyncByADO("dbo.GetEmployeeAttendanceDaily", inputParams, null);
          
            //string reportPath = _hostingEnvironment.ContentRootPath + Path.Combine("Reports\\Ar\\DailyAttendanceReportdetailsdefault.rdlc");
            string reportPath = GetReportPath("Reports\\Ar\\DailyAttendanceReportdetailsdefault.rdlc", "Reports\\En\\DailyAttendanceReportdetailsdefault.rdlc");
            if (result == null || result.Rows.Count == 0 || reportPath==null)
                return null;
            return PublicHelper.BuildRdlcReportWithDataSourc(result, reportPath, "DsMain");
        }

       
        //private string GetReportPathFromAppSetting(string reportConfigName,string DefaultFullPath=null)
        //{
        //    var basePath = _hostingEnvironment.ContentRootPath; 
        //    var languageId = _projectProvider.LangId();
        //    var languageName = Enum.GetName(typeof(EnumLangId), languageId);

        //    if (languageName == null)
        //    {
        //        throw new ArgumentException("Invalid language ID");
        //    }

        //    var reportKey = $"ReportPaths:{languageName}:{reportConfigName}";
        //    var reportPath = _configuration.GetValue<string>(reportKey);

        //    if (!string.IsNullOrEmpty(reportPath))
        //    {
        //        return basePath + Path.Combine(reportPath);
        //        //return Path.Combine(basePath, reportPath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        //    }
        //    return string.IsNullOrEmpty(DefaultFullPath) ? null : basePath + Path.Combine(DefaultFullPath);

        //}
        private string GetReportPath(string reportArFullPath = null, string reportEnFullPath = null, string defaultFullPath = null)
        {
            var basePath = _hostingEnvironment.ContentRootPath;
            var languageId = _projectProvider.LangId();
           
            // Select the report path based on the language
            string selectedPath = languageId switch
            {
                (int)EnumLangId.En => reportEnFullPath,
                (int)EnumLangId.Ar => reportArFullPath,
                _=>null
            };

            // Return the combined path, using the default path if no specific language path is provided
            if (!string.IsNullOrEmpty(selectedPath))
            {
                //return basePath + Path.Combine(selectedPath);
                 return Path.Combine(basePath, selectedPath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            }
            return !string.IsNullOrEmpty(defaultFullPath) ? Path.Combine(basePath, defaultFullPath.Replace("/", Path.DirectorySeparatorChar.ToString())) : null;
        }


    }
}
