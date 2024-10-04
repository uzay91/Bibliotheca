using AutoMapper;
using Business.Abstract;
using Core.Concrete.Dtos;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotheca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IMapper _mapper;
        public AuthController(IAuthService authService, IUserService userService, ITokenHelper tokenHelper, IMapper mapper)
        {
            _authService = authService;
            _userService = userService;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
        }

        /// <summary>
        /// This method allows you to register the bibliotheca
        /// </summary>
        /// <remarks>Deneme </remarks>
        /// <param name="userRegisterDto"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDataResult<UserDto>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDataResult<UserDto>))]
        [HttpPost("register")]// => attribute, data annotations
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            var result = await _authService.Register(userRegisterDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Status, result);
        }


        /// <summary>
        /// Bu metod sayesinde log in olunur
        /// </summary>
        /// <param name="userLoginDto"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccessToken))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var userLogin = await _authService.Login(userLoginDto);

            if (userLogin.Success)
            {
                var result = await _authService.CreateAccessToken(userLogin.Data);

                var user = await _userService.GetByUsername(userLoginDto.UserName);
                user.Data.RefreshToken = result.Data.RefreshToken;
                await _userService.Update(user.Data);

                return Ok(result.Data);
            }
            return BadRequest();
        }

        [HttpPost("login/refresh")]
        public async Task<IActionResult> LoginWithRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authService.LoginWithRefreshToken(refreshTokenDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Status, result);
        }

        /// <summary>
        /// bu method log out olmaya izin verir
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(Guid userId)
        {
            var result = await _authService.Logout(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Status, result);
        }

        [HttpPut("Changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var result = await _authService.ChangePassword(changePasswordDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Status, result);
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _authService.ForgotPassword(email);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Status, result);

        }

        [HttpPost("ForgotPassword/Reset")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var result = await _authService.ResetPassword(resetPasswordDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Status, result);
        }

    }
}
