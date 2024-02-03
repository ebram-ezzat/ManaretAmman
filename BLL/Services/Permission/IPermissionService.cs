using DataAccessLayer.DTO.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Permission
{
    public interface IPermissionService
    {
        public Task<List<GetUserTypeRolesOutput>> GetUserTypeRoles(GetUserTypeRolesInput getUserTypeRolesInput);
        public Task<int> InsertUserTypeRoles(InsertUserTypeRoles insertUserTypeRoles);
        public Task<List<GetUserRolesOutput>> GetUserRoles(GetUserRolesInput getUserRolesInput);
        public Task<int> GetUserRoles(InsertUserRolesInput getUserRolesInput);
    }
}
