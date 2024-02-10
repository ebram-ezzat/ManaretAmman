using DataAccessLayer.DTO.Permissions;
using DataAccessLayer.Models;
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

        public Task<int> DeleteUsers(DeleteUser deleteUser);
        public Task<int> InsertUsers(InsertUser insertUser);
        public Task<dynamic> GetUsers(GetUsersInput getUsersInput);
        public Task<List<GetUserRolesOutput>> GetUserRoles(GetUserRolesInput getUserRolesInput);
        public Task<int> InsertUserRoles(InsertUserRolesInput getUserRolesInput);
        public Task<List<GetLogedInPermissionOutput>> GetLogedInPermissions(GetLogedInPermissionInput getLogedInPermissionInput);
    }
}
