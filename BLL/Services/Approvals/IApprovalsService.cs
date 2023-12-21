using BusinessLogicLayer.Common;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Approvals
{
    public interface IApprovalsService
    {
        Task<object> GetVacationApprovalsAsync(PaginationFilter<GetEmployeeNotificationInput> filter);
        Task<int> SaveWorkEmployeeApprovals(WorkEmployeeApprovals workEmployeeApprovals);
        Task<(int, Dictionary<string, object>)> SaveOverTimeWorkEmployee(SaveOverTimeWorkEmployee saveOverTimeWorkEmployee);
        Task<int> DeleteOverTimeWorkEmployee(DeleteOverTimeWorkEmployee deleteOverTimeWorkEmployee);
        Task<object> GetOverTimeWorkEmployee(GetOverTimeWorkEmployeeInputModel  inputModel);
    }
}
