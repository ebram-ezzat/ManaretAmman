using AutoMapper;
using AutoMapper.Execution;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO.EmployeeAttendance;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.EmployeeTransaction;
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
using System.Linq;
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
        , IConfiguration configuration, ILookupsService lookupsService, IHostingEnvironment hostingEnvironment)
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

    public async Task<int> SaveAttendanceByUser(SaveAttendance saveAttendance)
    {
        int userId = _projectProvider.UserId();
        int projecId = _projectProvider.GetProjectId();
        if (userId == -1) throw new UnauthorizedAccessException("Incorrect userId from header");
        if (!_authService.IsValidUser(userId)) throw new UnauthorizedAccessException("Incorrect userId");

        return await _payrolLogOnlyContext.GetProcedures().SaveAttendanceByUserAsync(projecId, saveAttendance.attendanceDate, saveAttendance.typeId, saveAttendance.employeeId,
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

            var settingResult = await _lookupsService.GetSettings();
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

        var setting = await _lookupsService.GetSettings();
        foreach (var item in result)
        {
            if (item.EmployeeImage != null)
            {
                var fullPath = item.EmployeeImage.ToLower().Contains("ftp") ? item.EmployeeImage : setting?.AttachementPath + item.EmployeeImage;
                dynamic base64 = await PublicHelper.GetFileBase64ByFtpPath(fullPath, setting.WindowsUserName, setting.WindowsUserPassword);
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
        .Concat(!string.IsNullOrEmpty(getEmployeeEvaluation.CategoryName) ? new[] { (Expression<Func<EvaluationCategory, bool>>)(e => e.CategoryName.Contains(getEmployeeEvaluation.CategoryName)) } : Array.Empty<Expression<Func<EvaluationCategory, bool>>>())
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
        Expression<Func<EvaluationQuestion, object>> includes = entity => entity.EvaluationCategory;
        var dataDb = await _unitOfWork.EvaluationQuestionRepository.GetWithListOfFilters(filters, null, null, null, includes);
        var result = _mapper.Map<List<GetEvaluationQuestion>>(dataDb);
        return result;
    }

    public async Task<int> SaveOrUpdateEvaluationSurvey(SaveOrUpdateEvaluationSurvey saveOrUpdateEvaluationSurvey)
    {

        int id = 0;
        if (saveOrUpdateEvaluationSurvey.Id > 0)
        {
            var dbObj = _unitOfWork.EvaluationSurveyRepository.GetFirstOrDefault(x => x.Id == saveOrUpdateEvaluationSurvey.Id);
            dbObj.Name = saveOrUpdateEvaluationSurvey.Name;
            dbObj.Notes = saveOrUpdateEvaluationSurvey.Notes;
            dbObj.StatusId = saveOrUpdateEvaluationSurvey.StatusId;
            dbObj.ModificationDate = DateTime.Now;
            dbObj.ModifiedBy = _projectProvider.UserId();
            await _unitOfWork.EvaluationSurveyRepository.PUpdateAsync(dbObj);
            id = saveOrUpdateEvaluationSurvey.Id;
        }
        else
        {
            var evaluationSurvey = _mapper.Map<EvaluationSurvey>(saveOrUpdateEvaluationSurvey);
            evaluationSurvey.CreationDate = DateTime.Now;
            evaluationSurvey.CreatedBy = _projectProvider.UserId();
            evaluationSurvey.ProjectID = _projectProvider.GetProjectId();

            await _unitOfWork.EvaluationSurveyRepository.PInsertAsync(evaluationSurvey);
            id = evaluationSurvey.Id;
        }
        await _unitOfWork.SaveAsync();

        return id;
    }

    public async Task<List<GetEvaluationSurvey>> GetEvaluationSurvey(GetEvaluationSurvey getEvaluationSurvey)
    {
        var filters = new List<Expression<Func<EvaluationSurvey, bool>>>
        {
            e => e.ProjectID==_projectProvider.GetProjectId()
        }
         .Concat(getEvaluationSurvey.Id > 0 ? new[] { (Expression<Func<EvaluationSurvey, bool>>)(e => e.Id == getEvaluationSurvey.Id) } : Array.Empty<Expression<Func<EvaluationSurvey, bool>>>())
         .Concat(!string.IsNullOrEmpty(getEvaluationSurvey.Name) ? new[] { (Expression<Func<EvaluationSurvey, bool>>)(e => e.Name.Contains(getEvaluationSurvey.Name)) } : Array.Empty<Expression<Func<EvaluationSurvey, bool>>>())
          .ToList();
        // Include related data (optional)
        // Prepare the includes
        var dataDb = await _unitOfWork.EvaluationSurveyRepository.GetWithListOfFilters(filters);
        var result = _mapper.Map<List<GetEvaluationSurvey>>(dataDb);
        return result;
    }
    public async Task<int> DeleteEvaluationSurvey(DeleteEvalualtionSurvey deleteEvalualtionServey)
    {

        int id = 0;
        if (deleteEvalualtionServey.Id > 0)
        {
            var dbObj = _unitOfWork.EvaluationSurveyRepository.GetFirstOrDefault(x => x.Id == deleteEvalualtionServey.Id);
            if (dbObj != null)
            {
                _unitOfWork.EvaluationSurveyRepository.Delete(dbObj);
                id = deleteEvalualtionServey.Id;
            }

        }

        await _unitOfWork.SaveAsync();

        return id;
    }


    public async Task<int> SaveEvaluationSurveyQuestions(List<SaveEvaluationSurveyQuestions> LstQuestions)
    {
        //delete
        var dbQuestions = _unitOfWork.EvaluationSurveyQuestionsRepository.Query(x => x.SurveyId == LstQuestions.First().SurveyId).ToList();
        _unitOfWork.EvaluationSurveyQuestionsRepository.DeleteRange(dbQuestions);

        //add
        foreach (var Question in LstQuestions)
        {
            var evaluationSurveyQuestion = _mapper.Map<EvaluationSurveyQuestions>(Question);
            evaluationSurveyQuestion.CreationDate = DateTime.Now;
            evaluationSurveyQuestion.CreatedBy = _projectProvider.UserId();
            evaluationSurveyQuestion.SurveyId = Question.SurveyId;
            evaluationSurveyQuestion.CreatedBy = _projectProvider.UserId();
            evaluationSurveyQuestion.CreationDate = DateTime.Now;
            await _unitOfWork.EvaluationSurveyQuestionsRepository.InsertAsync(evaluationSurveyQuestion);
        }
        await _unitOfWork.SaveAsync();

        return 0;
    }

    public async Task<List<GetEvaluationSurveyQuestions>> GetEvaluationSurveyQuestions(GetEvaluationSurveyQuestions getEvaluationSurveyQuestions)
    {
        var filters = new List<Expression<Func<EvaluationSurveyQuestions, bool>>>
        {
            e => e.SurveyId==getEvaluationSurveyQuestions.SurveyId
        }
      .Concat(getEvaluationSurveyQuestions.CategoryId > 0 ? new[] { (Expression<Func<EvaluationSurveyQuestions, bool>>)(e => e.CategoryId == getEvaluationSurveyQuestions.CategoryId) } : Array.Empty<Expression<Func<EvaluationSurveyQuestions, bool>>>())
        .Concat(getEvaluationSurveyQuestions.Id > 0 ? new[] { (Expression<Func<EvaluationSurveyQuestions, bool>>)(e => e.Id == getEvaluationSurveyQuestions.Id) } : Array.Empty<Expression<Func<EvaluationSurveyQuestions, bool>>>())
         .ToList();
        // Include related data (optional)
        // Prepare the includes
        Expression<Func<EvaluationSurveyQuestions, object>> includeEvaluationCategory = entity => entity.EvaluationCategory;
        Expression<Func<EvaluationSurveyQuestions, object>> includeSurvey = entity => entity.EvaluationSurvey;
        Expression<Func<EvaluationSurveyQuestions, object>> includeQuestion = entity => entity.EvaluationQuestion;

        var dataDb = await _unitOfWork.EvaluationSurveyQuestionsRepository.GetWithListOfFilters(filters, null, null, null, includeEvaluationCategory, includeSurvey, includeQuestion);
        var result = _mapper.Map<List<GetEvaluationSurveyQuestions>>(dataDb);
        return result;
    }

    public async Task<List<GetEvaluationSurveySetup>> GetEvaluationSurveySetup(GetEvaluationSurveySetup getEvaluationSurveySetup)
    {
        var filters = new List<Expression<Func<EvaluationSurveySetup, bool>>>
        {
            e => e.SurveyId==getEvaluationSurveySetup.SurveyId,
            e=>e.StatusId==1//active
        }
        .Concat(getEvaluationSurveySetup.Id > 0 ? new[] { (Expression<Func<EvaluationSurveySetup, bool>>)(e => e.Id == getEvaluationSurveySetup.Id) } : Array.Empty<Expression<Func<EvaluationSurveySetup, bool>>>())
         .ToList();


        var dataDb = await _unitOfWork.EvaluationSurveySetupRepository.GetWithListOfFilters(filters);
        var result = _mapper.Map<List<GetEvaluationSurveySetup>>(dataDb);
        return result;
    }

    public async Task<int> DeleteEvaluationSurveySetup(DeleteEvaluationSurveySetup deleteEvaluationSurveySetup)
    {

        int id = 0;
        if (deleteEvaluationSurveySetup.Id > 0)
        {
            var dbObj = _unitOfWork.EvaluationSurveySetupRepository.GetFirstOrDefault(x => x.Id == deleteEvaluationSurveySetup.Id);
            if (dbObj != null)
            {
                dbObj.StatusId = 0;//Not active 
                dbObj.ModificationDate = DateTime.Now;
                dbObj.ModifiedBy = _projectProvider.UserId();
                await _unitOfWork.EvaluationSurveySetupRepository.PUpdateAsync(dbObj);
                id = deleteEvaluationSurveySetup.Id;
            }

        }

        await _unitOfWork.SaveAsync();

        return id;
    }

    public async Task<int> SaveOrUpdateEvaluationSurveySetup(SaveEvaluationSurveySetup saveOrUpdateEvaluationSurveySetup)
    {

        int id = 0;
        if (saveOrUpdateEvaluationSurveySetup.Id > 0)
        {
            var dbObj = _unitOfWork.EvaluationSurveySetupRepository.GetFirstOrDefault(x => x.Id == saveOrUpdateEvaluationSurveySetup.Id);

            var mapedobj = _mapper.Map<EvaluationSurveySetup>(saveOrUpdateEvaluationSurveySetup);

            dbObj.ToDate = mapedobj.ToDate;
            dbObj.FromDate = mapedobj.FromDate;
            dbObj.StatusId = mapedobj.StatusId;
            dbObj.SurveyId = mapedobj.SurveyId;
            dbObj.DepartmentIds = mapedobj.DepartmentIds;
            dbObj.EmployeelevelIds = mapedobj.EmployeelevelIds;
            dbObj.UsertypeData = mapedobj.UsertypeData;
            dbObj.ModificationDate = DateTime.Now;
            dbObj.ModifiedBy = _projectProvider.UserId();
            await _unitOfWork.EvaluationSurveySetupRepository.PUpdateAsync(dbObj);
            id = saveOrUpdateEvaluationSurveySetup.Id;
        }
        else
        {
            var evaluationSurveySetup = _mapper.Map<EvaluationSurveySetup>(saveOrUpdateEvaluationSurveySetup);
            evaluationSurveySetup.CreationDate = DateTime.Now;
            evaluationSurveySetup.CreatedBy = _projectProvider.UserId();
            evaluationSurveySetup.ProjectID = _projectProvider.GetProjectId();

            await _unitOfWork.EvaluationSurveySetupRepository.PInsertAsync(evaluationSurveySetup);
            id = evaluationSurveySetup.Id;
        }
        await _unitOfWork.SaveAsync();

        return id;
    }
    #endregion

    #region EmployeePenalty
    public async Task<dynamic> GetEmployeePenalty(GetEmployeePenalty getEmployeePenalty)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeePenaltyID", getEmployeePenalty.EmployeePenaltyID ?? Convert.DBNull },
            { "pEmployeeID", getEmployeePenalty.EmployeeID ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },  // Not nullable, no need for DB null check
            { "pFromDate", getEmployeePenalty.FromDate==null ? Convert.DBNull:getEmployeePenalty.FromDate.DateToIntValue() },
            { "pToDate", getEmployeePenalty.ToDate==null ? Convert.DBNull:getEmployeePenalty.ToDate.DateToIntValue() },
            { "pStatusID", getEmployeePenalty.StatusID ?? Convert.DBNull },
            { "pLanguageID", _projectProvider.LangId() }, // Not nullable, no need for DB null check
            { "pPenaltyID", getEmployeePenalty.PenaltyID ?? Convert.DBNull },
            { "pLoginUserID",_projectProvider.UserId() },
            { "pPageNo" ,getEmployeePenalty.PageNo },
            { "pPageSize",getEmployeePenalty.PageSize }
        };
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            {"prowcount","int" }
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeePenaltyResponse>("dbo.GetEmployeePenalty", inputParams, outputParams);
        return PublicHelper.CreateResultPaginationObject(getEmployeePenalty, result, outputValues); ;

    }

    public async Task<int> SaveEmployeePenalty(SaveEmployeePenalty saveEmployeePenalty)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeePenaltyID", saveEmployeePenalty.EmployeePenaltyID??Convert.DBNull },
                { "pEmployeeID", saveEmployeePenalty.EmployeeID??Convert.DBNull },
                { "pPenaltyID", saveEmployeePenalty.PenaltyID??Convert.DBNull },
                { "pDayCount", saveEmployeePenalty.DayCount??Convert.DBNull },
                { "pPenaltyDate", saveEmployeePenalty.PenaltyDate==null?Convert.DBNull:saveEmployeePenalty.PenaltyDate.DateToIntValue() },
                { "pReasonDesc", saveEmployeePenalty.ReasonDesc??Convert.DBNull },
                { "pCreatedBy", _projectProvider.UserId() },
                { "pStatusID", saveEmployeePenalty.StatusID??Convert.DBNull },
                { "pAppliedPenaltyCategoryTypeID", saveEmployeePenalty.AppliedPenaltyCategoryTypeID ??Convert.DBNull}
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeePenalty", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    public async Task<int> UpdateEmployeePenalty(SaveEmployeePenalty updateEmployeePenalty)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeePenaltyID", updateEmployeePenalty.EmployeePenaltyID??Convert.DBNull },
                { "pEmployeeID", updateEmployeePenalty.EmployeeID??Convert.DBNull },
                { "pPenaltyID", updateEmployeePenalty.PenaltyID??Convert.DBNull },
                { "pDayCount", updateEmployeePenalty.DayCount??Convert.DBNull },
                { "pPenaltyDate", updateEmployeePenalty.PenaltyDate==null?Convert.DBNull:updateEmployeePenalty.PenaltyDate.DateToIntValue() },
                { "pReasonDesc", updateEmployeePenalty.ReasonDesc??Convert.DBNull },
                { "pCreatedBy", _projectProvider.UserId() },
                { "pStatusID", updateEmployeePenalty.StatusID?? 1 },
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeePenalty", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    public async Task<int> ChangeStatusEmployeePenalty(SaveEmployeePenalty updateEmployeePenalty)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeePenaltyID", updateEmployeePenalty.EmployeePenaltyID??Convert.DBNull },
                { "pEmployeeID", updateEmployeePenalty.EmployeeID??Convert.DBNull },
                { "pCreatedBy", _projectProvider.UserId() },
                { "pStatusID", updateEmployeePenalty.StatusID?? 1 },
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.UpdateEmployeePenalty", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }
    #endregion

    #region employeeShifts
    public async Task<dynamic> GetEmployeeShifts(GetEmployeeShifts getEmployeeShifts)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pEmployeeID", getEmployeeShifts.EmployeeID ?? Convert.DBNull },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeShiftsResponse>("dbo.GetEmployeeShiftCheck", inputParams, null);
        return result;

    }

    public async Task<int> SaveEmployeeShifts(GetEmployeeShifts saveEmployeeShifts)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", saveEmployeeShifts.EmployeeID??Convert.DBNull },
                { "pShiftID", saveEmployeeShifts.ShiftID??Convert.DBNull },
                { "pCreatedBy", _projectProvider.UserId() }
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeShiftCheck", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }


    #endregion
    #region Employee Attandance Table (شاشة جدول الحضور )
    public async Task<List<GetEmployeeAttandanceShiftOutput>> GetEmployeeAttandanceShift(GetEmployeeAttandanceShiftInput getEmployeeAttandanceShiftInput)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pEmployeeID", getEmployeeAttandanceShiftInput.EmployeeID ?? Convert.DBNull },
            { "pEmployeeShiftID", getEmployeeAttandanceShiftInput.EmployeeShiftID ?? Convert.DBNull },
            { "pFromDate", getEmployeeAttandanceShiftInput.FromDate!=null?getEmployeeAttandanceShiftInput.FromDate.DateToIntValue() : Convert.DBNull },
            { "pToDate", getEmployeeAttandanceShiftInput.ToDate!=null?getEmployeeAttandanceShiftInput.ToDate.DateToIntValue() : Convert.DBNull },
            { "pCreatedBy", getEmployeeAttandanceShiftInput.CreatedBy?? Convert.DBNull },
            { "pLoginUserID",  _projectProvider.UserId()},

        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeAttandanceShiftOutput>("dbo.GetEmployeeShifts", inputParams, null);
        return result;
    }

    public async Task<int> DeleteEmployeeAttandanceShifts(DeleteEmployeeAttandanceShifts deleteEmployeeAttandanceShifts)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeShiftID", deleteEmployeeAttandanceShifts.EmployeeShiftID},

            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeeShifts", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    public async Task<int> SaveEmployeeAttandanceShifts(SaveEmployeeAttandanceShiftInput saveEmployeeAttandanceShiftInput)
    {
        int employeeShiftId = 0;
        foreach (var item in saveEmployeeAttandanceShiftInput.EmployeeIDs)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", item},
                {"pShiftID",saveEmployeeAttandanceShiftInput.ShiftID??Convert.DBNull },
                {"pFromDate",saveEmployeeAttandanceShiftInput.FromDate!=null?saveEmployeeAttandanceShiftInput.FromDate.DateToIntValue():Convert.DBNull  },
                {"pToDate",saveEmployeeAttandanceShiftInput.ToDate!=null?saveEmployeeAttandanceShiftInput.ToDate.DateToIntValue():Convert.DBNull  },
                {"pCreatedBy",_projectProvider.UserId() },
                {"pProjectID",_projectProvider.GetProjectId() }


            };

            Dictionary<string, object> outputParams = new Dictionary<string, object>
           {
            { "pError","int" },
             {"pEmployeeShiftID","int" }
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeShifts", inputParams, outputParams);
            employeeShiftId = (int)outputValues["pEmployeeShiftID"];
        }
        return employeeShiftId;
    }
    #endregion

    #region Employee Transaction

    public async Task<List<GetEmployeeTransactionOutput>> GetEmployeeTransaction(GetEmployeeTransactionInput getEmployeeTransactionInput)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeTransactionID", getEmployeeTransactionInput.EmployeeTransactionID ?? Convert.DBNull },
            { "pEmployeeID", getEmployeeTransactionInput.EmployeeID ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pFromDate", getEmployeeTransactionInput.FromDate!=null?getEmployeeTransactionInput.FromDate.DateToIntValue() : Convert.DBNull },
            { "pToDate", getEmployeeTransactionInput.ToDate!=null?getEmployeeTransactionInput.ToDate.DateToIntValue() : Convert.DBNull },
            {"pFlag",1 },
            { "pTransactionTypeID", getEmployeeTransactionInput.TransactionTypeID ?? Convert.DBNull },
            { "pLanguageID", _projectProvider.LangId() },
            { "pLoginUserID", _projectProvider.UserId() },
            { "pDepartmentID", getEmployeeTransactionInput.DepartmentID ?? Convert.DBNull },
            

        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeTransactionOutput>("dbo.GetEmployeeTransaction", inputParams, null);
        return result;
    }

    public async Task<int> DeleteEmployeeTransaction(DeleteEmployeeTransaction deleteEmployeeTransaction)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeTransactionID", deleteEmployeeTransaction.EmployeeTransactionID},
                { "pProjectID", _projectProvider.GetProjectId() },

            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeeTransaction", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    public async Task<int> SaveEmployeeTransaction(SaveEmployeeTransaction saveEmployeeTransaction)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeTransactionID", saveEmployeeTransaction.EmployeeTransactionID??Convert.DBNull },
                { "pEmployeeID", saveEmployeeTransaction.EmployeeID??Convert.DBNull },
               { "pTransactionDate", saveEmployeeTransaction.TransactionDate!=null?saveEmployeeTransaction.TransactionDate.DateToIntValue() : Convert.DBNull },
               { "pTransactionTypeID", saveEmployeeTransaction.TransactionTypeID??Convert.DBNull },
               { "pTransactionInMinutes", saveEmployeeTransaction.TransactionInMinutes??Convert.DBNull },
               { "pNotes", saveEmployeeTransaction.Notes??Convert.DBNull },
               { "pCreatedBy", _projectProvider.UserId() },
               { "pBySystem", saveEmployeeTransaction.BySystem??Convert.DBNull },
               { "pRelatedToDate", saveEmployeeTransaction.RelatedToDate??Convert.DBNull },
               { "pProjectID",  _projectProvider.GetProjectId() },
               { "pByPayroll", saveEmployeeTransaction.ByPayroll??Convert.DBNull },

            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeTransaction", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }
    #endregion
}
