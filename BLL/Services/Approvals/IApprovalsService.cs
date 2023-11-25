using BusinessLogicLayer.Common;
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
        public Task<object> GetVacationApprovalsAsync(PaginationFilter<GetEmployeeNotificationInput> filter);
    }
}
