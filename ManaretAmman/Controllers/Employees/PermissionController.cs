using BusinessLogicLayer.Services.Employees;
using BusinessLogicLayer.Services.Permission;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Permissions;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private IPermissionService _permissionService {  get; set; }
        public PermissionController(IPermissionService permissionService) {
            _permissionService= permissionService;
        }
        #region
        /*شاشة صلاحيات نوع المستخدم*/
        /// <summary>
        ///جيت شاشة صلاحيات نوع المستخدم.
        /// </summary>
        /// <param name = "getUserTypeRolesInput" > you should send Logedin userId on the header,
        /// flag is by default = 1 you can don't send it if you want the default value
        /// </param>
        /// <returns>A list of user type roles.there is no pagination response</returns>

        [HttpGet("GetUserTypeRoles")]
        public async Task <IApiResponse> GetUserTypeRoles([FromQuery] GetUserTypeRolesInput getUserTypeRolesInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _permissionService.GetUserTypeRoles(getUserTypeRolesInput);


            return ApiResponse<List<GetUserTypeRolesOutput>>.Success("data has been retrieved succussfully", result);
        }
        /// <summary>
        /// حفظ شاشة صلاحيات نوع المستخدم .
        /// </summary>
        /// <param name="insertUserTypeRoles">you should send Logedin userId on the header,
        /// RoleId is the string of ids sperated by comma","
        /// </param>
        /// <returns>A return Status of save </returns>

        [HttpPost("InsertUserTypeRoles")]
        public async Task<IApiResponse> InsertUserTypeRoles([FromBody]InsertUserTypeRoles insertUserTypeRoles)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _permissionService.InsertUserTypeRoles(insertUserTypeRoles);


            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }
        #endregion
        [HttpGet("GetUserRoles")]
        public async Task<IApiResponse> GetUserRoles([FromQuery] GetUserRolesInput getUserRolesInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _permissionService.GetUserRoles(getUserRolesInput);



            return ApiResponse<List<GetUserRolesOutput>>.Success("data has been retrieved succussfully", result);
        }

        [HttpPost("GetUserRoles")]
        public async Task<IApiResponse> InsertUserRoles([FromBody] InsertUserRolesInput getUserRolesInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _permissionService.GetUserRoles(getUserRolesInput);


            return null;
            //return ApiResponse<List<GetUserRolesOutput>>.Success("data has been retrieved succussfully", result);
        }
    }
}
