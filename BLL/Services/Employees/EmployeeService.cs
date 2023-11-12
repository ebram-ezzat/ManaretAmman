using AutoMapper;
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
        var result = _mapper.Map<List<EmployeeLookup>>(employees);

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
        var employeePapersResponse = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeePaperResponse>("[dbo].[GetEmployeePaper]", parameters, null);
        obj.CountRows = 10000;
        obj.Data = employeePapersResponse;
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

    public Task<int> SaveEmployeePaperProc(SaveEmployeePaper saveEmployeePaper)
    {
        throw new System.NotImplementedException();
    }
}
