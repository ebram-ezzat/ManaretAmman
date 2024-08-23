using BusinessLogicLayer.Common;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeVacations;
using DataAccessLayer.DTO.Locations;

namespace BusinessLogicLayer.Services.EmployeeVacations
{
    public interface IEmployeeVacationService
    {
        Task Create(EmployeeVacationInput employeeVacation);
        Task Update(EmployeeVacationsUpdate employeeVacation);
        Task Delete(int employeeVacationId);
        Task<EmployeeVacationOutput> Get(int id);
        //Task<PagedResponse<EmployeeVacationOutput>> GetPage(PaginationFilter<EmployeeVacationFilter> filter);
        Task<dynamic> GetPage(PaginationFilter<EmployeeVacationFilter> filter);
        Task<int> DeleteOfficialVacation(DeleteOfficialVacation deleteOfficialVacation);
        Task<int> SaveOfficialVacation(OfficialVacationSaveData officialVacationSaveData);
        Task<object> GetOfficialVacation(OfficialVacationGetInput officialVacationGetInput);
    }
}
