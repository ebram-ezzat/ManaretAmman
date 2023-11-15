using AutoMapper;
using AutoMapper.Execution;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.Models;
using System.Data;
using System.Dynamic;
using UnauthorizedAccessException = BusinessLogicLayer.Exceptions.UnauthorizedAccessException;

namespace BusinessLogicLayer.Services.Employees;

internal class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    IProjectProvider _projectProvider;
    IAuthService _authService;
    private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
    public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper, IProjectProvider projectProvider, IAuthService authService, PayrolLogOnlyContext payrolLogOnlyContext)
    {
        _unitOfWork = unitOfWork;
        _mapper     = mapper;
        _projectProvider = projectProvider;
        _authService = authService;
        _payrolLogOnlyContext = payrolLogOnlyContext;
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
        if(!_authService.IsValidUser(userId)) throw new UnauthorizedAccessException("Incorrect userId");
        int? employeeId = _authService.IsHr(userId);


        IQueryable<Employee> employees ;

        employees =
            from e in _unitOfWork.EmployeeRepository.PQuery()
            join lt in _unitOfWork.LookupsRepository.PQuery() on e.DepartmentID equals lt.ID into ltGroup
            from lt in ltGroup.DefaultIfEmpty()
            where e.ProjectID == projecId && (e.EmployeeID == employeeId || lt.EmployeeID == employeeId || employeeId == null) && lt.TableName == "Department" && lt.ColumnName == "DepartmentID"
            select  e;

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
        getEmployeePaperRequest.ProjectID= _projectProvider.GetProjectId();
        var parameters = PublicHelper.GetPropertiesWithPrefix<GetEmployeePaperRequest>(getEmployeePaperRequest, "p");
        var outPutPara = new Dictionary<string, object> { { "prowcount", "int"} };
        var (employeePapersResponse,outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeePaperResponse>("[dbo].[GetEmployeePaper]", parameters, outPutPara);
        int totalRecords= (int)outputValues["prowcount"];
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
            
        };

        // Define output parameters (optional)
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pError","int" }, // Assuming the output parameter "pError" is of type int
            // Add other output parameters as needed
        };

        // Call the ExecuteStoredProcedureAsync function
        var (result, outputValues)  = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeePaper", inputParams, outputParams);
        int pErrorValue = (int)outputValues["pError"];
        return result;

    }

    public async Task<int> SaveEmployeePaperProc(SaveEmployeePaper saveEmployeePaper)
    {
        int projecId = _projectProvider.GetProjectId();
        Dictionary<string, object> inputParams = new Dictionary<string, object>
        {
            { "pEmployeeID", saveEmployeePaper.EmployeeID },
            { "pPaperID", saveEmployeePaper.PaperID },           
            { "pNotes", saveEmployeePaper.Notes },
            { "pCratedBy", saveEmployeePaper.CreatedBy },
        };

        // Define output parameters (optional)
        Dictionary<string, object> outputParams = new Dictionary<string, object>
        {
            { "pDetailID","int" },
            { "pError","int" }, // Assuming the output parameter "pError" is of type int
            // Add other output parameters as needed

        };
        var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertEmployeePaper", inputParams, outputParams);

        if (saveEmployeePaper.File is not null)
        {
            var fileExtension = Path.GetExtension(saveEmployeePaper.File.FileName);

            int detailId = (int)outputValues["pDetailID"];
            outputParams["pDetailID"] = detailId;

            var (settingResult, outputSetting) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetSettingsResult>("dbo.GetSettings", new Dictionary<string, object> { { "pProjectID", projecId } }, null);
            var projectPath = settingResult.FirstOrDefault().AttachementPath;

            var filePath = projectPath + "/" + 01 + saveEmployeePaper.EmployeeID.ToString().PadLeft(6, '0') + detailId.ToString().PadLeft(6, '0') + "." + fileExtension;

            //var filePath = Path.Combine(settingResult.FirstOrDefault().AttachementPath, saveEmployeePaper.File.FileName);

            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await saveEmployeePaper.File.CopyToAsync(stream);
            }
            inputParams.Add("pPaperPath", filePath);
            var (resultUpdate, outputUpdate) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertEmployeePaper", inputParams, outputParams);

        }



        int pErrorValue = (int)outputValues["pError"];
       
        return result;
    }
}
