using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ClaimExtensions
    {
        public static void AddUserId(this ICollection<Claim> claims, string userId)
        {
            claims.Add(new Claim("userId", userId));
        }

        public static void AddTcNo(this ICollection<Claim> claims, string tcNo)
        {
            claims.Add(new Claim("tcNo", tcNo));
        }

        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            claims.Add(new Claim("email", email));
        }
        public static void AddEmailVerification(this ICollection<Claim> claims, bool emailVerification)
        {
            claims.Add(new Claim("emailVerification", emailVerification.ToString().ToLower()));
        }
        public static void AddPhone(this ICollection<Claim> claims, string phone)
        {
            claims.Add(new Claim("phone", phone));
        }
        public static void AddPhoneVerification(this ICollection<Claim> claims, bool phoneVerification)
        {
            claims.Add(new Claim("phoneVerification", phoneVerification.ToString().ToLower()));
        }
        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));


        }
    }
}
