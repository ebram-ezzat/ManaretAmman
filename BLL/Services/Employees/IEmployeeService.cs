using DataAccessLayer.DTO.Employees;

namespace BusinessLogicLayer.Services.Employees;

public interface IEmployeeService
{
    Task<List<EmployeeLookup>> GetList();
    Task<List<EmployeeLookup>> GetEmployeesProc();
    Task SaveAttendanceByUser(SaveAttendance saveAttendance);
    Task<object> GetEmployeePaperProc(GetEmployeePaperRequest getEmployeePaperRequest);
    Task<int> DeleteEmployeePaperProc(int EmployeeId, int DetailId);
    Task<int> SaveEmployeePaperProc(SaveEmployeePaper saveEmployeePaper);

}
