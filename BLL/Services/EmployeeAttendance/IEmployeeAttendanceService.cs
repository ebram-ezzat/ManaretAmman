﻿using BusinessLogicLayer.Common;
using DataAccessLayer.DTO.EmployeeAttendance;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.EmployeeAttendance
{
    public interface IEmployeeAttendanceService
    {
        public Task<PagedResponse<EmployeeAttendanceOutput>> GetEmployeeAttendance(PaginationFilter<EmployeeAttendanceInput> filter);
       public Task<dynamic> GetEmployeeAttendanceTreatment(EmployeeAttendanceTreatmentInput employeeAttendanceInput);
        public Task<int> SaveEmployeeAttendanceTreatment(List<SaveEmployeeLeaveInput> saveEmployeeLeaveInput);

        public Task<int> SaveEmployeeVacationTreatment(List<SaveEmployeeVacationInput> saveEmployeeLeaveInput);

    }
}
