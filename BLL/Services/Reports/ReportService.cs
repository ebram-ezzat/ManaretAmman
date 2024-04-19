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
using System.Data.Common;
using System.Data;
using BusinessLogicLayer.Services.Lookups;


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
        private readonly ILookupsService _lookupsService;

        public ReportService(IMapper mapper, IProjectProvider projectProvider, IAuthService authService, PayrolLogOnlyContext payrolLogOnlyContext,
            IConfiguration configuration, IHostingEnvironment hostingEnvironment,ILookupsService lookupsService)
        {
            _mapper = mapper;
            _projectProvider = projectProvider;
            _authService = authService;
            _payrolLogOnlyContext = payrolLogOnlyContext;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _lookupsService = lookupsService;
        }
        public async Task<object> GetEmployeeSalaryReport(GetEmployeeSalaryReportRequest getEmployeeSalaryReportRequest)
        {
            getEmployeeSalaryReportRequest.ProjectID = _projectProvider.GetProjectId();
            getEmployeeSalaryReportRequest.loginuserid = _projectProvider.UserId();
            var parameters = PublicHelper.GetPropertiesWithPrefix<GetEmployeeSalaryReportRequest>(getEmployeeSalaryReportRequest, "p");

            var (getEmployeeSalaryReportResponse, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeSalaryReportResponse>("[dbo].[GetEmployeeSalary]", parameters, null);
            return getEmployeeSalaryReportResponse.FirstOrDefault()?.EmailContent;
        }

        #region تقرير شاشة خدمات شوون الموظفين
        public async Task<object> GetEmployeeAffairsServiceReport(GetEmployeeAffairsServiceReportRequest getEmployeeAffairsServiceReportRequest)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pEmployeeHRServiceID",getEmployeeAffairsServiceReportRequest.EmployeeHRServiceID },
                 {"pEmployeeID",getEmployeeAffairsServiceReportRequest.EmployeeID??Convert.DBNull },
                {"pProjectID",_projectProvider.GetProjectId() },
                {"pLanguageID",_projectProvider.LangId() },
                {"pCreatedBy",_projectProvider.UserId()==-1? Convert.DBNull: _projectProvider.UserId()},
                {"pDepartmentID", Convert.DBNull}
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures()
              .ExecuteReportStoredProcedureAsyncByADO("dbo.GetEmployeeHRServiceReport", inputParams, null);
            if (result == null || result.Rows.Count == 0)
                return null;
            string reportPath = GetReportPathIfValid(getEmployeeAffairsServiceReportRequest.HRServiceReportName);
            if (string.IsNullOrEmpty(reportPath))
                return null;

            return getEmployeeAffairsServiceReportRequest.IsExcel ?
                     PublicHelper.BuildRdlcReportWithDataSourcExcelFormat(result, reportPath, "DsMain") :
                     PublicHelper.BuildRdlcReportWithDataSourcPDFFormat(result, reportPath, "DsMain");
        }


        #endregion


        #region التقرير اليومى
        public async Task<object> GetEmployeeAttendanceDailyReport(GetEmployeeAttendanceDailyRequest getEmployeeAttendanceDailyRequest)
        {
            var inputParams = BuildBaseReportInputParams(getEmployeeAttendanceDailyRequest);
            //var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures()
            //    .ExecuteStoredProcedureAsync<GetEmployeeAttendanceDailyResponse>("dbo.GetEmployeeAttendanceDaily", inputParams, null);
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures()
               .ExecuteReportStoredProcedureAsyncByADO("dbo.GetEmployeeAttendanceDaily", inputParams, null);

            var settingResult = await _lookupsService.GetSettings();



            string reportName = getEmployeeAttendanceDailyRequest.ReportType switch
            {
                (int)EnumReportType.Date => settingResult.DailyAttendanceReportName,
                (int)EnumReportType.Employee => settingResult.DailyAttendanceByEmployeeReportName,
                (int)EnumReportType.AllEmployees => settingResult.DailyAttendanceByAllEmployeeReportName,
                _ => null
            };

            //string reportPath = _hostingEnvironment.ContentRootPath + Path.Combine("Reports\\Ar\\DailyAttendanceReportdetailsdefault.rdlc");
            string reportPath =!string.IsNullOrEmpty(reportName) ? GetReportPath($"Reports\\Ar\\{reportName}.rdlc", $"Reports\\En\\{reportName}.rdlc"):null;
            if (result == null || result.Rows.Count == 0 || reportPath==null)
                return null;
            return getEmployeeAttendanceDailyRequest.IsExcel?PublicHelper.BuildRdlcReportWithDataSourcExcelFormat(result, reportPath, "DsMain"): PublicHelper.BuildRdlcReportWithDataSourcPDFFormat(result, reportPath, "DsMain");
        }
        #endregion
        #region التقرير اليومى التفصيلى
        public async Task<object> GetEmployeeAttendanceDailyDetailedReport(GetEmployeeAttendanceDailyDetailedReportRequest getEmployeeAttendanceDailyDetailedReportRequest)
        {
          
            return await GenerateReportAsync(
               getEmployeeAttendanceDailyDetailedReportRequest,
               "dbo.GetEmployeeAttendanceDaily",
               r => r.ReportType,
               (req, settings) => req.ReportType switch
               {
                   (int)EnumReportType.Date => settings.DailyAttendanceReportDetails,                  
                   _ => null
               }
           );
        }
        #endregion
        #region تقرير العمل الاضافى
        public async Task<object> GetEmployeeOverTimeWorkReport(GetEmployeeOverTimeWorkReportRequest getEmployeeOverTimeReportRequest)
        {
            return await GenerateReportAsync(
                 getEmployeeOverTimeReportRequest,
                 "dbo.GetEmployeeAttendanceDailyAdditional",
                 r => r.ReportType,
                 (req, settings) => req.ReportType switch
                 {
                     (int)EnumReportType.Date => settings.DailyAdditionalWorkReportName,
                     (int)EnumReportType.Employee => settings.DailyAdditionalWorkByEmployeeReportName,
                     (int)EnumReportType.AllEmployees => settings.DailyAdditionalWorkByAllEmployeeReportName,
                     _ => null
                 }
             );
        }


        #endregion
        #region تقرير التاخير الصباحى
        public async Task<object> GetEmployeeMorningLateReport(GetEmployeeMorningLateReportRequest getEmployeeMorningLateReportRequest)
        {
            return await GenerateReportAsync(
                 getEmployeeMorningLateReportRequest,
                 "dbo.GetEmployeeAttendanceDailyLate",
                 r => r.ReportType,
                 (req, settings) => req.ReportType switch
                 {
                     (int)EnumReportType.Date => settings.DailyLateReportName,
                     (int)EnumReportType.Employee => settings.DailyLateByEmployeeReportName,
                     (int)EnumReportType.AllEmployees => settings.DailyLateByAllEmployeeReportName,
                     _ => null
                 }
             );
        }
        #endregion

        #region تقرير الخروج المبكر
        public async Task<object> GetEmployeeEarlyLeaveReport(GetEmployeeEarlyLeaveReportRequest getEmployeeEarlyLeaveReportRequest)
        {
            getEmployeeEarlyLeaveReportRequest.Flag = 2;
            return await GenerateReportAsync(
                  getEmployeeEarlyLeaveReportRequest,
                  "dbo.GetEmployeeAttendanceDailyLate",
                  r => r.ReportType,
                  (req, settings) => req.ReportType switch
                  {
                      (int)EnumReportType.Date => settings.DailyEarlyLeaveReportName,
                      (int)EnumReportType.Employee => settings.DailyEarlyLeaveByEmployeeReportName,
                      (int)EnumReportType.AllEmployees => settings.DailyEarlyLeaveByAllEmployeeReportName,
                      _ => null
                  }
              );
        }
        #endregion
        #region تقرير الغيابات
        public async Task<object> GetEmployeeAbsentsReport(GetEmployeeAbsentsReportRequest getEmployeeAbsentsReportRequest)
        {
            getEmployeeAbsentsReportRequest.Flag = 2;
            return await GenerateReportAsync(
                 getEmployeeAbsentsReportRequest,
                 "dbo.GetEmployeeAttendanceDailyAbsent",
                 r => r.ReportType,
                 (req, settings) => req.ReportType switch
                 {
                     (int)EnumReportType.Date => settings.DailyAbsentEmployeesReportName,
                     (int)EnumReportType.Employee => settings.DailyAbsentByEmployeeReportName,
                     (int)EnumReportType.AllEmployees => settings.DailyAbsentByAllEmployeeReportName,
                     _ => null
                 }
             );
        }
        #endregion



        #region ReportGeneralFunctions
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

        private Dictionary<string,object> BuildBaseReportInputParams(ReportBaseFields reportBaseFields)
        {
            return  new Dictionary<string, object>()
            {
                {"pEmployeeID",reportBaseFields.EmployeeID??Convert.DBNull },
                {"pFromDate",reportBaseFields.FromDate==null?Convert.DBNull:reportBaseFields.FromDate.DateToIntValue() },
                {"pToDate",reportBaseFields.ToDate==null
                ?Convert.DBNull:reportBaseFields.ToDate.DateToIntValue()},
                {"pProjectID",_projectProvider.GetProjectId()},
                {"pFlag",reportBaseFields.Flag},
                {"pLanguageID",_projectProvider.LangId()},
                {"pCreatedBy",Convert.DBNull },
                {"pDepartmentID" ,reportBaseFields.DepartmentID??Convert.DBNull},
                {"pLoginUserID",_projectProvider.UserId()==-1?Convert.DBNull :_projectProvider.UserId()}
            };
        }

        private async Task<object> GenerateReportAsync<TRequest>(
            TRequest request,
            string storedProcedureName,
            Func<TRequest, int> reportTypeSelector,
            Func<TRequest, dynamic, string> reportNameSelector) where TRequest:ReportBaseFields // New selector function for dynamic report names
        {
                var inputParams = BuildBaseReportInputParams(request);
                var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures()
                    .ExecuteReportStoredProcedureAsyncByADO(storedProcedureName, inputParams, null);

                if (result == null || result.Rows.Count == 0)
                    return null;

                var settingResult = await _lookupsService.GetSettings();
                string reportName = reportNameSelector(request, settingResult); // Use the new selector function

                string reportPath = GetReportPathIfValid(reportName);
                if (string.IsNullOrEmpty(reportPath))
                    return null;

                return request.IsExcel ?
                       PublicHelper.BuildRdlcReportWithDataSourcExcelFormat(result, reportPath, "DsMain") :
                       PublicHelper.BuildRdlcReportWithDataSourcPDFFormat(result, reportPath, "DsMain");
        }
        private string GetReportPathIfValid(string reportName)
        {
            return !string.IsNullOrEmpty(reportName) ?
                   GetReportPath($"Reports\\Ar\\{reportName}.rdlc", $"Reports\\En\\{reportName}.rdlc") :
                   null;
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
        #endregion

    }
}
