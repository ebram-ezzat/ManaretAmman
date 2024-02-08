using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Permissions;
using DataAccessLayer.Models;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
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
                {"pprojectid",_projectProvider.GetProjectId() },
                 {"pflag",getUserTypeRolesInput.Flag },
                {"ploginuserid",_projectProvider.UserId() }
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
                {"pFromOtherProcedure",insertUser.FromOtherProcedure},
                {"pStatusID",insertUser.StatusID },
                {"pUserTypeID", null},
                {"pcreatedby",_projectProvider.UserId()}
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                {"pUserID",insertUser.UserId.HasValue?insertUser.UserId.Value:"int"},//Input output direction
                { "pError","int" }
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.InsertUsers", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }

        public async Task<List<GetUsersResult>> GetUsers(GetUsersInput getUsersInput)
        {
            var res = await _payrolLogOnlyContext.GetProcedures().GetUsersAsync(getUsersInput.UserId, _projectProvider.GetProjectId()
                , getUsersInput.UserName, getUsersInput.UserPassword, getUsersInput.UserTypeId, getUsersInput.BiosID, getUsersInput.Flag);
            return res;
        }
        #endregion

        #region صلاحيات المستخدم
        public async Task<List<GetUserRolesOutput>> GetUserRoles(GetUserRolesInput getUserRolesInput)
        {
            var inputParams = new Dictionary<string, object>()
            {
                {"pusertypeid",getUserRolesInput.UserId},
                {"pprojectid",_projectProvider.GetProjectId() },
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


    }
}
