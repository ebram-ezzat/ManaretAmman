using BusinessLogicLayer.Services.User;
using DataAccessLayer.DTO.Users;
using LanguageExt.Common;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _IUserService;
        public UserController(IUserService userService)
        {
            _IUserService = userService;
        }
        [HttpPost("UpdateUserFireBaseToken")]
        public async Task<IApiResponse> UpdateUserFireBaseToken([FromBody] UpdateUserFirbaseTokenDto updateUserFirbaseTokenDto)
        {
            if (updateUserFirbaseTokenDto == null || string.IsNullOrEmpty(updateUserFirbaseTokenDto.FirbaseToken) || updateUserFirbaseTokenDto.UserId <= 0)
            {
                return ApiResponse.Failure(" An unexpected error on validation of data occurred");
            }
            var result = await _IUserService.UpdateUserFireBaseToken(updateUserFirbaseTokenDto);
            return ApiResponse<int>.Success("data has been update succussfully", result);
        }
    }
}
