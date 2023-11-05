using AutoMapper;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.Models;
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
}
