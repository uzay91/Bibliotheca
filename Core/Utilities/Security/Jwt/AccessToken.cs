using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Jwt
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime ExpiresIn { get; set; }
        public int Expire { get; set; } //bunu seconds olarak verilecek 60*dakika
        public string RefreshToken { get; set; }
        public Guid UserId { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }

    }
}
