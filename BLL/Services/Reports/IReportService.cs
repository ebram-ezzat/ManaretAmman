using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Reports
{
    public interface IReportService
    {
        Task<object> GetEmployeeSalaryReport(GetEmployeeSalaryReportRequest getEmployeePaperRequest);
        Task<object> GetEmployeeAttendanceDailyReport(GetEmployeeAttendanceDailyRequest getEmployeeAttendanceDailyRequest);

    }
}
