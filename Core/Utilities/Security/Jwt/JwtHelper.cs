using Core.Concrete.Dtos;
using Core.Concrete.Entities;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private readonly TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;
        private int _expire;

        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public AccessToken CreateAccessToken(User user, IEnumerable<UserOperationClaimDto> operationClaims)
        {
            _accessTokenExpiration = DateTime.UtcNow.AddSeconds(_tokenOptions.AccessTokenExpiration);
            _expire = _tokenOptions.AccessTokenExpiration;
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            var accessToken = new AccessToken
            {
                Token = token,
                ExpiresIn = _accessTokenExpiration,
                Expire = _expire,
                RefreshToken = GenerateRefreshToken(),
                UserId = user.Id,
                IsEmailVerified = user.IsEmailVerified,
                IsPhoneVerified = user.IsPhoneVerified
            };
            return accessToken;
        }

        private SecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user, SigningCredentials signingCredentials, IEnumerable<UserOperationClaimDto> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            var generatedToken = Convert.ToBase64String(randomNumber);
            generatedToken = generatedToken.Replace("-", "");
            generatedToken = generatedToken.Replace("+", "");
            generatedToken = generatedToken.Replace("/", "");
            generatedToken = generatedToken.Replace("=", "");
            return generatedToken;
        }


        private static IEnumerable<Claim> SetClaims(User user, IEnumerable<UserOperationClaimDto> operationclaims)
        {
            ICollection<Claim> claims = new List<Claim>();
            claims.AddUserId(user.Id.ToString());
            claims.AddTcNo(user.TcNo);
            claims.AddEmail(user.Email);
            claims.AddEmailVerification(user.IsEmailVerified);
            claims.AddPhone(user.PhoneNumber);
            claims.AddPhoneVerification(user.IsPhoneVerified);
            claims.AddRoles(operationclaims.Select(o => o.Role).ToArray());

            return claims;
        }
    }
}
