using Core.Concrete;
using Core.Concrete.Dtos;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        #region Bu methodların Authentication olmaya ihtiyacı yoktur. Authenticate olmayı sağlar
        Task<IDataResult<UserDto>> Register(UserRegisterDto userRegisterDto);
        Task<IDataResult<UserDto>> Login(UserLoginDto userLoginDto);
        Task<IDataResult<AccessToken>> LoginWithRefreshToken(RefreshTokenDto refreshToken);
        #endregion

        
        Task<IResult> Logout(Guid userId);
        Task<IResult> ChangePassword(ChangePasswordDto passwordDto);
        Task<IDataResult<ForgotPasswordDto>> ForgotPassword(string email); 
        Task<IResult> ResetPassword(ResetPasswordDto resetPasswordDto); 
        Task<IResult> UserExists(string tcNo);
        Task<IDataResult<AccessToken>> CreateAccessToken(UserDto userDto);
    }
}
