using DataAccessLayer.DTO.EmployeeAttendance;
using DataAccessLayer.DTO.EmployeeContract;
using DataAccessLayer.DTO.EmployeeDeductions;
using DataAccessLayer.DTO.EmployeeIncrease;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.EmployeeSalary;
using DataAccessLayer.DTO.EmployeeShifts;
using DataAccessLayer.DTO.EmployeeTransaction;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Employees;

public interface IEmployeeService
{
    Task<List<EmployeeLookup>> GetList();
    Task<List<EmployeeLookup>> GetEmployeesProc();
    Task<int> SaveAttendanceByUser(SaveAttendance saveAttendance);
    Task<object> GetEmployeePaperProc(GetEmployeePaperRequest getEmployeePaperRequest);
    Task<int> DeleteEmployeePaperProc(int EmployeeId, int DetailId);
    Task<int> SaveEmployeePaperProc(SaveEmployeePaper saveEmployeePaper);
    Task<List<EmplyeeProfileVModel>> EmployeeProfile(int EmployeeId);

    Task<int> SaveEmployeeAffairsService(SaveEmployeeAffairsServices saveEmployeeAffairsService);
    Task<int> DeleteEmployeeAffairsService(DeleteEmployeeAffairsService deleteEmployeeAffairsService);

    Task<dynamic> GetEmployeeAffairsService(GetEmployeeAffairsServiceRequest getEmployeeAffairsServiceRequest);

    Task<int> SaveOrUpdateEmployeeEvaluation(SaveOrUpdateEmployeeEvaluation saveOrUpdateEmployeeEvaluation);
    Task<List<GetEmployeeEvaluation>> GetEmployeeEvaluation(GetEmployeeEvaluation getEmployeeEvaluation);
    Task<int> SaveOrUpdateEmployeeEvaluationQuestion(SaveOrUpdateEvaluationQuestion saveOrUpdateEvaluationQuestion);
    Task<List<GetEvaluationQuestion>> GetEmployeeEvaluationQuestion(GetEvaluationQuestion getEvaluationQuestion);

    Task<int> SaveOrUpdateEvaluationSurvey(SaveOrUpdateEvaluationSurvey saveOrUpdateEvaluationSurvey);
    Task<List<GetEvaluationSurvey>> GetEvaluationSurvey(GetEvaluationSurvey getEvaluationSurvey);
    Task<int> DeleteEvaluationSurvey(DeleteEvalualtionSurvey deleteEvalualtionServey);
    Task<int> SaveEvaluationSurveyQuestions(List<SaveEvaluationSurveyQuestions> LstQuestions);
    Task<List<GetEvaluationSurveyQuestions>> GetEvaluationSurveyQuestions(GetEvaluationSurveyQuestions getEvaluationSurveyQuestions);
    Task<dynamic> GetEmployeePenalty(GetEmployeePenalty getEmployeePenalty);
    Task<int> SaveEmployeePenalty(SaveEmployeePenalty saveEmployeePenalty);
    Task<int> UpdateEmployeePenalty(SaveEmployeePenalty saveEmployeePenalty);

    Task<List<GetEvaluationSurveySetup>> GetEvaluationSurveySetup(GetEvaluationSurveySetup getEvaluationSurveySetup);
    Task<int> DeleteEvaluationSurveySetup(DeleteEvaluationSurveySetup deleteEvaluationSurveySetup);
    Task<int> SaveOrUpdateEvaluationSurveySetup(SaveEvaluationSurveySetup saveOrUpdateEvaluationSurveySetup);
    Task<int> ChangeStatusEmployeePenalty(SaveEmployeePenalty updateEmployeePenalty);
    Task<dynamic> GetEmployeeShifts(GetEmployeeShifts getEmployeeShifts);
    Task<int> SaveEmployeeShifts(GetEmployeeShifts saveEmployeeShifts);
    Task<List<GetEmployeeAttandanceShiftOutput>> GetEmployeeAttandanceShift(GetEmployeeAttandanceShiftInput getEmployeeAttandanceShiftInput);
    Task<int> DeleteEmployeeAttandanceShifts(DeleteEmployeeAttandanceShifts deleteEmployeeAttandanceShifts);
    Task<int> SaveEmployeeAttandanceShifts(SaveEmployeeAttandanceShiftInput saveEmployeeAttandanceShiftInput);
    Task<dynamic> GetEmployeeTransaction(GetEmployeeTransactionInput getEmployeeTransactionInput);
    Task<int> DeleteEmployeeTransaction(DeleteEmployeeTransaction deleteEmployeeTransaction);
    Task<int> SaveEmployeeTransaction(SaveEmployeeTransaction saveEmployeeTransaction);
    Task<int> SaveEmployeeTransactionAuto(SaveEmployeeTransactionAutoInput saveEmployeeTransactionAutoInput);

    Task<dynamic> GetEmployeeAllowancesMainScreen(GetEmployeeAllowancesInput getEmployeeAllowancesInput);
    Task<dynamic> GetEmployeeAllowancesPopupScreen(GetEmployeeAllowancesInput getEmployeeAllowancesInput);
    Task<int> DeleteEmployeeAllowances(DeleteEmployeeAllowances deleteEmployeeAllowances);
    Task<int> UpdateEmployeeAllowances(UpdateEmployeeAllowances updateEmployeeAllowances);
    Task<int> SaveEmployeeAllowances(SaveEmployeeAllowances saveEmployeeAllowances);
    Task<dynamic> GetEmployeeAllowancesDeductionDDL(GetAllowanceDeductionInput getAllowanceDeductionInput);

    Task<List<GetEmployeeSalaryOutput>> GetEmployeeSalary(GetEmployeeSalaryInput getEmployeeSalaryInput);
    Task<GetEmployeeSalaryDetailsOutput> GetEmployeeSalaryDetails(GetEmployeeSalaryInput getEmployeeSalaryInput);
    Task<int> DeleteCancelSalary(DeleteCancelSalary deleteCancelSalary);
    Task<int> CalculateEmployeeSalary(CalculateEmployeeSalary calculateEmployeeSalary);
    Task<dynamic> GetEmployees(GetEmployeesInput getEmployeesInput);
    Task<int> SaveOrUpdateEmployee(SaveOrUpdateEmployeeAllData saveOrUpdateEmployeeAllData);
    Task<int> DeleteEmployeeWithRelatedData(DeleteEmployeeWithRelatedData deleteEmployeeWithRelatedDate);
    Task<dynamic> GetEmployeeDeductionsMainScreen(GetEmployeeDeductionsInput getEmployeeDeductionsInput);
    Task<dynamic> GetEmployeeDeductionsPopupScreen(GetEmployeeDeductionsInput getEmployeeDeductionsInput);
    Task<int> DeleteEmployeeDeductions(DeleteEmployeeDeductions deleteEmployeeDeductions);
    Task<int> UpdateEmployeeDeductions(UpdateEmployeeDeductions updateEmployeeDeductions);
    Task<int> SaveEmployeeDeductions(SaveEmployeeDeductions saveEmployeeDeductions);
    Task<dynamic> GetEmployeeContracts(GetEmployeeContracts getEmployeeContracts);
    Task<int> SaveEmployeeContracts(SaveEmployeeContracts saveEmployeeContracts);
    Task<dynamic> GetEmployeesAdditionalInfo(GetEmployeeAdditionalInfoInput getEmployeeAdditionalInfoInput);
    Task<int> SaveEmployeeAdditionalInfo(SaveOrUpdateEmployeeAdditionalInfo saveOrUpdateEmployeeAdditionalInfo);
    Task<dynamic> GetEmployeeIncrease(GetEmployeeIncreaseInput getEmployeeIncreaseInput);
    Task<int> DeleteEmployeeIncrease(DeleteEmployeeIncrease deleteEmployeeIncrease);
    Task<int> SaveEmployeeIncrease(SaveEmployeeIncrease saveEmployeeIncrease);
    Task<dynamic> GetEmployeeShiftsExchange(GetEmployeeShiftsExchangeInput getEmployeeShiftsExchange);
    Task<int> SaveEmployeeShiftsExchange(SaveEmployeeShiftsExchange saveEmployeeShiftsExchange);
    Task<List<GetEmployeeRatingOutput>> GetEmployeeRating(GetEmployeeRatingInput getEmployeeRatingInput);
    Task<int> AcceptEmployeeRating(UpdateEmployeeRatingInput updateEmployeeRatingInput);
    Task<dynamic> GetEmployeeRatingDetails(GetEmployeeRatingDetailsInput getEmployeeRatingDetailsInput);
    Task<int> SaveEmployeeRatingDetails(SaveEmployeeRatingDetailsInput saveEmployeeRatingDetailsInput);

}
