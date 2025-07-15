using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DTO.Users;

namespace BusinessLogicLayer.Services.User
{
    public interface IUserService
    {
        public Task<int> UpdateUserFireBaseToken(UpdateUserFirbaseTokenDto updateUserFirbaseTokenDto);
    }
}
