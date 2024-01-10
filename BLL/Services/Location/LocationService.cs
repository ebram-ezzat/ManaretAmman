using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Locations;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;


namespace BusinessLogicLayer.Services.Location
{
    public class LocationService : ILocationService
    {
        private readonly IProjectProvider _projectProvider;
        private readonly IConfiguration _configuration;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        public LocationService(IProjectProvider projectProvider, IConfiguration configuration, PayrolLogOnlyContext payrolLogOnlyContext) 
        {
            _projectProvider = projectProvider;
            _configuration = configuration;
            _payrolLogOnlyContext = payrolLogOnlyContext;
        }

        #region EmployeeLocation 
        public async Task<int> DeleteEmployeeLocationProc(DeleteEmployeeLocation deleteEmployeeLocation)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", deleteEmployeeLocation.EmployeeID},
                { "pLocationID", deleteEmployeeLocation.LocationID },
               

            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
             {

                { "pError","int" },

            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteEmployeeLocations", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];

            
            return result;
        }
        public async Task<int> SaveEmployeeLocationProc(InsertEmployeeLocation saveEmployeeLocation)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pLocationID", saveEmployeeLocation.LocationID},
                { "pEmployeeID", saveEmployeeLocation.EmployeeID },
                { "pDistance", saveEmployeeLocation.Distance},
                { "pStartDate", saveEmployeeLocation.StartDate},
                { "pEndDate",saveEmployeeLocation.EndDate},
                { "pCreatedBy",_projectProvider.UserId()},
                { "pAnyWhere",saveEmployeeLocation.AnyWhere},
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
             {

                { "pError","int" },

            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertEmployeeLocations", inputParams, outputParams);
            return (int)result;
        }
        public async Task<List<GetEmployeeLocationResponse>> GetEmployeeLocation(GetEmployeeLocationInput getEmployeeLocationInput)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pEmployeeID", getEmployeeLocationInput.EmployeeID},
                { "pLocationID", getEmployeeLocationInput.LocationID },
                { "pProjectID", _projectProvider.GetProjectId() },

            };
           
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetEmployeeLocationResponse>("dbo.GetEmployeeLocations", inputParams, null);
            return result;
        }
        #endregion
        #region CompanyLocation
        public async Task<int> SaveCompanyLocationProc(InsertLocation saveLocation)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pProjectID",  _projectProvider.GetProjectId() },
                { "pAlias", saveLocation.Alias },
                { "pDistance", saveLocation.Distance},
                { "pLongitude", saveLocation.Longitude },
                { "pLatitude",saveLocation.Latitude},
                { "pCreatedBy",_projectProvider.UserId()},
               
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
             {                
                { "pError","int" },
                { "pLocationID","int"},

            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertLocations", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            if (pErrorValue == 1)
                return (int)outputValues["pLocationID"];
            return pErrorValue;
        }       

        public async Task<int> UpdateCompanyLocationProc(UpdateLocation updateLocation)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pLocationID", updateLocation.LocationID },
                { "pProjectID",  _projectProvider.GetProjectId() },
                { "pAlias", updateLocation.Alias },
                { "pDistance", updateLocation.Distance},
                { "pLongitude", updateLocation.Longitude },
                { "pLatitude",updateLocation.Latitude},
                { "pModifiedBy",_projectProvider.UserId()},

            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
             {
                { "pError","int" },
                

            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.UpdateLocations", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            
            return pErrorValue;
        }
        public async Task<int> DeleteCompanyLocationProc(int LocationID)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {

                { "pLocationID",LocationID },


            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
             {

                { "pError","int" },

            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteLocations", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];


            return result;
        }

        public async Task<object> GetCompanyLocation(GetLocationsInput getLocationsInput)
        {

            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {               
                {"pProjectID", _projectProvider.GetProjectId() },
                {"pPageNo",getLocationsInput.PageNo },
                {"pPageSize",getLocationsInput.PageSize}
            };

            Dictionary<string, object> outputParams = new Dictionary<string, object>
             {

                { "prowcount","int" },

            };

            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetLocationsResponse>("dbo.GetLocations", inputParams, outputParams);
            return PublicHelper.CreateResultPaginationObject(getLocationsInput, result, outputValues);

          
        }
        #endregion
    }
}
