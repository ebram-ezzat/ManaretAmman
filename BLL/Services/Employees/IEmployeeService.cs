﻿using DataAccessLayer.DTO.Employees;
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
    Task<int> SaveEvaluationSurveyQuestions(List<SaveEvaluationSurveyQuestions> LstQuestions);
    Task<List<GetEvaluationSurveyQuestions>> GetEvaluationSurveyQuestions(GetEvaluationSurveyQuestions getEvaluationSurveyQuestions);
    Task<dynamic> GetEmployeePenalty(GetEmployeePenalty getEmployeePenalty);
    Task<int> SaveEmployeePenalty(SaveEmployeePenalty saveEmployeePenalty);
    Task<int> UpdateEmployeePenalty(SaveEmployeePenalty saveEmployeePenalty);

    Task<List<GetEvaluationSurveySetup>> GetEvaluationSurveySetup(GetEvaluationSurveySetup getEvaluationSurveySetup);
    Task<int> DeleteEvaluationSurveySetup(DeleteEvaluationSurveySetup deleteEvaluationSurveySetup);
    Task<int> SaveOrUpdateEvaluationSurveySetup(SaveEvaluationSurveySetup saveOrUpdateEvaluationSurveySetup);
    Task<int> ChangeStatusEmployeePenalty(SaveEmployeePenalty updateEmployeePenalty);
}
