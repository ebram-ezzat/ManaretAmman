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

        public async Task<List<GetUserRolesOutput>> GetUserRoles(GetUserRolesInput getUserRolesInput)
        {
            var parameters = PublicHelper.GetPropertiesWithPrefix<GetUserRolesInput>(getUserRolesInput, "p");
           
            var (userRoles, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<GetUserRolesOutput>("dbo.Getuserroles", parameters, null);
            return userRoles;

        }

        public Task<int> GetUserRoles(InsertUserRolesInput insertUserRolesInput)
        {
            return null;
            //var parameters = PublicHelper.GetPropertiesWithPrefix<GetUserRolesInput>(insertUserRolesInput, "p");

            //var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.Getuserroles", parameters, null);
            //return userRoles;
        }

        public async Task<List<GetUserTypeRolesOutput>> GetUserTypeRoles(GetUserTypeRolesInput getUserTypeRolesInput)
        {
            var inputParams = new Dictionary<string, object>()
            {
                { "pusertypeid",getUserTypeRolesInput.UserTypeId},
                {"pprojectid",_projectProvider.GetProjectId() },
                 {"pflag",getUserTypeRolesInput.Falg },
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
                {"pcreatedby",_projectProvider.UserId() }
            };
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {                
                { "pError","int" },
            };
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.Insertusertyperoles", inputParams, outputParams);
            int pErrorValue = (int)outputValues["pError"];
            return pErrorValue;
        }
    }
}
