using DataAccessLayer.DTO.Employees;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Employees;

public interface IEmployeeService
{
    Task<List<EmployeeLookup>> GetList();
    Task<List<EmployeeLookup>> GetEmployeesProc();
    Task SaveAttendanceByUser(SaveAttendance saveAttendance);
    Task<object> GetEmployeePaperProc(GetEmployeePaperRequest getEmployeePaperRequest);
    Task<int> DeleteEmployeePaperProc(int EmployeeId, int DetailId);
    Task<int> SaveEmployeePaperProc(SaveEmployeePaper saveEmployeePaper);
    Task<List<EmplyeeProfileVModel>> EmployeeProfile(int EmployeeId);

    Task<int> SaveEmployeeAffairsService (SaveEmployeeAffairsServices saveEmployeeAffairsService);
    Task<int> DeleteEmployeeAffairsService(DeleteEmployeeAffairsService deleteEmployeeAffairsService);

    Task<dynamic> GetEmployeeAffairsService(GetEmployeeAffairsServiceRequest getEmployeeAffairsServiceRequest);

    Task<int> SaveOrUpdateEmployeeEvaluation(SaveOrUpdateEmployeeEvaluation saveOrUpdateEmployeeEvaluation);
    Task<List<GetEmployeeEvaluation>> GetEmployeeEvaluation(GetEmployeeEvaluation getEmployeeEvaluation);
    Task<int> SaveOrUpdateEmployeeEvaluationQuestion(SaveOrUpdateEvaluationQuestion saveOrUpdateEvaluationQuestion);
    Task<List<GetEvaluationQuestion>> GetEmployeeEvaluationQuestion(GetEvaluationQuestion getEvaluationQuestion);

    Task<int> SaveOrUpdateEvaluationSurvey(SaveOrUpdateEvaluationSurvey saveOrUpdateEvaluationSurvey);
    Task<List<GetEvaluationSurvey>> GetEvaluationSurvey(GetEvaluationSurvey getEvaluationSurvey);
    Task<int> DeleteEvaluationSurvey(DeleteEvalualtionSurvey deleteEvalualtionServey);

}
