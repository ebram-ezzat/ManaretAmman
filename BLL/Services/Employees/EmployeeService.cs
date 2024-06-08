﻿using AutoMapper;
using AutoMapper.Execution;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Locations;
using DataAccessLayer.Models;
using LanguageExt;
using LanguageExt.ClassInstances.Const;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.ReportingServices.Interfaces;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using UnauthorizedAccessException = BusinessLogicLayer.Exceptions.UnauthorizedAccessException;

namespace BusinessLogicLayer.Services.Employees;

internal class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    IProjectProvider _projectProvider;
    IAuthService _authService;
    private readonly IConfiguration _configuration;
    private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
    private readonly ILookupsService _lookupsService;
    private readonly IHostingEnvironment _hostingEnvironment;
    public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper, IProjectProvider projectProvider, IAuthService authService, PayrolLogOnlyContext payrolLogOnlyContext
        , IConfiguration configuration, ILookupsService lookupsService,IHostingEnvironment hostingEnvironment)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _projectProvider = projectProvider;
        _authService = authService;
        _payrolLogOnlyContext = payrolLogOnlyContext;
        _configuration = configuration;
        _lookupsService = lookupsService;
        _hostingEnvironment = hostingEnvironment;
    }
    public async Task<List<EmployeeLookup>> GetEmployeesProc()
    {
        int userId = _projectProvider.UserId();
        int projecId = _projectProvider.GetProjectId();
        if (userId == -1) throw new UnauthorizedAccessException("Incorrect userId from header");
        if (!_authService.IsValidUser(userId)) throw new UnauthorizedAccessException("Incorrect userId");
        //  int? employeeId = _authService.IsHr(userId);

        var parameters = new Dictionary<string, object>
                        {
                            { "pProjectID", projecId } ,
                            {"pStatusID",null },
                            {"pEmployeeID",null},
                            {"pSearch",null },
                            { "psupervisorid", null },
                            {"pLoginUserID",userId }

                        };

        var employees = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<Employee>("dbo.GetEmployees", parameters, null);
        var result = _mapper.Map<List<EmployeeLookup>>(employees.Item1);

        return result;

    }

    public async Task SaveAttendanceByUser(SaveAttendance saveAttendance)
    {
        int userId = _projectProvider.UserId();
        int projecId = _projectProvider.GetProjectId();
        if (userId == -1) throw new UnauthorizedAccessException("Incorrect userId from header");
        if (!_authService.IsValidUser(userId)) throw new UnauthorizedAccessException("Incorrect userId");

        await _payrolLogOnlyContext.GetProcedures().SaveAttendanceByUserAsync(projecId, saveAttendance.attendanceDate, saveAttendance.typeId, saveAttendance.employeeId,
            saveAttendance.macIp, saveAttendance.langtitude, saveAttendance.latitude, saveAttendance.locationId, userId);
    }

    public async Task<List<EmployeeLookup>> GetList()
    {

        //TODO seperate in single service
        int userId = _projectProvider.UserId();
        int projecId = _projectProvider.GetProjectId();
        if (userId == -1) throw new UnauthorizedAccessException("Incorrect userId from header");
        if (!_authService.IsValidUser(userId)) throw new UnauthorizedAccessException("Incorrect userId");
        int? employeeId = _authService.IsHr(userId);


        IQueryable<Employee> employees;

        employees =
            from e in _unitOfWork.EmployeeRepository.PQuery()
            join lt in _unitOfWork.LookupsRepository.PQuery() on e.DepartmentID equals lt.ID into ltGroup
            from lt in ltGroup.DefaultIfEmpty()
            where e.ProjectID == projecId && (e.EmployeeID == employeeId || lt.EmployeeID == employeeId || employeeId == null) && lt.TableName == "Department" && lt.ColumnName == "DepartmentID"
            select e;

        if (employees is null)
        {
            throw new NotFoundException("data is missing");
        }

        var result = _mapper.Map<List<EmployeeLookup>>(employees);

        return result;
    }

    public async Task<object> GetEmployeePaperProc(GetEmployeePaperRequest getEmployeePaperRequest)
    {
        dynamic obj = new ExpandoObject();
        getEmployeePaperRequest.LoginUserID = _projectProvider.UserId();
        getEmployeePaperRequest.ProjectID = _projectProvider.GetProjectId();
        var parameters = PublicHelper.GetPropertiesWithPrefix<GetEmployeePaperRequest>(getEmployeePaperRequest, "p");
        var outPutPara = new Dictionary<string, object> { { "prowcount", "int" } };
        var (employeePapersResponse, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeePaperResponse>("[dbo].[GetEmployeePaper]", parameters, outPutPara);
        int totalRecords = (int)outputValues["prowcount"];
        var totalPages = ((double)totalRecords / (double)getEmployeePaperRequest.PageSize);

        int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        obj.totalPages = roundedTotalPages;
        obj.result = employeePapersResponse;
        obj.pageIndex = getEmployeePaperRequest.PageNo;
        obj.offset = getEmployeePaperRequest.PageSize;
        return obj;
    }


    public async Task<int> DeleteEmployeePaperProc(int EmployeeId, int DetailId)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeID", EmployeeId },
            { "pDetailID", DetailId },
            { "pCreatedBy",  _projectProvider.UserId() },

        };

        // Define output parameters (optional)
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" }, // Assuming the output parameter "pError" is of type int
            // Add other output parameters as needed
        };

        // Call the ExecuteStoredProcedureAsync function
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeePaper", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];

        //check if user not HR return -3 you have no permission
        if (pErrorValue == -3)
        {
            throw new UnauthorizedAccessException("NoPermisson");
        }
        return result;

    }

    public async Task<int> SaveEmployeePaperProc(SaveEmployeePaper saveEmployeePaper)
    {
        int projecId = _projectProvider.GetProjectId();
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeID", saveEmployeePaper.EmployeeID },
            { "pPaperID", saveEmployeePaper.PaperID },
            { "pPaperPath", "" },
            { "pNotes", saveEmployeePaper.Notes },
            { "pCratedBy", _projectProvider.UserId() },
        };

        // Define output parameters (optional)
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pDetailID","int" },
            { "pError","int" }, // Assuming the output parameter "pError" is of type int
            // Add other output parameters as needed

        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertEmployeePaper", inputParams, outputParams);

        //check if user not HR return -3 you have no permission
        if (outputValues.TryGetValue("pError", out var value))
        {
            if (Convert.ToInt32(value) == -3)
            {
                throw new UnauthorizedAccessException("NoPermisson");
            }
        }

        if (saveEmployeePaper.File is not null)
        {
            var fileExtension = Path.GetExtension(saveEmployeePaper.File.FileName);

            int detailId = (int)outputValues["pDetailID"];
            outputParams["pDetailID"] = detailId;

            var settingResult =await _lookupsService.GetSettings();
            //var (settingResult, outputSetting) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetSettingsResult>("dbo.GetSettings", new Dictionary<string, object> { { "pProjectID", projecId } }, null);
            var projectPath = settingResult.AttachementPath;
            var fileName = "01" + saveEmployeePaper.EmployeeID.ToString().PadLeft(6, '0') + detailId.ToString().PadLeft(6, '0') + fileExtension;
            var filePath = projectPath + "01" + saveEmployeePaper.EmployeeID.ToString().PadLeft(6, '0') + detailId.ToString().PadLeft(6, '0') + fileExtension;

            inputParams["pPaperPath"] = filePath;

            var (resultUpdate, outputUpdate) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertEmployeePaper", inputParams, outputParams);


            //var filePath = Path.Combine(settingResult.FirstOrDefault().AttachementPath, saveEmployeePaper.File.FileName);
            using (var fileStream = saveEmployeePaper.File.OpenReadStream())
            {
                string ftpUrl = projectPath + fileName;
                //await UploadFileAsync(projectPath, fileStream, "file", "image/jpeg", fileName);
                string userName = settingResult.WindowsUserName; //_configuration["UploadServerCredentials:UserName"];
                string password = settingResult.WindowsUserPassword;// _configuration["UploadServerCredentials:Password"];
                bool IsComplete = PublicHelper.UploadFileToFtp(ftpUrl, userName, password, fileStream, fileName);
            }



            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await saveEmployeePaper.File.CopyToAsync(stream);
            //}


        }



        int pErrorValue = (int)outputValues["pError"];

        return result;
    }

    public async Task<List<EmplyeeProfileVModel>> EmployeeProfile(int EmployeeId)
    {
       
            int projecId = _projectProvider.GetProjectId();

            var parameters = new Dictionary<string, object>
                        {
                            {"pProjectID", projecId } ,
                            {"pEmployeeID",EmployeeId},
                            {"pFlag",1 },
                            {"pLanguageID", 1 } ,
                            //{"pCreatedBy", "" } ,
                            //{"pFromDate",""},
                            //{"pToDate", "" } ,
                            //{"pDepartmentID" , "" }
                        };

            var employees = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<EmployeeProfile>("dbo.GetEmployeeReport", parameters, null);

            var result = _mapper.Map<List<EmplyeeProfileVModel>>(employees.Item1);

            var setting =await _lookupsService.GetSettings();
            foreach(var item in result)
            {
                if (item.EmployeeImage != null)
                {
                    var fullPath = item.EmployeeImage.ToLower().Contains("ftp") ? item.EmployeeImage : setting?.AttachementPath + item.EmployeeImage;
                    dynamic base64  = await PublicHelper.GetFileBase64ByFtpPath(fullPath, setting.WindowsUserName, setting.WindowsUserPassword);
                    item.ImgBase64 = base64?.Base64Content;
                }
            }


        //    );
        //string reportPath = _hostingEnvironment.ContentRootPath + Path.Combine("Reports\\EmployeesReport.rdlc");
        //var base64Report = PublicHelper.BuildRdlcReportWithDataSourc(result, reportPath, "DsMain");
        return result;
       
    }
    #region شاشة خدمات شوون الموظفين
    public async Task<int> SaveEmployeeAffairsService(SaveEmployeeAffairsServices saveEmployeeAffairsService)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeHRServiceID", saveEmployeeAffairsService.EmployeeHRServiceID??Convert.DBNull },
                { "pEmployeeID", saveEmployeeAffairsService.EmployeeID??Convert.DBNull },
                { "pHRServiceID", saveEmployeeAffairsService.HRServiceID??Convert.DBNull },
                { "pMonthID", saveEmployeeAffairsService.MonthID??Convert.DBNull },
                { "pHRServiceDate", saveEmployeeAffairsService.HRServiceDate==null?Convert.DBNull:saveEmployeeAffairsService.HRServiceDate.DateToIntValue() },
                { "pReasonDesc", saveEmployeeAffairsService.Notes??Convert.DBNull },                
                { "pStatusID", saveEmployeeAffairsService.StatusID?? 1 },
                { "pYearID", saveEmployeeAffairsService.YearID??Convert.DBNull },
                { "pBankID", saveEmployeeAffairsService.BankID ??Convert.DBNull},
                { "pBranchID", saveEmployeeAffairsService.BranchID??Convert.DBNull },
                { "pServiceText", saveEmployeeAffairsService.Notes??Convert.DBNull },
                { "pAttachmentDesc", saveEmployeeAffairsService.AttachmentDesc??Convert.DBNull },      
                { "pCreatedBy", _projectProvider.UserId() } 
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {            
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeHRService", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    public async Task<dynamic> GetEmployeeAffairsService(GetEmployeeAffairsServiceRequest getEmployeeAffairsServiceRequest)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeHRServiceID", getEmployeeAffairsServiceRequest.EmployeeHRServiceID ?? Convert.DBNull },
            { "pEmployeeID", getEmployeeAffairsServiceRequest.EmployeeID ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },  // Not nullable, no need for DB null check
            { "pFromDate", getEmployeeAffairsServiceRequest.FromDate==null ? Convert.DBNull:getEmployeeAffairsServiceRequest.FromDate.DateToIntValue() },
            { "pToDate", getEmployeeAffairsServiceRequest.ToDate==null ? Convert.DBNull:getEmployeeAffairsServiceRequest.ToDate.DateToIntValue() },
            { "pStatusID", getEmployeeAffairsServiceRequest.StatusID ?? Convert.DBNull },
            { "pLanguageID", _projectProvider.LangId() }, // Not nullable, no need for DB null check
            { "pHRServiceID", getEmployeeAffairsServiceRequest.HRServiceID ?? Convert.DBNull },
            {"pLoginUserID",_projectProvider.UserId() },
            {"pPageNo" ,getEmployeeAffairsServiceRequest.PageNo},
            {"pPageSize",getEmployeeAffairsServiceRequest.PageSize }
        };
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            {"prowcount","int" }
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeAffairsServiceResponse>("dbo.GetEmployeeHRService", inputParams, outputParams);
        return PublicHelper.CreateResultPaginationObject(getEmployeeAffairsServiceRequest, result, outputValues); ;

    }

    public async Task<int> DeleteEmployeeAffairsService(DeleteEmployeeAffairsService deleteEmployeeAffairsService)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeHRServiceID", deleteEmployeeAffairsService.EmployeeHRServiceID},
                { "pEmployeeID", deleteEmployeeAffairsService.EmployeeID},
                {"pStatusID",deleteEmployeeAffairsService.StatusID },
                {"pCreatedBy",_projectProvider.UserId() }
        };
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            {"pError","int" }
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.UpdateEmployeeHRService", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    #endregion
    #region Employee evalution
    public async Task<int> SaveOrUpdateEmployeeEvaluation(SaveOrUpdateEmployeeEvaluation saveOrUpdateEmployeeEvaluation)
    {
        int id = 0;
        if (saveOrUpdateEmployeeEvaluation.CategoryId > 0)
        {
            var dbObj = _unitOfWork.EvaluationCategoryRepository.GetFirstOrDefault(x => x.CategoryId == saveOrUpdateEmployeeEvaluation.CategoryId);
            dbObj.CategoryName = saveOrUpdateEmployeeEvaluation.CategoryName;
            dbObj.StatusId = saveOrUpdateEmployeeEvaluation.StatusId;
            dbObj.ModifiedBy = _projectProvider.GetProjectId();
            dbObj.ModificationDate = DateTime.Now;
            _unitOfWork.EvaluationCategoryRepository.Update(dbObj);
            id = saveOrUpdateEmployeeEvaluation.CategoryId;
        }
        else
        {
            var employeeCategory = _mapper.Map<EvaluationCategory>(saveOrUpdateEmployeeEvaluation);

            employeeCategory.CreatedBy = _projectProvider.UserId();
            employeeCategory.CreationDate = DateTime.Now;
            employeeCategory.ProjectID = _projectProvider.GetProjectId();


            await _unitOfWork.EvaluationCategoryRepository.PInsertAsync(employeeCategory);
            id = employeeCategory.CategoryId;
        }
        await _unitOfWork.SaveAsync();

        return id;
    }

    public async Task<List<GetEmployeeEvaluation>> GetEmployeeEvaluation(GetEmployeeEvaluation getEmployeeEvaluation)
    {
        // Define filters
        var filters = new List<Expression<Func<EvaluationCategory, bool>>>
        {
            e => e.ProjectID==_projectProvider.GetProjectId(),
             //e => e.StatusId == getEmployeeEvaluation.StatusId,
            //getEmployeeEvaluation.CategoryId>0?(e => e.CategoryId==getEmployeeEvaluation.CategoryId) : null,
           // getEmployeeEvaluation.CategoryName!=null?(e => e.CategoryName==getEmployeeEvaluation.CategoryName) : null
        }
        .Concat(getEmployeeEvaluation.StatusId != null ? new[] { (Expression<Func<EvaluationCategory, bool>>)(e => e.StatusId == getEmployeeEvaluation.StatusId) } : Array.Empty<Expression<Func<EvaluationCategory, bool>>>())
        .Concat(!string.IsNullOrEmpty(getEmployeeEvaluation.CategoryName) ? new[] { (Expression<Func<EvaluationCategory, bool>>)(e => e.CategoryName == getEmployeeEvaluation.CategoryName) } : Array.Empty<Expression<Func<EvaluationCategory, bool>>>())
         .Concat(getEmployeeEvaluation.CategoryId > 0 ? new[] { (Expression<Func<EvaluationCategory, bool>>)(e => e.CategoryId == getEmployeeEvaluation.CategoryId) } : Array.Empty<Expression<Func<EvaluationCategory, bool>>>())
                .ToList();
        var dataDb = await _unitOfWork.EvaluationCategoryRepository.GetWithListOfFilters(filters);
        var result = _mapper.Map<List<GetEmployeeEvaluation>>(dataDb);
        return result;
    }

    public async Task<int> SaveOrUpdateEmployeeEvaluationQuestion(SaveOrUpdateEvaluationQuestion saveOrUpdateEvaluationQuestion)
    {
        int id = 0;
        if (saveOrUpdateEvaluationQuestion.Id > 0)
        {
            var dbObj = _unitOfWork.EvaluationQuestionRepository.GetFirstOrDefault(x => x.Id == saveOrUpdateEvaluationQuestion.Id);
            dbObj.Question = saveOrUpdateEvaluationQuestion.Question;
            dbObj.CategoryId = saveOrUpdateEvaluationQuestion.CategoryId;
            await _unitOfWork.EvaluationQuestionRepository.PUpdateAsync(dbObj);
            id = saveOrUpdateEvaluationQuestion.Id;
        }
        else
        {
            var evaluationQuestion = _mapper.Map<EvaluationQuestion>(saveOrUpdateEvaluationQuestion);

            evaluationQuestion.CreatedBy = _projectProvider.UserId();
            evaluationQuestion.CreationDate = DateTime.Now;
            evaluationQuestion.ProjectID = _projectProvider.GetProjectId();


            await _unitOfWork.EvaluationQuestionRepository.PInsertAsync(evaluationQuestion);
            id = evaluationQuestion.Id;
        }
        await _unitOfWork.SaveAsync();

        return id;
    }

    public async Task<List<GetEvaluationQuestion>> GetEmployeeEvaluationQuestion(GetEvaluationQuestion getEvaluationQuestion)
    {
        var filters = new List<Expression<Func<EvaluationQuestion, bool>>>
        {
            e => e.ProjectID==_projectProvider.GetProjectId()
        }
       .Concat(getEvaluationQuestion.CategoryId != null ? new[] { (Expression<Func<EvaluationQuestion, bool>>)(e => e.CategoryId == getEvaluationQuestion.CategoryId) } : Array.Empty<Expression<Func<EvaluationQuestion, bool>>>())       
         .Concat(getEvaluationQuestion.Id > 0 ? new[] { (Expression<Func<EvaluationQuestion, bool>>)(e => e.Id == getEvaluationQuestion.Id) } : Array.Empty<Expression<Func<EvaluationQuestion, bool>>>())
         .Concat(!string.IsNullOrEmpty(getEvaluationQuestion.Question) ? new[] { (Expression<Func<EvaluationQuestion, bool>>)(e => e.Question == getEvaluationQuestion.Question) } : Array.Empty<Expression<Func<EvaluationQuestion, bool>>>())
          .ToList();
        // Include related data (optional)
        // Prepare the includes
        Expression<Func<EvaluationQuestion, object>> includes =  entity => entity.EvaluationCategory ;
        var dataDb = await _unitOfWork.EvaluationQuestionRepository.GetWithListOfFilters(filters,null,null,null, includes);
        var result = _mapper.Map<List<GetEvaluationQuestion>>(dataDb);
        return result;
    }
    #endregion
}
