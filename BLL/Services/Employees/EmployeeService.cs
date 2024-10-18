using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO.EmployeeAttendance;
using DataAccessLayer.DTO.EmployeeContract;
using DataAccessLayer.DTO.EmployeeDeductions;
using DataAccessLayer.DTO.EmployeeIncrease;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.EmployeeSalary;
using DataAccessLayer.DTO.EmployeeTransaction;
using DataAccessLayer.DTO.EmployeeVacations;
using DataAccessLayer.DTO.Locations;
using DataAccessLayer.Models;
using LanguageExt;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
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
        return PublicHelper.CreateResultPaginationObject(getEmployeePenalty, result, outputValues);

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
        if(getEmployeeShifts.EmployeeID != null && getEmployeeShifts.EmployeeID=="-1")
        {
            var (resultNaga, outputValuesNaga) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeShiftsResponseNagative>("dbo.GetEmployeeShiftCheck", inputParams, null);
            return resultNaga;
        }
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

    public async Task<dynamic> GetEmployeeTransaction(GetEmployeeTransactionInput getEmployeeTransactionInput)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeTransactionID", getEmployeeTransactionInput.EmployeeTransactionID ?? Convert.DBNull },
            { "pEmployeeID", getEmployeeTransactionInput.EmployeeID ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pFromDate", getEmployeeTransactionInput.FromDate!=null?getEmployeeTransactionInput.FromDate.DateToIntValue() : Convert.DBNull },
            { "pToDate", getEmployeeTransactionInput.ToDate!=null?getEmployeeTransactionInput.ToDate.DateToIntValue() : Convert.DBNull },
            {"pFlag",getEmployeeTransactionInput.Flag??1 },
            { "pTransactionTypeID", getEmployeeTransactionInput.TransactionTypeID ?? Convert.DBNull },
            { "pLanguageID", _projectProvider.LangId() },
            { "pLoginUserID", _projectProvider.UserId() },
            { "pDepartmentID", getEmployeeTransactionInput.DepartmentID ?? Convert.DBNull },
            { "pPageNo", getEmployeeTransactionInput.PageNo },
            { "pPageSize", getEmployeeTransactionInput.PageSize },


        };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            {"prowcount","int" }
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeTransactionOutput>("dbo.GetEmployeeTransaction", inputParams, outputParams);
        return PublicHelper.CreateResultPaginationObject(getEmployeeTransactionInput, result, outputValues);
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

    public async Task<int> SaveEmployeeTransactionAuto(SaveEmployeeTransactionAutoInput saveEmployeeTransactionAutoInput)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", saveEmployeeTransactionAutoInput.EmployeeID??Convert.DBNull },
               { "pTransactionDate", saveEmployeeTransactionAutoInput.TransactionDate.DateToIntValue()?? Convert.DBNull },
               { "pTransactionTypeID", saveEmployeeTransactionAutoInput.TransactionTypeID??Convert.DBNull },
               { "pTransactionInMinutes", saveEmployeeTransactionAutoInput.TransactionInMinutes??Convert.DBNull },
               { "pNotes", saveEmployeeTransactionAutoInput.Notes??Convert.DBNull },
               { "pCreatedBy", _projectProvider.UserId() },
               { "pBySystem", saveEmployeeTransactionAutoInput.BySystem??Convert.DBNull },
               { "pRelatedToDate", saveEmployeeTransactionAutoInput.RelatedToDate??Convert.DBNull },
               { "pProjectID",  _projectProvider.GetProjectId() },

            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
           { "pEmployeeTransactionID",  "int"},

            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeTransactionAuto", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }
    #endregion

    #region Employee Salary

    public async Task<List<GetEmployeeSalaryOutput>> GetEmployeeSalary(GetEmployeeSalaryInput getEmployeeSalaryInput)
    {
        Dictionary<string, object> inputParams;
        inputParams = new Dictionary<string, object>
            {
                { "pProjectID", _projectProvider.GetProjectId() },
                //{ "pEmployeeID", getEmployeeSalaryInput.EmployeeID ?? Convert.DBNull },
                { "pStatusID", getEmployeeSalaryInput.StatusID ?? Convert.DBNull },
                { "pTypeID", 0 },
                { "pCurrentYearID", getEmployeeSalaryInput.CurrentYearID ?? Convert.DBNull },
                { "pCurrentMonthID", getEmployeeSalaryInput.CurrentMonthID ?? Convert.DBNull },
                { "pLanguageID", _projectProvider.LangId() },
                { "pDepartmentID", getEmployeeSalaryInput.DepartmentID ?? Convert.DBNull },
                { "pDailyWork", 0 },
                { "ploginuserid", _projectProvider.UserId() },
            };

        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeSalaryOutput>("dbo.GetEmployeeSalary", inputParams, null);
        return result;
    }
    public async Task<GetEmployeeSalaryDetailsOutput> GetEmployeeSalaryDetails(GetEmployeeSalaryInput getEmployeeSalaryInput)
    {
        Dictionary<string, object> inputParams;
        inputParams = new Dictionary<string, object>
            {
                { "pProjectID", _projectProvider.GetProjectId() },
                { "pEmployeeID", getEmployeeSalaryInput.EmployeeID ?? Convert.DBNull },
                { "pStatusID", getEmployeeSalaryInput.StatusID ?? Convert.DBNull },
                //{ "pTypeID", getEmployeeSalaryInput.TypeID ?? Convert.DBNull },
                { "pCurrentYearID", getEmployeeSalaryInput.CurrentYearID ?? Convert.DBNull },
                { "pCurrentMonthID", getEmployeeSalaryInput.CurrentMonthID ?? Convert.DBNull },
                { "pLanguageID", _projectProvider.LangId() },
                { "pDepartmentID", getEmployeeSalaryInput.DepartmentID ?? Convert.DBNull },
                //{ "pDailyWork", getEmployeeSalaryInput.DailyWork ?? Convert.DBNull },
                { "ploginuserid", _projectProvider.UserId() },
            };

        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeSalaryOutput>("dbo.GetEmployeeSalary", inputParams, null);
        GetEmployeeSalaryDetailsOutput DetailsOutput = new GetEmployeeSalaryDetailsOutput();
        DetailsOutput.BasicSalary = result.Where(x => x.SubTypeID == 0 && x.TypeID == 1).Sum(x => x.Amount);
        DetailsOutput.OverTime = result.Where(x => x.SubTypeID == -101 && x.TypeID == 2).Sum(x => x.Amount);
        DetailsOutput.advances = result.Where(x => x.SubTypeID == 0 && x.TypeID == 4).Sum(x => x.Amount);
        DetailsOutput.NetSalary = result.Where(x => x.TypeID == 0).Sum(x => x.Amount);
        DetailsOutput.AllowancesTable = result.Where(x => x.TypeID == 2 && x.SubTypeID != 0 && x.SubTypeID != 101)
        .Select(x => Tuple.Create(x.SUBTYpeDesc, x.Amount))
        .ToList();

        DetailsOutput.DeductionsTable = result.Where(x => x.SubTypeID != 0 && x.TypeID == 3)
       .Select(x => Tuple.Create(x.SUBTYpeDesc, x.Amount))
       .ToList();
        return DetailsOutput;
    }

    public async Task<int> DeleteCancelSalary(DeleteCancelSalary deleteCancelSalary)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", deleteCancelSalary.EmployeeID??Convert.DBNull },
                { "pProjectID",  _projectProvider.GetProjectId() },
                { "pCurrentMonthID",  deleteCancelSalary.CurrentMonthID??Convert.DBNull },
                { "pCurrentYearID",  deleteCancelSalary.CurrentYearID??Convert.DBNull },
                { "pDepartmentID",  deleteCancelSalary.DepartmentID??Convert.DBNull },
                { "pDailyWork",  deleteCancelSalary.DailyWork??Convert.DBNull },
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeeSalary", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    public async Task<int> CalculateEmployeeSalary(CalculateEmployeeSalary calculateEmployeeSalary)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pProjectID",  _projectProvider.GetProjectId() },
                { "pEmployeeID", calculateEmployeeSalary.EmployeeID??Convert.DBNull },
                { "pCurrentMonthID",  calculateEmployeeSalary.CurrentMonthID??Convert.DBNull },
                { "pCurrentYearID",  calculateEmployeeSalary.CurrentYearID??Convert.DBNull },
                { "pFromDate", calculateEmployeeSalary.FromDate!=null?calculateEmployeeSalary.FromDate.DateToIntValue() : Convert.DBNull },
                { "pToDate", calculateEmployeeSalary.ToDate!=null?calculateEmployeeSalary.ToDate.DateToIntValue() : Convert.DBNull },
                { "pCreatedBy", _projectProvider.UserId() },
                { "pDepartmentID",  calculateEmployeeSalary.DepartmentID??Convert.DBNull },
                { "pDailyWork",  calculateEmployeeSalary.DailyWork??Convert.DBNull },
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.CalculateEmployeeSalary", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }



    #endregion
    #region Employee Allownances (شاشة علاوات الموظفين)
    public async Task<dynamic> GetEmployeeAllowancesMainScreen(GetEmployeeAllowancesInput getEmployeeAllowancesInput)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeAllowanceID", getEmployeeAllowancesInput.EmployeeAllowanceID ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pEmployeeID", getEmployeeAllowancesInput.EmployeeID ?? Convert.DBNull },
            { "pNatureID", getEmployeeAllowancesInput.NatureID ?? Convert.DBNull },
            { "pLanguageID", _projectProvider.LangId() },
            { "pFlag", getEmployeeAllowancesInput.Flag },
            { "pAllowanceID", getEmployeeAllowancesInput.AllowanceID ?? Convert.DBNull },
            { "pFromDate", getEmployeeAllowancesInput.FromDate.DateToIntValue() ?? Convert.DBNull },
            { "pToDate", getEmployeeAllowancesInput.ToDate.DateToIntValue() ?? Convert.DBNull },
            { "pDepartmentID", getEmployeeAllowancesInput.DepartmentID ?? Convert.DBNull },
            { "pLoginUserID", _projectProvider.UserId()},
            { "pPageNo", getEmployeeAllowancesInput.PageNo },
            { "pPageSize", getEmployeeAllowancesInput.PageSize },

        };
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "prowcount","int" },
        };

        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeAllowancesMainScreenOutput>("dbo.GetEmployeeAllowances", inputParams, outputParams);
        return PublicHelper.CreateResultPaginationObject(getEmployeeAllowancesInput, result, outputValues);
    }

    public async Task<dynamic> GetEmployeeAllowancesPopupScreen(GetEmployeeAllowancesInput getEmployeeAllowancesInput)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeAllowanceID", getEmployeeAllowancesInput.EmployeeAllowanceID ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pEmployeeID", getEmployeeAllowancesInput.EmployeeID ?? Convert.DBNull },
            { "pNatureID", getEmployeeAllowancesInput.NatureID ?? Convert.DBNull },
            { "pLanguageID", _projectProvider.LangId() },
            { "pFlag", getEmployeeAllowancesInput.Flag },
            { "pAllowanceID", getEmployeeAllowancesInput.AllowanceID ?? Convert.DBNull },
            { "pFromDate", getEmployeeAllowancesInput.FromDate.DateToIntValue() ?? Convert.DBNull },
            { "pToDate", getEmployeeAllowancesInput.ToDate.DateToIntValue() ?? Convert.DBNull },
            { "pDepartmentID", getEmployeeAllowancesInput.DepartmentID ?? Convert.DBNull },
            { "pLoginUserID", _projectProvider.UserId()},
            { "pPageNo", getEmployeeAllowancesInput.PageNo },
            { "pPageSize", getEmployeeAllowancesInput.PageSize },

        };
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "prowcount","int" },
        };

        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeAllowancesPopupOutput>("dbo.GetEmployeeAllowances", inputParams, outputParams);
        return PublicHelper.CreateResultPaginationObject(getEmployeeAllowancesInput, result, outputValues);
    }

    public async Task<int> DeleteEmployeeAllowances(DeleteEmployeeAllowances deleteEmployeeAllowances)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", deleteEmployeeAllowances.EmployeeID},
                { "pEmployeeAllowanceID", deleteEmployeeAllowances.EmployeeAllowanceID },

            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeeAllowances", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }
    public async Task<int> UpdateEmployeeAllowances(UpdateEmployeeAllowances updateEmployeeAllowances)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pAllowanceID", updateEmployeeAllowances.AllowanceID},
                { "pEmployeeAllowanceID", updateEmployeeAllowances.EmployeeAllowanceID??Convert.DBNull },
                { "pEndDate", updateEmployeeAllowances.EndDate.DateToIntValue()??Convert.DBNull },
                { "pProjectID", _projectProvider.GetProjectId() },
                { "pCreatedBy", _projectProvider.UserId() },
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.UpdateEmployeeAllowances", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }
    public async Task<int> SaveEmployeeAllowances(SaveEmployeeAllowances saveEmployeeAllowances)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID ", saveEmployeeAllowances.EmployeeID??Convert.DBNull},
                { "pAllowanceID", saveEmployeeAllowances.AllowanceID??Convert.DBNull },
                { "pEndDate", saveEmployeeAllowances.EndDate.DateToIntValue()??Convert.DBNull },
                { "pStartDate", saveEmployeeAllowances.StartDate.DateToIntValue()??Convert.DBNull },
               { "pAmount", saveEmployeeAllowances.Amount??Convert.DBNull },
                { "pCalculatedWithOverTime", saveEmployeeAllowances.CalculatedWithOverTime??Convert.DBNull },
                { "pProjectID", _projectProvider.GetProjectId() },
                { "pCreatedBy", _projectProvider.UserId() },
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pEmployeeAllowanceID","int"},
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeAllowances", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }
    public async Task<dynamic> GetEmployeeAllowancesDeductionDDL(GetAllowanceDeductionInput getAllowanceDeductionInput)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pAllowanceID", getAllowanceDeductionInput.AllowanceID ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pNatureID", getAllowanceDeductionInput.NatureID },
            { "pSearch", getAllowanceDeductionInput.Search }

        };


        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetAllowanceDeductionOutput>("dbo.GetAllowance_deduction", inputParams, null);
        return result;
    }


    #endregion

    #region اقتطاعات الموظفين

    public async Task<dynamic> GetEmployeeDeductionsMainScreen(GetEmployeeDeductionsInput getEmployeeDeductionsInput)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeDeductionID", getEmployeeDeductionsInput.EmployeeDeductionID ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pEmployeeID", getEmployeeDeductionsInput.EmployeeID ?? Convert.DBNull },
            { "pNatureID", getEmployeeDeductionsInput.NatureID ?? Convert.DBNull },
            { "pFlag", getEmployeeDeductionsInput.Flag },
            { "pAllowanceID", getEmployeeDeductionsInput.AllowanceID ?? Convert.DBNull },
            { "pFromDate", getEmployeeDeductionsInput.FromDate.DateToIntValue() ?? Convert.DBNull },
            { "pToDate", getEmployeeDeductionsInput.ToDate.DateToIntValue() ?? Convert.DBNull },
            { "pDepartmentID", getEmployeeDeductionsInput.DepartmentID ?? Convert.DBNull },
            { "pLoginUserID", _projectProvider.UserId()},
            { "pPageNo", getEmployeeDeductionsInput.PageNo },
            { "pPageSize", getEmployeeDeductionsInput.PageSize },

        };
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "prowcount","int" },
        };

        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeDeductionsMainScreenOutput>("dbo.GetEmployeeDeductions", inputParams, outputParams);
        return PublicHelper.CreateResultPaginationObject(getEmployeeDeductionsInput, result, outputValues);
    }

    public async Task<dynamic> GetEmployeeDeductionsPopupScreen(GetEmployeeDeductionsInput getEmployeeDeductionsInput)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeDeductionID", getEmployeeDeductionsInput.EmployeeDeductionID ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pEmployeeID", getEmployeeDeductionsInput.EmployeeID ?? Convert.DBNull },
            { "pNatureID", getEmployeeDeductionsInput.NatureID ?? Convert.DBNull },
            { "pFlag", getEmployeeDeductionsInput.Flag },
            { "pAllowanceID", getEmployeeDeductionsInput.AllowanceID ?? Convert.DBNull },
            { "pFromDate", getEmployeeDeductionsInput.FromDate.DateToIntValue() ?? Convert.DBNull },
            { "pToDate", getEmployeeDeductionsInput.ToDate.DateToIntValue() ?? Convert.DBNull },
            { "pDepartmentID", getEmployeeDeductionsInput.DepartmentID ?? Convert.DBNull },
            { "pLoginUserID", _projectProvider.UserId()},
            { "pPageNo", getEmployeeDeductionsInput.PageNo },
            { "pPageSize", getEmployeeDeductionsInput.PageSize },

        };
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "prowcount","int" },
        };

        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeDeductionsPopupOutput>("dbo.GetEmployeeDeductions", inputParams, outputParams);
        return PublicHelper.CreateResultPaginationObject(getEmployeeDeductionsInput, result, outputValues);
    }

    public async Task<int> DeleteEmployeeDeductions(DeleteEmployeeDeductions deleteEmployeeDeductions)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", deleteEmployeeDeductions.EmployeeID},
                { "pEmployeeDeductionID", deleteEmployeeDeductions.EmployeeDeductionID },

            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeeDeductions", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    public async Task<int> UpdateEmployeeDeductions(UpdateEmployeeDeductions updateEmployeeDeductions)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                //{ "pDeductionID", updateEmployeeDeductions.AllowanceID},
                { "pEmployeeDeductionID", updateEmployeeDeductions.EmployeeAllowanceID??Convert.DBNull },
                { "pEndDate", updateEmployeeDeductions.EndDate.DateToIntValue()??Convert.DBNull },
                { "pCreatedBy", _projectProvider.UserId() },
                { "pProjectID", _projectProvider.GetProjectId() }
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.UpdateEmployeeDeduction", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    public async Task<int> SaveEmployeeDeductions(SaveEmployeeDeductions saveEmployeeDeductions)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID ", saveEmployeeDeductions.EmployeeID??Convert.DBNull},
                { "pDeductionID", saveEmployeeDeductions.DeductionID??Convert.DBNull },
                { "pEndDate", saveEmployeeDeductions.EndDate.DateToIntValue()??Convert.DBNull },
                { "pStartDate", saveEmployeeDeductions.StartDate.DateToIntValue()??Convert.DBNull },
               { "pAmount", saveEmployeeDeductions.Amount??Convert.DBNull },
                //{ "pCalculatedWithOverTime", saveEmployeeDeductions.CalculatedWithOverTime??Convert.DBNull },
                { "pProjectID", _projectProvider.GetProjectId() },
                { "pCreatedBy", _projectProvider.UserId() },
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pEmployeeDeductionID","int"},
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeDeductions", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    #endregion

    #region Adding Employess(شاشة اضافة موظفين )
    public async Task<dynamic> GetEmployees(GetEmployeesInput getEmployeesInput)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pStatusID", getEmployeesInput.StatusID ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pEmployeeID", getEmployeesInput.EmployeeID ?? Convert.DBNull },
            { "pDepartmentID", getEmployeesInput.DepartmentID ?? Convert.DBNull },
            { "pCreatedBy", getEmployeesInput.CreatedBy ?? Convert.DBNull },
            { "psupervisorid", getEmployeesInput.SupervisorID ?? Convert.DBNull },
            { "pSearch", getEmployeesInput.Search ?? Convert.DBNull },
            { "pYearID", getEmployeesInput.YearID ?? Convert.DBNull },
            { "pLoginUserID", _projectProvider.UserId()},
            { "pPageNo", getEmployeesInput.PageNo },
            { "pPageSize", getEmployeesInput.PageSize },

        };
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "prowcount","int" },
        };

        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeesOutput>("dbo.GetEmployees", inputParams, outputParams);
        return PublicHelper.CreateResultPaginationObject(getEmployeesInput, result, outputValues);
    }
    public async Task<int> DeleteEmployeeWithRelatedData(DeleteEmployeeWithRelatedData deleteEmployeeWithRelatedDate)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", deleteEmployeeWithRelatedDate.EmployeeId},

            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        //حذف العلاوات
        var (resultAllowances, outputValuesAllowances) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeeAllowances", inputParams, outputParams);
        int pErrorValueAllowances = (int)outputValuesAllowances["pError"];
        if (pErrorValueAllowances < 1)
        {
            throw new Exception("error on delete employee Allowances(العلاوات) information data");
        }
        //حذف الاقطاعات
        var (resultDeductions, outputValuesDeductions) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeeDeductions", inputParams, outputParams);
        int pErrorValueDeductions = (int)outputValuesDeductions["pError"];
        if (pErrorValueDeductions < 1)
        {
            throw new Exception("error on delete employee Deductions(الاقتطاعات) information data");
        }
        //حذف الموظف
        var (resultEmployee, outputValuesEmployee) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployees", inputParams, outputParams);
        int pErrorValueEmployee = (int)outputValuesEmployee["pError"];
        if (pErrorValueEmployee < 1)
        {
            throw new Exception("error on delete employee main information data");
        }
        return 1;//suceess
    }
    public async Task<int> SaveOrUpdateEmployee(SaveOrUpdateEmployeeAllData saveOrUpdateEmployeeAllData)
    {
        //save main emplyee main Info(معلومات الموظف الاساسية)
        int employerId = await SaveOrUpdateEmployeeMainInFormation(saveOrUpdateEmployeeAllData.SaveOrUpdateEmployeeInFormation);
        if (employerId < 1)
        {
            if(employerId ==-3)
            {
                throw new Exception("error on saveing employee main information data, EmployeeNumber Existed Before,Error Code -3 ");

            }
            if(employerId==-4)
            {
                throw new Exception("error on saveing employee main information data, TheContract Existed Before at same date,Error Code -4 ");

            }
            if (employerId == -5)
            {
                throw new Exception("error on saveing employee main information data, UserName Existed Before,Error Code -5");

            }
            throw new Exception("error on saveing employee main information data");
        }
        //save EmployeeContracts(معلومات العقد)
        int returnedContractIdOrError = await SaveOrUpdateContractInformation(saveOrUpdateEmployeeAllData.SaveOrUpdateEmployeeContract, employerId);
        if (returnedContractIdOrError < 1)
        {
            throw new Exception("error on saveing EmployeeContracts information data");
        }
        //save emplyee Allowances(العلاوات)
        int perrorAllowances = await SaveOrUpdateEmployeeAllowanceInformation(saveOrUpdateEmployeeAllData.SaveOrUpdateEmployeeAllowances, employerId);
        if (perrorAllowances < 1)
        {
            throw new Exception("error on saveing employee Allowances(العلاوات) information data");
        }
        //save emplyee Deductions(الاقتطاعات)
        int perrorDeductions = await SaveOrUpdateEmployeeDeductionInformation(saveOrUpdateEmployeeAllData.SaveOrUpdateEmployeeDeductions, employerId);
        if (perrorDeductions < 1)
        {
            throw new Exception("error on saveing employee Deductions(الاقتطاعات) information data");
        }
        //save employee attandance (الحضور)
        int perrorAttandance = await SaveOrUpdateEmployeeAttandance(employerId);
        if (perrorAttandance < 1)
        {
            throw new Exception("error on saveing employee attandance information data");
        }
        //Save Employee Balance (رصيد)
        int perrorBalance = saveOrUpdateEmployeeAllData.SaveOrUpdateEmployeeContract.CurrentBalance.HasValue ? await SaveOrUpdateEmployeeBalance(employerId, saveOrUpdateEmployeeAllData.SaveOrUpdateEmployeeContract.CurrentBalance) : 1;
        if (perrorBalance < 1)
        {
            throw new Exception("error on saveing employee Balance information data");
        }
        //save emplyee Shift(الشفتات)
        int perrorShifts = await SaveOrUpdateEmployeeShifts(saveOrUpdateEmployeeAllData.SaveOrUpdateEmployeeShifts, employerId);
        if (perrorShifts < 1)
        {
            throw new Exception("error on saveing employee Shifts information data");
        }

      
        return employerId;
    }
    private async Task<int> SaveOrUpdateEmployeeMainInFormation(SaveOrUpdateEmployeeInFormation saveOrUpdateEmployeeInFormation)
    {
        if (saveOrUpdateEmployeeInFormation != null)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeName", saveOrUpdateEmployeeInFormation.EmployeeName ?? Convert.DBNull },
            { "pEmployeeNumber", saveOrUpdateEmployeeInFormation.EmployeeNumber ?? Convert.DBNull },
            { "pStatusID", saveOrUpdateEmployeeInFormation.StatusID ?? Convert.DBNull },
            { "pSettingID", saveOrUpdateEmployeeInFormation.SettingID ?? Convert.DBNull },
            { "pCreatedBy", _projectProvider.UserId()<1 ? Convert.DBNull: _projectProvider.UserId()},
            { "pNationalId", saveOrUpdateEmployeeInFormation.NationalId ?? Convert.DBNull },
            { "pSocialNumber", saveOrUpdateEmployeeInFormation.SocialNumber ?? Convert.DBNull },
            { "pCareerNumber", saveOrUpdateEmployeeInFormation.CareerNumber ?? Convert.DBNull },
            { "pJobTitleName", saveOrUpdateEmployeeInFormation.JobTitleName ?? Convert.DBNull },
            { "pBirthDate", saveOrUpdateEmployeeInFormation.BirthDate.DateToIntValue() ?? Convert.DBNull }, // Assuming DateToIntValue() exists for date conversions
            { "pGenderID", saveOrUpdateEmployeeInFormation.GenderID ?? Convert.DBNull },
            { "pNationalityID", saveOrUpdateEmployeeInFormation.NationalityID ?? Convert.DBNull },
            { "pMobileNo", saveOrUpdateEmployeeInFormation.MobileNo ?? Convert.DBNull },
            { "pEmailNo", saveOrUpdateEmployeeInFormation.EmailNo ?? Convert.DBNull },
            { "pMaritalStatusID", saveOrUpdateEmployeeInFormation.MaritalStatusID ?? Convert.DBNull },
            { "pEmergencyCallName", saveOrUpdateEmployeeInFormation.EmergencyCallName ?? Convert.DBNull },
            { "pEmergencyCallMobile", saveOrUpdateEmployeeInFormation.EmergencyCallMobile ?? Convert.DBNull },
            { "pAccountNo", saveOrUpdateEmployeeInFormation.AccountNo ?? Convert.DBNull },
            { "pIBAN", saveOrUpdateEmployeeInFormation.IBAN ?? Convert.DBNull },
            { "pBankID", saveOrUpdateEmployeeInFormation.BankID ?? Convert.DBNull },
            { "pBranchID", saveOrUpdateEmployeeInFormation.BranchID ?? Convert.DBNull },
            { "pEmployeeImage", saveOrUpdateEmployeeInFormation.EmployeeImage ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pHasBreak", saveOrUpdateEmployeeInFormation.HasBreak == true ? 1 : 0 },
            { "pStartDate", saveOrUpdateEmployeeInFormation.StartDate.DateToIntValue() ?? Convert.DBNull }, // Assuming DateToIntValue() exists
            { "pDepartmentID", saveOrUpdateEmployeeInFormation.DepartmentID ?? Convert.DBNull },
            { "pUserName", saveOrUpdateEmployeeInFormation.UserName ?? Convert.DBNull },
            { "pPassword", saveOrUpdateEmployeeInFormation.Password ?? Convert.DBNull },
            { "pSSNType", saveOrUpdateEmployeeInFormation.SSNType ?? Convert.DBNull },
            { "pIsDynamicShift", saveOrUpdateEmployeeInFormation.IsDynamicShift ?? Convert.DBNull },
            { "pEmployeeNameEn", saveOrUpdateEmployeeInFormation.EmployeeNameEn ?? Convert.DBNull },
            { "pIsMilitary", saveOrUpdateEmployeeInFormation.IsMilitary ?? Convert.DBNull },
            { "pJobTitleID", saveOrUpdateEmployeeInFormation.JobTitleID ?? Convert.DBNull },
            { "pSectionID", saveOrUpdateEmployeeInFormation.SectionID ?? Convert.DBNull },
            { "pEmergencyCallMobile2", saveOrUpdateEmployeeInFormation.EmergencyCallMobile2 ?? Convert.DBNull },
            { "pEmergencyCallName2", saveOrUpdateEmployeeInFormation.EmergencyCallName2 ?? Convert.DBNull },
            { "pDateForMozawleh", saveOrUpdateEmployeeInFormation.DateForMozawleh.DateToIntValue() ?? Convert.DBNull },
            { "pcompanynameid", saveOrUpdateEmployeeInFormation.CompanyNameID ?? Convert.DBNull },
            {"pHasMobileInfo",saveOrUpdateEmployeeInFormation.HasMobileInfo ?? Convert.DBNull }
        };

            Dictionary<string, object> outputParams = new Dictionary<string, object>
                {
                    { "pEmployeeID", saveOrUpdateEmployeeInFormation.EmployeeID.HasValue &&saveOrUpdateEmployeeInFormation.EmployeeID.Value>0? saveOrUpdateEmployeeInFormation.EmployeeID.Value:"int" }, // OUTPUT
                    { "pError", "int" } // OUTPUT
                };

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertEmployees", inputParams, outputParams);

            int pErrorValue = (int)outputValues["pError"];
            if (pErrorValue > 0)
            {
                if (saveOrUpdateEmployeeInFormation.EmployeeID.HasValue && saveOrUpdateEmployeeInFormation.EmployeeID.Value > 0)
                {
                    return saveOrUpdateEmployeeInFormation.EmployeeID.Value;
                }
                return (int)outputValues["pEmployeeID"];
            }
            return pErrorValue;
        }
        return -1;
    }
    private async Task<int> SaveOrUpdateEmployeeShifts(SaveOrUpdateEmployeeShifts saveOrUpdateEmployeeShifts, int EmployeeId)
    {
        int perror = 1;

        if (saveOrUpdateEmployeeShifts != null && !string.IsNullOrEmpty(saveOrUpdateEmployeeShifts.ShiftId))
        {
            int ShiftidCount = saveOrUpdateEmployeeShifts.ShiftId.Split(';').Count();
            string pEmployeeID = string.Join(";", Enumerable.Repeat(EmployeeId, ShiftidCount - 1));

            Dictionary<string, object> inputParams = new Dictionary<string, object>
                {
                    { "pEmployeeID",pEmployeeID},
                    { "pShiftID",saveOrUpdateEmployeeShifts.ShiftId },
                    { "pCreatedBy", _projectProvider.UserId() },

                };

            Dictionary<string, object> outputParams = new Dictionary<string, object>
                {
                    { "pError", "int" } // OUTPUT
                };

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeShiftCheck", inputParams, outputParams);

            perror = (int)outputValues["pError"];


            return perror;
        }
        return 2;//there is no data on object of List Shifts
    }
    private async Task<int> SaveOrUpdateContractInformation(SaveOrUpdateEmployeeContract contractModel, int EmployeeId)
    {
        if (contractModel != null)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            //{ "pContractID", contractModel.ContractID ?? Convert.DBNull },
            { "pStartDate", contractModel.StartDate.DateToIntValue() ?? Convert.DBNull },
            { "pEndDate", contractModel.EndDate.DateToIntValue() ?? Convert.DBNull },
            { "pEmployeeID", EmployeeId},
            { "pSalary", contractModel.Salary ?? Convert.DBNull },
            { "pCreatedBy", _projectProvider.UserId()},
            { "pSocialSecuritySalary", contractModel.SocialSecuritySalary ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pIsDailyWork", contractModel.IsDailyWork }, // Convert bool to int for the stored procedure
            { "pContractTypeID", contractModel.ContractTypeID ?? Convert.DBNull },
            { "pincometaxtype", contractModel.IncomeTaxType ?? Convert.DBNull },
            { "pCompanyID", contractModel.CompanyID ?? Convert.DBNull },
            { "pEmployeeWorkingHours", contractModel.EmployeeWorkingHours ?? Convert.DBNull }
        };

            Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pContractID", contractModel.ContractID.HasValue && contractModel.ContractID.Value > 0 ? contractModel.ContractID.Value : "int" }, // OUTPUT
            { "pError", "int" } // OUTPUT
        };

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertEmployeeContracts", inputParams, outputParams);

            int pErrorValue = (int)outputValues["pError"];
            if (pErrorValue > 1)
            {
                if (contractModel.ContractID.HasValue && contractModel.ContractID.Value > 0)
                {
                    return contractModel.ContractID.Value;
                }
                return (int)outputValues["pContractID"];
            }
            return pErrorValue;
        }
        return -1;
    }
    private async Task<int> SaveOrUpdateEmployeeAllowanceInformation(List<SaveOrUpdateEmployeeAllowance> employeeAllowanceModel, int EmployeeId)
    {
        int pErrorValue = 1;
        if (employeeAllowanceModel != null && employeeAllowanceModel.Count() > 0)
        {
            foreach (var item in employeeAllowanceModel)
            {
                Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            //{ "pEmployeeAllowanceID", item.EmployeeAllowanceID ?? Convert.DBNull },
            { "pEmployeeID", EmployeeId },
            { "pStartDate", item.StartDate.DateToIntValue() ?? Convert.DBNull },
            { "pEndDate", item.EndDate.DateToIntValue() ?? Convert.DBNull },
            { "pAllowanceID", item.AllowanceID ?? Convert.DBNull },
            { "pCreatedBy", _projectProvider.UserId() },
            { "pProjectID", _projectProvider.GetProjectId() }, // Required, so no null check
            { "pAmount", item.Amount ?? Convert.DBNull },
            { "pCalculatedWithOverTime", item.CalculatedWithOverTime ?? Convert.DBNull }
        };

                Dictionary<string, object> outputParams = new Dictionary<string, object>
                {
                    { "pEmployeeAllowanceID", item.EmployeeAllowanceID.HasValue && item.EmployeeAllowanceID.Value > 0 ? item.EmployeeAllowanceID.Value : "int" }, // OUTPUT
                    { "pError", "int" } // OUTPUT
                };

                var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeAllowances", inputParams, outputParams);

                pErrorValue = (int)outputValues["pError"];
            }
            return pErrorValue;


        }
        return 2;//The model is empty and not have any data 
    }
    private async Task<int> SaveOrUpdateEmployeeDeductionInformation(List<SaveOrUpdateEmployeeDeduction> employeeDeductionModel, int EmployeeId)
    {
        int pErrorValue = 1;
        if (employeeDeductionModel != null && employeeDeductionModel.Count() > 0)
        {
            foreach (var item in employeeDeductionModel)
            {
                Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                //{ "pEmployeeDeductionID", item.EmployeeDeductionID ?? Convert.DBNull },
                { "pEmployeeID", EmployeeId},
                { "pStartDate", item.StartDate.DateToIntValue() ?? Convert.DBNull },
                { "pEndDate", item.EndDate.DateToIntValue() ?? Convert.DBNull },
                { "pDeductionID", item.DeductionID ?? Convert.DBNull },
                { "pCreatedBy",_projectProvider.UserId()  },
                { "pProjectID", _projectProvider.GetProjectId() },
                { "pAmount", item.Amount ?? Convert.DBNull }
            };

                Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "pEmployeeDeductionID", item.EmployeeDeductionID.HasValue && item.EmployeeDeductionID.Value > 0 ? item.EmployeeDeductionID.Value : "int" }, // OUTPUT
                { "pError", "int" } // OUTPUT
            };

                var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeDeductions", inputParams, outputParams);

                pErrorValue = (int)outputValues["pError"];
            }


            return pErrorValue;
        }
        return 2;//empty ,there is no data 
    }

    private async Task<int> SaveOrUpdateEmployeeAttandance(int EmployeeId)
    {
        int perror = 1;

    

            Dictionary<string, object> inputParams = new Dictionary<string, object>
                {
                    { "pEmployeeID",EmployeeId},
                    { "pProjectID",_projectProvider.GetProjectId() },

                };

            Dictionary<string, object> outputParams = new Dictionary<string, object>
                {
                    { "pError", "int" } // OUTPUT
                };

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertEmployeeAttendance", inputParams, outputParams);

            perror = (int)outputValues["pError"];


            return perror;
       
    }
    private async Task<int> SaveOrUpdateEmployeeBalance(int EmployeeId,int? CurrentBalance)
    {
        int perror = 1;
        if(CurrentBalance.HasValue)
        {
            var settingResult = await _lookupsService.GetSettings();

            var activeYear = settingResult?.ActiveYear;

            Dictionary<string, object> inputParams = new Dictionary<string, object>
                {
                    { "pEmployeeID",EmployeeId},
                    { "pCurrentBalance",CurrentBalance},
                    { "pYearID",activeYear},
                    { "pProjectID",_projectProvider.GetProjectId() },

                };

            Dictionary<string, object> outputParams = new Dictionary<string, object>
                {
                    { "pError", "int" } // OUTPUT
                };

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertEmployeeBalance", inputParams, outputParams);

            perror = (int)outputValues["pError"];
        }
       


        return perror;

    }


    #endregion

    #region Employee Contracts

    public async Task<dynamic> GetEmployeeContracts(GetEmployeeContracts getEmployeeContracts)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pContractID", getEmployeeContracts.ContractID ?? Convert.DBNull },
            { "pContractTypeID", getEmployeeContracts.ContractTypeID ?? Convert.DBNull },
            { "pEmployeeID", getEmployeeContracts.EmployeeID ?? Convert.DBNull },
            { "pFromDate", getEmployeeContracts.FromDate==null ? Convert.DBNull:getEmployeeContracts.FromDate.DateToIntValue() },
            { "pToDate", getEmployeeContracts.ToDate==null ? Convert.DBNull:getEmployeeContracts.ToDate.DateToIntValue() },
            { "pProjectID", _projectProvider.GetProjectId() },  // Not nullable, no need for DB null check
            { "pLanguageID", _projectProvider.LangId() }, // Not nullable, no need for DB null check
            { "pStatusID", getEmployeeContracts.StatusID ?? Convert.DBNull },
            { "pToFromDate", getEmployeeContracts.ToFromDate==null ? Convert.DBNull:getEmployeeContracts.ToFromDate.DateToIntValue() },
            { "pToToDate",getEmployeeContracts.ToToDate==null ? Convert.DBNull:getEmployeeContracts.ToToDate.DateToIntValue() },
            { "pPageNo" ,getEmployeeContracts.PageNo },
            { "pPageSize",getEmployeeContracts.PageSize }
        };
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            {"prowcount","int" }
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeContractsResponse>("dbo.GetEmployeeContracts", inputParams, outputParams);
        return PublicHelper.CreateResultPaginationObject(getEmployeeContracts, result, outputValues);

    }

    public async Task<int> SaveEmployeeContracts(SaveEmployeeContracts saveEmployeeContracts)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pContractTypeID", saveEmployeeContracts.ContractTypeID??Convert.DBNull },
                { "pProjectID",_projectProvider.GetProjectId()},
                { "pEmployeeID", saveEmployeeContracts.EmployeeID??Convert.DBNull },
                { "pStatusID", saveEmployeeContracts.StatusID??Convert.DBNull },
                { "pCreatedBy", _projectProvider.UserId() },
                { "pContractEndDate", saveEmployeeContracts.ContractEndDate==null?Convert.DBNull:saveEmployeeContracts.ContractEndDate.DateToIntValue() },
                //{ "pContractConfirmDate", saveEmployeeContracts.ContractConfirmDate==null?Convert.DBNull:saveEmployeeContracts.ContractConfirmDate.DateToIntValue() },
                { "pContractFromDate", saveEmployeeContracts.ContractStartDate==null?Convert.DBNull:saveEmployeeContracts.ContractStartDate.DateToIntValue() },
                //{ "pContractStartDate", saveEmployeeContracts.ContractStartDate==null?Convert.DBNull:saveEmployeeContracts.ContractStartDate.DateToIntValue() },
                { "pCompanyID", saveEmployeeContracts.CompanyID??Convert.DBNull }
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeContracts", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    #endregion
    #region Employee Additional Info(معلومات اضافية للموظف)
    public async Task<dynamic> GetEmployeesAdditionalInfo(GetEmployeeAdditionalInfoInput getEmployeeAdditionalInfoInput)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pEmployeeID", getEmployeeAdditionalInfoInput.EmployeeID },            

        };
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "prowcount","int" },
        };

        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeesOutput>("dbo.GetEmployees", inputParams, outputParams);
        return result;
    }
    public async Task<int> SaveEmployeeAdditionalInfo(SaveOrUpdateEmployeeAdditionalInfo saveOrUpdateEmployeeAdditionalInfo)
    {
        if (saveOrUpdateEmployeeAdditionalInfo != null)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeID", saveOrUpdateEmployeeAdditionalInfo.EmployeeID ?? Convert.DBNull },
            { "pStatusID", saveOrUpdateEmployeeAdditionalInfo.StatusID ?? Convert.DBNull },
            { "pSettingID", saveOrUpdateEmployeeAdditionalInfo.SettingID ?? Convert.DBNull },
            { "pCreatedBy", _projectProvider.UserId() < 1 ? Convert.DBNull : _projectProvider.UserId() },
            { "pNationalId", saveOrUpdateEmployeeAdditionalInfo.NationalId ?? Convert.DBNull },
            { "pSocialNumber", saveOrUpdateEmployeeAdditionalInfo.SocialNumber ?? Convert.DBNull },
            { "pCareerNumber", saveOrUpdateEmployeeAdditionalInfo.CareerNumber ?? Convert.DBNull },
            { "pJobTitleName", saveOrUpdateEmployeeAdditionalInfo.JobTitleName ?? Convert.DBNull },
            { "pBirthDate", saveOrUpdateEmployeeAdditionalInfo.BirthDate.DateToIntValue() ?? Convert.DBNull }, // Assuming DateToIntValue() exists
            { "pGenderID", saveOrUpdateEmployeeAdditionalInfo.GenderID ?? Convert.DBNull },
            { "pNationalityID", saveOrUpdateEmployeeAdditionalInfo.NationalityID ?? Convert.DBNull },
            { "pMobileNo", saveOrUpdateEmployeeAdditionalInfo.MobileNo ?? Convert.DBNull },
            { "pEmailNo", saveOrUpdateEmployeeAdditionalInfo.EmailNo ?? Convert.DBNull },
            { "pMaritalStatusID", saveOrUpdateEmployeeAdditionalInfo.MaritalStatusID ?? Convert.DBNull },
            { "pEmergencyCallName", saveOrUpdateEmployeeAdditionalInfo.EmergencyCallName ?? Convert.DBNull },
            { "pEmergencyCallMobile", saveOrUpdateEmployeeAdditionalInfo.EmergencyCallMobile ?? Convert.DBNull },
            { "pAccountNo", saveOrUpdateEmployeeAdditionalInfo.AccountNo ?? Convert.DBNull },
            { "pIBAN", saveOrUpdateEmployeeAdditionalInfo.IBAN ?? Convert.DBNull },
            { "pBankID", saveOrUpdateEmployeeAdditionalInfo.BankID ?? Convert.DBNull },
            { "pBranchID", saveOrUpdateEmployeeAdditionalInfo.BranchID ?? Convert.DBNull },
            { "pEmployeeImage", saveOrUpdateEmployeeAdditionalInfo.EmployeeImage ?? Convert.DBNull },
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pHasBreak", saveOrUpdateEmployeeAdditionalInfo.HasBreak.HasValue && saveOrUpdateEmployeeAdditionalInfo.HasBreak.Value == 1 ? 1 : 0 },
            { "pStartDate", saveOrUpdateEmployeeAdditionalInfo.StartDate.DateToIntValue() ?? Convert.DBNull },
            { "pDepartmentID", saveOrUpdateEmployeeAdditionalInfo.DepartmentID ?? Convert.DBNull },
            { "pUserName", saveOrUpdateEmployeeAdditionalInfo.UserName ?? Convert.DBNull },
            { "pPassword", saveOrUpdateEmployeeAdditionalInfo.Password ?? Convert.DBNull },
            { "pSSNType", saveOrUpdateEmployeeAdditionalInfo.SSNType ?? Convert.DBNull },
            { "pIsDynamicShift", saveOrUpdateEmployeeAdditionalInfo.IsDynamicShift ?? Convert.DBNull },
            { "pReligionID", saveOrUpdateEmployeeAdditionalInfo.ReligionID ?? Convert.DBNull },
            { "pFamilyCount", saveOrUpdateEmployeeAdditionalInfo.FamilyCount ?? Convert.DBNull },
            { "pPermitNo", saveOrUpdateEmployeeAdditionalInfo.PermitNo ?? Convert.DBNull },
            { "pPermitEndDate", saveOrUpdateEmployeeAdditionalInfo.PermitEndDate.DateToIntValue() ?? Convert.DBNull },
            { "pCompanyID", saveOrUpdateEmployeeAdditionalInfo.CompanyID ?? Convert.DBNull },
            { "pSectionID", saveOrUpdateEmployeeAdditionalInfo.SectionID ?? Convert.DBNull },
            { "pParticipateDate", saveOrUpdateEmployeeAdditionalInfo.ParticipateDate.DateToIntValue() ?? Convert.DBNull },
            { "pSSNParticipateDate", saveOrUpdateEmployeeAdditionalInfo.SSNParticipateDate.DateToIntValue() ?? Convert.DBNull },
            { "pEmergencyCallName2", saveOrUpdateEmployeeAdditionalInfo.EmergencyCallName2 ?? Convert.DBNull },
            { "pEmergencyCallMobile2", saveOrUpdateEmployeeAdditionalInfo.EmergencyCallMobile2 ?? Convert.DBNull },
            { "pFridayShift", saveOrUpdateEmployeeAdditionalInfo.FridayShift ?? Convert.DBNull },
            { "pCShift", saveOrUpdateEmployeeAdditionalInfo.CShift ?? Convert.DBNull },
            { "pNoOvertime", saveOrUpdateEmployeeAdditionalInfo.NoOvertime ?? Convert.DBNull },
            { "pEndHealthCertificate", saveOrUpdateEmployeeAdditionalInfo.EndHealthCertificate ?? Convert.DBNull },
            { "pStartHealthCertificate", saveOrUpdateEmployeeAdditionalInfo.StartHealthCertificate ?? Convert.DBNull },
            { "pSwiftcode", saveOrUpdateEmployeeAdditionalInfo.Swiftcode ?? Convert.DBNull },
            { "pEmployeeWorkingHours", saveOrUpdateEmployeeAdditionalInfo.EmployeeWorkingHours ?? Convert.DBNull }
        };

            Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pEmployeeID", saveOrUpdateEmployeeAdditionalInfo.EmployeeID.HasValue && saveOrUpdateEmployeeAdditionalInfo.EmployeeID.Value > 0 ? saveOrUpdateEmployeeAdditionalInfo.EmployeeID.Value : "int" }, // OUTPUT
            { "pError", "int" } // OUTPUT
        };

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.SaveEmployeeAdditionalInfo", inputParams, outputParams);

            int pErrorValue = (int)outputValues["pError"];
            if (pErrorValue > 0)
            {
                if (saveOrUpdateEmployeeAdditionalInfo.EmployeeID.HasValue && saveOrUpdateEmployeeAdditionalInfo.EmployeeID.Value > 0)
                {
                    return saveOrUpdateEmployeeAdditionalInfo.EmployeeID.Value;
                }
                return (int)outputValues["pEmployeeID"];
            }
            return pErrorValue;
        }
        return -1;
    }

    #endregion

    #region Employee Additional Info(معلومات اضافية للموظف)
    public async Task<dynamic> GetEmployeeIncrease(GetEmployeeIncreaseInput getEmployeeIncreaseInput)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {

            { "pDetailID", getEmployeeIncreaseInput.DetailID },
            { "pEmployeeID", getEmployeeIncreaseInput.EmployeeID },
            { "pFlag", 1 },
            { "pProjectID", _projectProvider.GetProjectId() },
            { "pFromDate", getEmployeeIncreaseInput.FromDate==null?Convert.DBNull:getEmployeeIncreaseInput.FromDate.DateToIntValue() },
            { "pToDate", getEmployeeIncreaseInput.ToDate==null?Convert.DBNull:getEmployeeIncreaseInput.ToDate.DateToIntValue() },
            { "pDepartmentID", getEmployeeIncreaseInput.DepartmentID },
            { "pLoginUserID", _projectProvider.UserId() },
            { "pPageNo", getEmployeeIncreaseInput.PageNo },
            { "pPageSize", getEmployeeIncreaseInput.PageSize },


        };
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "prowcount","int" },
        };

        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeIncreaseOutput>("dbo.GetEmployeeIncrease", inputParams, outputParams);
        return result;
    }

    public async Task<int> DeleteEmployeeIncrease(DeleteEmployeeIncrease deleteEmployeeIncrease)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pDetailID", deleteEmployeeIncrease.DetailID },  
                { "pEmployeeID", deleteEmployeeIncrease.EmployeeID},
                { "pCreatedBy", _projectProvider.UserId()},
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeeIncrease", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    public async Task<int> SaveEmployeeIncrease(SaveEmployeeIncrease saveEmployeeIncrease)
    {
        Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID ", saveEmployeeIncrease.EmployeeID??Convert.DBNull},
                { "pIncreaseDate", saveEmployeeIncrease.IncreaseDate.DateToIntValue()??Convert.DBNull },
                { "pSSSIncreaseDate", saveEmployeeIncrease.SSSIncreaseDate.DateToIntValue()??Convert.DBNull },
                { "pAmount", saveEmployeeIncrease.Amount??Convert.DBNull },
               { "pSSNAmount", saveEmployeeIncrease.SSNAmount??Convert.DBNull },
               { "pCreatedBy", _projectProvider.UserId() },
                { "pProjectID", _projectProvider.GetProjectId() },
                
            };

        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" },
        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertEmployeeIncrease", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return pErrorValue;
    }

    #endregion
}
