using BusinessLogicLayer.Services.Employees;
using BusinessLogicLayer.Services.Permission;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Permissions;
using DataAccessLayer.Models;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        #region شاشة صلاحيات نوع المستخدم

        /// <summary>
        /// There is no pagination here
        ///جيت شاشة صلاحيات نوع المستخدم.
        /// </summary>
        ///  <remarks>
        ///  you should send Logedin userId on the header,
        /// {flag} is by default = 1 you can don't send it if you want the default value
        ///  </remarks>
        /// <param name = "getUserTypeRolesInput" > 
        /// </param>
        /// <returns>A list of user type roles.</returns>

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
        /// RoleId and AllowEdit and AllowDelete and AllowAdd fields is the string of ids sperated by comma";"
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

        #region  شاشة المستخدمين

        /// <summary>
        ///
        /// </summary>
        /// <param name = "User" > you should send Logedin userId on the header,
        /// flag is by default = 1 you can don't send it if you want the default value
        /// </param>
        /// <returns>Status of Delete operation</returns>
        [HttpDelete("DeleteUser")]
        public async Task<IApiResponse> DeleteUser([FromQuery]DeleteUser User)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _permissionService.DeleteUsers(User);


            return ApiResponse<int>.Success("data has been deleted succussfully", result);
        }

        /// <summary>
        /// you can use this function for insert or update 
        /// </summary>
        /// <parm name="insertUser">to use this Api as Update you should send UserId</parm>
        /// <returns>Status of Insert operation</returns>
        [HttpPost("InsertUser")]
        public async Task<IApiResponse> InsertUser([FromBody]InsertUser insertUser)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _permissionService.InsertUsers(insertUser);


            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }

        /// <summary>
        /// there is no pagination response
        /// </summary>
        /// <remarks>{flag}=2 , {Bios}=null</remarks>
        /// <param name = "getUsersInput" >
        /// </param>
        /// <returns>List of Users </returns>

        [HttpGet("GetUsers")]
        public async Task<IApiResponse> GetUsers([FromQuery] GetUsersInput getUsersInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _permissionService.GetUsers(getUsersInput);



            return ApiResponse<List<GetUsersResult>>.Success("data has been retrieved succussfully", result);
        }
        #endregion

        #region صلاحيات المستخدم
        /// <summary>
        /// There is no pagination here      
        /// </summary>
        /// <remarks>you can send LogedInUserId from the header,
        /// {Flag} default valus is 1 you can don't send it if you want the default
        /// </remarks>           
        /// <param name="getUserRolesInput"></param>
        /// <returns>List Of User Roles </returns>
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

        [HttpPost("InsertUserRoles")]
        public async Task<IApiResponse> InsertUserRoles([FromBody] InsertUserRolesInput insertUserRolesInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _permissionService.InsertUserRoles(insertUserRolesInput);
            
            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }
        #endregion

    }
}
