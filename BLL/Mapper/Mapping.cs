﻿using AutoMapper;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeLeaves;
using DataAccessLayer.DTO.EmployeeLoans;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.EmployeeVacations;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;
using System.Text.Json;

namespace BusinessLogicLayer.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            #region Employee

            CreateMap<Employee, EmployeeLookup>();
            CreateMap<EmployeeProfile, EmplyeeProfileVModel>();

            CreateMap<SaveOrUpdateEmployeeEvaluation, EvaluationCategory>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
            .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectID, opt => opt.Ignore())
            .ForMember(dest => dest.Projects, opt => opt.Ignore());

            CreateMap<GetEmployeeEvaluation, EvaluationCategory>()
           .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
           .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
           .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
           .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
           .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
           .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
           .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
           .ForMember(dest => dest.ProjectID, opt => opt.Ignore())
           .ForMember(dest => dest.Projects, opt => opt.Ignore());

            CreateMap<EvaluationCategory, GetEmployeeEvaluation>()
          .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
          .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
          .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId));


            CreateMap<SaveOrUpdateEvaluationQuestion, EvaluationQuestion>()
           .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
           .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
           .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
           .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
           .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
           .ForMember(dest => dest.ProjectID, opt => opt.Ignore())
           .ForMember(dest => dest.Project, opt => opt.Ignore())
           .ForMember(dest => dest.EvaluationCategory, opt => opt.Ignore());



            CreateMap<GetEvaluationQuestion, EvaluationQuestion>()
           .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
           .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
           .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
           .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
           .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
           .ForMember(dest => dest.ProjectID, opt => opt.Ignore())
           .ForMember(dest => dest.Project, opt => opt.Ignore())
           .ForMember(dest => dest.EvaluationCategory, opt => opt.Ignore());


            CreateMap<EvaluationQuestion, GetEvaluationQuestion>()
          .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
          .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.EvaluationCategory.CategoryName))
         .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));


            CreateMap<SaveOrUpdateEvaluationSurvey, EvaluationSurvey>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
           .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
           .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
           .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
           .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
           .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
           .ForMember(dest => dest.ProjectID, opt => opt.Ignore())
           .ForMember(dest => dest.Projects, opt => opt.Ignore());

            CreateMap<EvaluationSurvey, GetEvaluationSurvey>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
       .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
          .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId));

            CreateMap<GetEvaluationSurvey, EvaluationSurvey>()
         .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
         .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
         .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
         .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
         .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
         .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
         .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
         .ForMember(dest => dest.ProjectID, opt => opt.Ignore());


            CreateMap<EvaluationSurveyQuestions, GetEvaluationSurveyQuestions>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
              .ForMember(dest => dest.EvaluationCategoryName, opt => opt.MapFrom(src => src.EvaluationCategory.CategoryName))
              .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
              .ForMember(dest => dest.EvaluationQuestion, opt => opt.MapFrom(src => src.EvaluationQuestion.Question))
              .ForMember(dest => dest.WithNotes, opt => opt.MapFrom(src => src.WithNotes))
              .ForMember(dest => dest.SurveyId, opt => opt.MapFrom(src => src.SurveyId))
              .ForMember(dest => dest.EvaluationSurveyName, opt => opt.MapFrom(src => src.EvaluationSurvey.Name))
              .ForMember(dest => dest.QuestionDegree, opt => opt.MapFrom(src => src.QuestionDegree));


            CreateMap<GetEvaluationSurveyQuestions, EvaluationSurveyQuestions>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                  .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                  .ForMember(dest => dest.WithNotes, opt => opt.MapFrom(src => src.WithNotes))
                  .ForMember(dest => dest.SurveyId, opt => opt.MapFrom(src => src.SurveyId))
                  .ForMember(dest => dest.QuestionDegree, opt => opt.MapFrom(src => src.QuestionDegree))
                   .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                     .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                     .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                     .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                     .ForMember(dest => dest.ProjectID, opt => opt.Ignore());

            CreateMap<SaveEvaluationSurveyQuestions, EvaluationSurveyQuestions>()
           .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
           .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
           .ForMember(dest => dest.WithNotes, opt => opt.MapFrom(src => src.WithNotes))
           .ForMember(dest => dest.SurveyId, opt => opt.MapFrom(src => src.SurveyId))

           .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
           .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
           .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
           .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.EvaluationQuestion, opt => opt.Ignore())
           .ForMember(dest => dest.EvaluationCategory, opt => opt.Ignore())
           .ForMember(dest => dest.EvaluationSurvey, opt => opt.Ignore())
           .ForMember(dest => dest.ProjectID, opt => opt.Ignore());


            CreateMap<EvaluationSurveySetup, GetEvaluationSurveySetup>()
                     .ForMember(dest => dest.DepartmentIds, opt => opt.MapFrom<DepartmentIdsResolver>())
            .ForMember(dest => dest.EmployeelevelIds, opt => opt.MapFrom<EmployeelevelIdsResolver>())
            .ForMember(dest => dest.UsertypeData, opt => opt.MapFrom<UserTypeDataResolver>())
             .ForMember(dest => dest.SurveyId, opt => opt.MapFrom(src => src.SurveyId))
            .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
            .ForMember(dest => dest.FromDate, opt => opt.MapFrom(src => src.FromDate))
            .ForMember(dest => dest.ToDate, opt => opt.MapFrom(src => src.ToDate))
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))

               .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModificationDate, opt => opt.MapFrom(src => src.ModificationDate))
                 .ReverseMap()
            .ForMember(dest => dest.DepartmentIds, opt => opt.MapFrom<DepartmentIdsStringResolver>())
            .ForMember(dest => dest.EmployeelevelIds, opt => opt.MapFrom<EmployeelevelIdsStringResolver>())
            .ForMember(dest => dest.UsertypeData, opt => opt.MapFrom<UserTypeDataJsonResolver>())
            ;


            CreateMap<SaveEvaluationSurveySetup, EvaluationSurveySetup>()
                            .ForMember(dest => dest.DepartmentIds, opt => opt.MapFrom(src => string.Join(",", src.DepartmentIds)))
            .ForMember(dest => dest.EmployeelevelIds, opt => opt.MapFrom(src => string.Join(",", src.EmployeelevelIds)))
            .ForMember(dest => dest.UsertypeData, opt => opt.MapFrom<UsertypeDataStringResolver>())
            .ForMember(dest => dest.SurveyId, opt => opt.MapFrom(src=>src.SurveyId))
            .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
            .ForMember(dest => dest.FromDate, opt => opt.MapFrom(src => src.FromDate))
            .ForMember(dest => dest.ToDate, opt => opt.MapFrom(src => src.ToDate))

               .ForMember(dest => dest.ProjectID, opt => opt.Ignore())
               .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
            ;

            #endregion


            #region Lookups

            CreateMap<LookupTable, LookupDto>();

            #endregion


            #region EmployeeLeaves

            CreateMap<EmployeeLeaf, EmployeeLeavesInput>().ForMember(destination => destination.ID,
                options => options.MapFrom(source => source.EmployeeLeaveID));

            CreateMap<EmployeeLeavesInput, EmployeeLeaf>().ForMember(destination => destination.EmployeeLeaveID,
                options => options.MapFrom(source => source.ID));
            CreateMap<EmployeeLeavesUpdate, EmployeeLeaf>().ForMember(destination => destination.EmployeeLeaveID,
                options => options.MapFrom(source => source.ID));

            CreateMap<EmployeeLeaf, EmployeeLeavesOutput>().ForMember(destination => destination.EmployeeName,
                options => options.MapFrom(source => source.Employee.EmployeeName))
                .ForMember(destination => destination.ID,
                options => options.MapFrom(source => source.EmployeeLeaveID));

            #endregion


            #region EmployeeVacations

            CreateMap<EmployeeVacation, EmployeeVacationInput>().ForMember(destination => destination.ID,
                options => options.MapFrom(source => source.EmployeeVacationID));

            CreateMap<EmployeeVacationInput, EmployeeVacation>().ForMember(destination => destination.EmployeeVacationID,
                    options => options.MapFrom(source => source.ID));
            CreateMap<EmployeeVacationsUpdate, EmployeeVacation>().ForMember(destination => destination.EmployeeVacationID,
                    options => options.MapFrom(source => source.ID));

            CreateMap<EmployeeVacation, EmployeeVacationOutput>().ForMember(destination => destination.EmployeeName,
                options => options.MapFrom(source => source.Employee.EmployeeName)).ForMember(destination => destination.ID,
                options => options.MapFrom(source => source.EmployeeVacationID));

            #endregion


            #region EmployeeLoans
            //          source    , distination
            CreateMap<EmployeeLoan, EmployeeLoansInput>().ForMember(destination => destination.ID,
                options => options.MapFrom(source => source.EmployeeLoanID));

            CreateMap<EmployeeLoansInput, EmployeeLoan>().ForMember(destination => destination.EmployeeLoanID,
                options => options.MapFrom(source => source.ID));
            CreateMap<EmployeeLoansUpdate, EmployeeLoan>().ForMember(destination => destination.EmployeeLoanID,
                options => options.MapFrom(source => source.ID));

            CreateMap<EmployeeLoan, EmployeeLoansOutput>().ForMember(destination => destination.EmployeeName,
                options => options.MapFrom(source => source.Employee.EmployeeName))
                .ForMember(destination => destination.ID,
                options => options.MapFrom(source => source.EmployeeLoanID));

            #endregion

            #region Reminders
            CreateMap<GetRemindersResult, RemiderOutput>();
            #endregion

        }
    }
}
