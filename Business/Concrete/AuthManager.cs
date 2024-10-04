using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Concrete;
using Core.Concrete.Dtos;
using Core.Concrete.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using Core.Aspects.Autofac.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IMapper _mapper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IOperationClaimService operationClaimService,
            IUserOperationClaimService userOperationClaimService, IMapper mapper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _operationClaimService = operationClaimService;
            _userOperationClaimService = userOperationClaimService;
            _mapper = mapper;
        }

        [ValidationAspect(typeof(RegisterValidator),Priorty =1)]
        [TransactionScopeAspectAsync]
        public async Task<IDataResult<UserDto>> Register(UserRegisterDto userRegisterDto)
        {
            var userExist = await UserExists(userRegisterDto.TcNo);
            if (userExist.Success)
            {
                return new ErrorDataResult<UserDto>(409, Message.AlreadyRegistered);
            }
            // Tc kontrolü yapılacak mersis üzerinden

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userRegisterDto.Password, out passwordHash, out passwordSalt);

            var user = new User()
            {
                TcNo = userRegisterDto.TcNo,
                FullName = userRegisterDto.FullName,
                LastName = userRegisterDto.LastName,
                BirthDate = userRegisterDto.BirthDate,
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.Email,
                PhoneNumber = userRegisterDto.PhoneNumber,
                IsEmailVerified = false,
                IsPhoneVerified = false,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RefreshToken = null,
                ResetToken = null,
            };

            var addedUser = await _userService.Add(user);
            if (!addedUser.Success)
            {
                return new ErrorDataResult<UserDto>(404);
            }

            var operationClaim = await _operationClaimService.GetByRole("user");

            var userOperationClaim = new UserOperationClaim()
            {
                OperationClaimId = operationClaim.Data.Id,
                UserId = addedUser.Data.Id,
            };

            await _userOperationClaimService.Add(userOperationClaim);

            var result = _mapper.Map<UserDto>(addedUser.Data);
            return new SuccessDataResult<UserDto>(result);

        }

        public async Task<IDataResult<UserDto>> Login(UserLoginDto userLoginDto)
        {
            var userToCheck = await _userService.GetByUsername(userLoginDto.UserName);
            if (!userToCheck.Success)
            {
                return new ErrorDataResult<UserDto>(404, Message.NotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
            {
                return new ErrorDataResult<UserDto>(401, Message.WrongPassword);
            }

            var result = _mapper.Map<UserDto>(userToCheck.Data);
            return new SuccessDataResult<UserDto>(result);
        }

        public async Task<IDataResult<AccessToken>> LoginWithRefreshToken(RefreshTokenDto refreshToken)
        {
            var user = await _userService.GetByRefreshToken(refreshToken.RefreshToken, refreshToken.UserId);
            if (user.Success == false)
            {
                return new ErrorDataResult<AccessToken>(404, Message.NotFound);
            }
            var userDto = _mapper.Map<UserDto>(user);
            var accessToken = await CreateAccessToken(userDto);
            user.Data.RefreshToken = accessToken.Data.RefreshToken;
            await _userService.Update(user.Data);

            return new SuccessDataResult<AccessToken>();
        }

        public async Task<IResult> Logout(Guid userId)
        {
            var user = await _userService.GetById(userId);
            if (user.Success == false)
            {
                return new ErrorResult(404, Message.NotFound);
            }
            user.Data.RefreshToken = null;
            await _userService.Update(user.Data);
            return new SuccessResult();
        }

        public async Task<IResult> ChangePassword(ChangePasswordDto passwordDto)
        {
            var currentUser = await _userService.GetById(passwordDto.UserId);

            
            if (!HashingHelper.VerifyPasswordHash(passwordDto.CurrentPassword, currentUser.Data.PasswordHash, currentUser.Data.PasswordSalt))
            {
                return new ErrorResult(400, Message.WrongCurrentPassword);
            }

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(passwordDto.NewPassword, out passwordHash, out passwordSalt);

            currentUser.Data.PasswordHash = passwordHash;
            currentUser.Data.PasswordSalt = passwordSalt;
            await _userService.Update(currentUser.Data);
            return new SuccessResult(status: 200, message: Message.PasswordChanged);
        }

        public async Task<IDataResult<ForgotPasswordDto>> ForgotPassword(string email)
        {
            var user = await _userService.GetByEmail(email);
            if (!user.Success)
            {
                return new ErrorDataResult<ForgotPasswordDto>(404, Message.NotFound);

            }
            var generatedResetToken = GenerateResetPassToken();
            user.Data.ResetToken = generatedResetToken;
            await _userService.Update(user.Data);

            var resetUrl = $"https://bibliotheca.com/api/auth/forgot_password/reset?token={generatedResetToken}&uid={user.Data.Id}";

            var result = new ForgotPasswordDto
            {
                ForgotUrl = resetUrl,
            };


            return new SuccessDataResult<ForgotPasswordDto>(result);
        }

        public async Task<IResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userService.GetById(resetPasswordDto.UserId);
            if (user.Success && user.Data.ResetToken == resetPasswordDto.ResetToken)
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(resetPasswordDto.NewPassword, out passwordHash, out passwordSalt);

                user.Data.PasswordHash = passwordHash;
                user.Data.PasswordSalt = passwordSalt;
                user.Data.ResetToken = null;
                await _userService.Update(user.Data);
                return new SuccessResult(200);

            }
            return new ErrorResult(400, Message.NotFound);

        }

        public async Task<IResult> UserExists(string tcNo)
        {
            var user = await _userService.GetByTcNo(tcNo);
            if (user.Data == null)
            {
                return new ErrorResult(404, Message.NotFound);
            }
            return new SuccessResult(200);
        }

        public async Task<IDataResult<AccessToken>> CreateAccessToken(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var claims = await _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateAccessToken(user, claims.Data);
            return new SuccessDataResult<AccessToken>(accessToken);

        }

        private static string GenerateResetPassToken()
        {
            var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var resetToken = new string(
                Enumerable.Repeat(allChar.ToLower(), 128)
                    .Select(token => token[random.Next(token.Length)]).ToArray());

            return resetToken;
        }
    }
}
