using AutoMapper;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeLeaves;
using DataAccessLayer.DTO.EmployeeLoans;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.EmployeeVacations;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;

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
