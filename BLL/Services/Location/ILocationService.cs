using DataAccessLayer.DTO.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Location
{
    public interface ILocationService
    {
        Task<int> SaveEmployeeLocationProc(InsertEmployeeLocation saveEmployeeLocation);
        Task<object> GetEmployeeLocation(GetEmployeeLocationInput getEmployeeLocationInput);
        Task<int> DeleteEmployeeLocationProc(DeleteEmployeeLocation deleteEmployeeLocation);

        Task<int> SaveCompanyLocationProc(InsertLocation saveEmployeeLocation);
        Task<int> UpdateCompanyLocationProc(UpdateLocation saveEmployeeLocation);
        Task<int> DeleteCompanyLocationProc(int LocationID);
        Task<object> GetCompanyLocation(GetLocationsInput getLocationsInput);

    }
}
