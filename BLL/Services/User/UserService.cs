using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.Users;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.User
{
    public class UserService : IUserService
    {
        private IProjectProvider _projectProvider;
        private readonly ILookupsService _lookupsService;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        public UserService(IProjectProvider projectProvider, ILookupsService lookupsService, PayrolLogOnlyContext payrolLogOnlyContext)
        {
            _projectProvider = projectProvider;
            _lookupsService = lookupsService;
            _payrolLogOnlyContext = payrolLogOnlyContext;
        }

        public async Task<int> UpdateUserFireBaseToken(UpdateUserFirbaseTokenDto updateUserFirbaseTokenDto)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {               
                { "pProjectID",  _projectProvider.GetProjectId() },
                { "pUserID",  updateUserFirbaseTokenDto.UserId},                
                { "ptoken",  updateUserFirbaseTokenDto.FirbaseToken },

            };

            // Define output parameters (optional)
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                 { "pError","int" },
            };

            // Call the ExecuteStoredProcedureAsync function
            var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.UpdateUsertoken", inputParams, outputParams);
            //int pErrorValue = (int)outputValues["pError"];

            return result;
        }
    }
}
