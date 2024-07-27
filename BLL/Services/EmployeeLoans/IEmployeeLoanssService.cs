using BusinessLogicLayer.Common;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeLoans;

namespace BusinessLogicLayer.Services.EmployeeLoans
{
    public interface IEmployeeLoansService
    {
        Task<int> Create(EmployeeLoansInput employee);
        Task<int> Update(EmployeeLoansUpdate employee);
        Task Delete(int employeeLoanId);
        Task<EmployeeLoansOutput> Get(int id);
        Task<dynamic> GetPage(PaginationFilter<EmployeeLoanFilter> filter);
        Task<int> CreateScheduledLoans(SchededuledLoansInput employees);
        Task<int> UpdateScheduledLoans(SchededuledLoansInput employees);

    }
}
