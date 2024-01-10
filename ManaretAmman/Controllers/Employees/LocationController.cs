using BusinessLogicLayer.Services.Employees;
using BusinessLogicLayer.Services.Location;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Locations;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private ILocationService _locationService;
        public LocationController(ILocationService locationService) 
        {
            _locationService = locationService;
        }
        /* Employee Locations*/
        #region
        [HttpGet("GetEmployeeLocation")]
        public async Task<IApiResponse> GetEmployeeLocation([FromQuery]GetEmployeeLocationInput getEmployeeLocationInput)
        {

            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _locationService.GetEmployeeLocation(getEmployeeLocationInput);



            return ApiResponse<List<GetEmployeeLocationResponse>>.Success("data has been retrieved succussfully", result);
        }

        [HttpPost("SaveEmployeeLocation")]
        public async Task<IApiResponse> SaveEmployeeLocation(InsertEmployeeLocation insertEmployeeLocation)
        {
          
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _locationService.SaveEmployeeLocationProc(insertEmployeeLocation);



            return ApiResponse<int>.Success("data has been Saved succussfully", result);
        }
        [HttpDelete("DeleteEmployeeLocation")]
        public async Task<IApiResponse> DeleteEmployeeLocation([FromQuery]DeleteEmployeeLocation deleteEmployeeLocation)
        {

            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _locationService.DeleteEmployeeLocationProc(deleteEmployeeLocation);



            return ApiResponse<int>.Success("data has been Deleted succussfully", result);
        }

        #endregion
        /* Company Locations */
        #region
        [HttpPost("SaveLocation")]
        public async Task<IApiResponse> SaveLocation(InsertLocation insertLocation)
        {

            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _locationService.SaveCompanyLocationProc(insertLocation);



            return ApiResponse<int>.Success("data has been Saved succussfully", result);
        }
        [HttpDelete("DeleteLocation")]
        public async Task<IApiResponse> DeleteLocation([FromQuery] int LocationID)
        {

            if (LocationID<=0)
            {
              

                return ApiResponse.Failure(" An unexpected error on validation occurred ", new string[] { "LocationID must be bigger than 0" });
            }
            var result = await _locationService.DeleteCompanyLocationProc(LocationID);



            return ApiResponse<int>.Success("data has been Deleted succussfully", result);
        }
        [HttpPut("UpdateLocation")]
        public async Task<IApiResponse> UpdateLocation(UpdateLocation updateLocation)
        {

            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _locationService.UpdateCompanyLocationProc(updateLocation);

            if(result<0)// perror -1
                return ApiResponse.Failure(" An unexpected error on Update occurred");
            return ApiResponse<int>.Success("data has been update succussfully", result);
        }
        [HttpGet("GetLocations")]
        public async Task<IApiResponse> GetLocations(GetLocationsInput getLocationsInput)
        {

            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _locationService.GetCompanyLocation(getLocationsInput);
          
            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        #endregion
    }
}
