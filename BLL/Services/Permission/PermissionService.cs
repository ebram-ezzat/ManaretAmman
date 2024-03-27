using Azure.Core;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Permissions;
using DataAccessLayer.Models;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Permission
{
    public class PermissionService:IPermissionService
    {
        private PayrolLogOnlyContext _payrolLogOnlyContext;
        private IProjectProvider _projectProvider;
        private readonly ILookupsService _lookupsService;
       
        public PermissionService(PayrolLogOnlyContext payrolLogOnlyContext, IProjectProvider projectProvider, ILookupsService lookupsService) {
            _payrolLogOnlyContext = payrolLogOnlyContext;
            _projectProvider= projectProvider;
            _lookupsService= lookupsService;
        }
        #region صلاحيات نوع المستخدم
        public async Task<List<GetUserTypeRolesOutput>> GetUserTypeRoles(GetUserTypeRolesInput getUserTypeRolesInput)
        {
            var inputParams = new Dictionary<string, object>()
            {
                { "pusertypeid",getUserTypeRolesInput.UserTypeId},
                {"pprojectid",_projectProvider.GetProjectId()},
                 {"pflag",getUserTypeRolesInput.Flag},
                {"ploginuserid",_projectProvider.UserId()},
                {"pCurrentProjectID",getUserTypeRolesInput.CurrentProjectID??Convert.DBNull}
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetUserTypeRolesOutput>("dbo.Getusertyperoles", inputParams, null);
            return result;
        }
        public async Task<int> InsertUserTypeRoles(InsertUserTypeRoles insertUserTypeRoles)
        {
            var inputParams = new Dictionary<string, object>()
            {
                { "pusertypeid",insertUserTypeRoles.UserTypeId},
                {"pprojectid",_projectProvider.GetProjectId() },
                 {"proleid",insertUserTypeRoles.RoleId },
                {"pcreatedby",_projectProvider.UserId() },
                 {"pAllowEdit",insertUserTypeRoles.AllowEdit },
                 {"pAllowDelete",insertUserTypeRoles.AllowDelete},
                 {"pAllowAdd",insertUserTypeRoles.AllowAdd}
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "pError","int" },
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.Insertusertyperoles", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }


        public async Task<List<GetProjectsOutPutOfFlag2>> GetProjectsByFlag2(GetProjectsInput getProjectsInput)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pProjectID",Convert.DBNull},//here not needed 
                {"pflag",getProjectsInput.Flag},                
                {"pSearch",getProjectsInput.Search?? Convert.DBNull}
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetProjectsOutPutOfFlag2>("dbo.GetProjects", inputParams, null);
            return result;
        }
        #endregion

        #region المستخدمين
        public async Task<int> DeleteUsers(DeleteUser deleteUser)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pUserID",deleteUser.UserId},
                {"pProjectID",_projectProvider.GetProjectId() }
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "pError","int" },
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.DeleteUsers", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }

        public async Task<int> InsertUsers(InsertUser insertUser)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pUserName",insertUser.UserName},
                {"pUserPassword",insertUser.UserPassword},
                {"pProjectID",_projectProvider.GetProjectId()},
                {"pFromOtherProcedure", Convert.DBNull},
                {"pStatusID",insertUser.StatusID },
                {"pUserTypeID", Convert.DBNull},
                {"pcreatedby",_projectProvider.UserId()}
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                {"pUserID",insertUser.UserId.HasValue && insertUser.UserId > 0 ? insertUser.UserId.Value:"int"},//Input output direction
                { "pError","int" }
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertUsers", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }

        public async Task<dynamic> GetUsers(GetUsersInput getUsersInput)
        {
            var returnValue = new OutputParameter<int>();
            var rowCount = new OutputParameter<int>();
            var res = await _payrolLogOnlyContext.GetProcedures().GetUsersAsync(getUsersInput.UserId, _projectProvider.GetProjectId()
                , getUsersInput.UserName, getUsersInput.UserPassword, getUsersInput.UserTypeId, getUsersInput.BiosID, getUsersInput.Flag,getUsersInput.PageNo,getUsersInput.PageSize, returnValue, rowCount);

            dynamic obj = new ExpandoObject();
            var totalPages = ((double)rowCount.Value / (double)getUsersInput.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            obj.totalPages = roundedTotalPages;
            obj.result = res;
            obj.pageIndex = getUsersInput.PageNo;
            obj.offset = getUsersInput.PageSize;
            return obj;
        }
        #endregion

        #region صلاحيات المستخدم
        public async Task<List<GetUserRolesOutput>> GetUserRoles(GetUserRolesInput getUserRolesInput)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"puserid",Convert.DBNull},
                {"pprojectid",_projectProvider.GetProjectId() },
                {"pusertypeid",getUserRolesInput.UserId},               
                {"pflag",getUserRolesInput.Flag },
                {"ploginuserid",_projectProvider.UserId() }
            };
            
             
            var (userRoles, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetUserRolesOutput>("dbo.Getuserroles", inputParams, null);
            return userRoles;

        }

        public async Task<int> InsertUserRoles(InsertUserRolesInput insertUserRolesInput)
        {

            var inputParams = new Dictionary<string, object>()
            {
                { "puserid",insertUserRolesInput.UserId},
                {"pprojectid",_projectProvider.GetProjectId() },
                {"pusertypeid",insertUserRolesInput.UserTypeId },
                {"pcreatedby",_projectProvider.UserId() }
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "pError","int" },
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.Insertuserroles", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }



        #endregion

        #region صلاحيات المستخدمين

        #endregion

        #region صلاحيات المستخدم عند تسجيل دخوله 
        /// <summary>
        /// Retrieves the permissions for a logged-in user based on the provided input.
        /// </summary>
        /// <param name="getLogedInPermissionInput">An object containing the user ID and flag to retrieve permissions for.</param>
        /// <returns>
        /// A list of permission outputs containing role ID, edit, delete, add, and default value permissions for the logged-in user.
        /// </returns>
        public async Task<List<GetLogedInPermissionOutput>> GetLogedInPermissions(GetLogedInPermissionInput getLogedInPermissionInput)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"puserid",_projectProvider.UserId()},
                {"pprojectid",_projectProvider.GetProjectId()},
                {"pflag",getLogedInPermissionInput.Flag },
                {"ploginuserid",Convert.DBNull },//here he used the userid as a logedinuser on the stored procedure so i don't send it twice
                {"pusertypeid",Convert.DBNull}
            };
         


            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetLogedInPermissionOutput>("dbo.Getuserroles", inputParams, null);
            return result;
        }


        #endregion
        #region صلاحيات المستخدم
        public async Task<int> InsertUserRolesByUserType(InsertUserRolesByUserType insertUserRolesByUserType)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"puserid",insertUserRolesByUserType.UserId},
                {"pprojectid",_projectProvider.GetProjectId()},
                {"pusertypeid",insertUserRolesByUserType.UserTypeId},
                {"pcreatedby",_projectProvider.UserId()}                
            };
            


            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "pError","int" },
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertuserrolesByUserType", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }

        public async Task<List<GetUserRolesByUserTypeOutput>> GetUserRolesByUserType(GetUserRolesByUserTypeInput getUserRolesByUserTypeInput)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"puserid",Convert.DBNull},
                {"pprojectid",_projectProvider.GetProjectId() },
                 {"pflag",getUserRolesByUserTypeInput.Flag },
                 {"ploginuserid",_projectProvider.UserId() },
                {"pusertypeid",getUserRolesByUserTypeInput.UserTypeID}            
               
            };

            var (userRoles, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetUserRolesByUserTypeOutput>("dbo.Getuserroles", inputParams, null);
            return userRoles;
        }
        #endregion
    }
}
