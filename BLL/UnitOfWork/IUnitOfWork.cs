﻿using BusinessLogicLayer.Repositories;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Employee> EmployeeRepository { get; }
        IRepository<LookupTable> LookupsRepository { get; }

        IRepository<EmployeeLeaf> EmployeeLeaveRepository { get; }
        IRepository<EmployeeVacation> EmployeeVacationRepository { get; }
        IRepository<EmployeeLoan> EmployeeLoanRepository { get; }
        IRepository<GetEmployeeBalanceReportResult> EmployeeBalanceRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<Project> ProjectRepository { get; }
        IRepository<MobileVersion> MobileVersionRepository { get; }
        IRepository<EvaluationCategory> EvaluationCategoryRepository { get; }
        IRepository<EvaluationQuestion> EvaluationQuestionRepository { get; }

        IRepository<EvaluationSurvey> EvaluationSurveyRepository { get; }
        IRepository<EvaluationSurveyQuestions> EvaluationSurveyQuestionsRepository { get; }
        IRepository<EvaluationSurveySetup> EvaluationSurveySetupRepository { get; }

        void Save();
        Task SaveAsync();
    }
}
