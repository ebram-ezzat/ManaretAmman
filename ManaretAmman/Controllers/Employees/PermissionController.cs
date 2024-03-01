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

        /// <summary>
        /// {Flag} must be 2 ,{ProjectId} on the header must be 21
        /// </summary>
        /// <param name="getProjectsInput"></param>
        /// <returns></returns>
        [HttpGet("GetProjects")]
        public async Task<IApiResponse> GetProjects([FromQuery]GetProjectsInput getProjectsInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _permissionService.GetProjectsByFlag2(getProjectsInput);

            return ApiResponse<List<GetProjectsOutPutOfFlag2>>.Success("data has been retrieved succussfully", result);
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
        /// {Flag}=4 , {Bios}=null
        /// </summary>
        /// <remarks></remarks>
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



            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
        #endregion

        #region صلاحيات المستخدم
        /// <summary>
        /// There is no pagination here      
        /// </summary>
        /// <remarks>you can send LogedInUserId from the header,
        /// {Flag} value here it should be 2        
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

        #region صلاحيات المستخدم عند تسجيل دخوله 
        /// <summary>
        /// {Flag} Value here should be 3,
        /// {UserId} Value get from HttpRequestHeader LoggedInUserId and it is Required,
        ///   There is no pagination here 
        /// </summary>
        /// <returns>List of GetLogedInPermissionOutput</returns>
        [HttpGet("GetLogedInPermission")]
        public async Task<IApiResponse> GetLogedInPermission([FromQuery]GetLogedInPermissionInput GetLogedInPermissionInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _permissionService.GetLogedInPermissions(GetLogedInPermissionInput);

            return ApiResponse<List<GetLogedInPermissionOutput>>.Success("data has been reterived succussfully", result);
        }
        #endregion

        #region صلاحيات المستخدم
        /// <summary>
        /// {UserId} string with separted with "; "
        /// </summary>
        /// <param name="insertUserRolesByUserType"></param>
        /// <returns></returns>
        [HttpPost("InsertUserRolesByUserType")]
        public async Task<IApiResponse> InsertUserRolesByUserType([FromBody] InsertUserRolesByUserType insertUserRolesByUserType)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _permissionService.InsertUserRolesByUserType(insertUserRolesByUserType);

            return ApiResponse<int>.Success("data has been saved succussfully", result);
        }

        /// <summary>
        /// There is no pagination here      
        /// </summary>
        /// <remarks>you can send LogedInUserId from the header,
        /// {Flag} value here it should be 4  ,{UserTypeId} is selected from dropdown    
        /// </remarks>           
        /// <param name="getUserRolesByUserTypeInput"></param>
        /// <returns>List Of User Roles </returns>
        [HttpGet("GetUserRolesByUserType")]
        public async Task<IApiResponse> GetUserRolesByUserType([FromQuery] GetUserRolesByUserTypeInput getUserRolesByUserTypeInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _permissionService.GetUserRolesByUserType(getUserRolesByUserTypeInput);



            return ApiResponse<List<GetUserRolesByUserTypeOutput>>.Success("data has been retrieved succussfully", result);
        }
        #endregion

    }
}
