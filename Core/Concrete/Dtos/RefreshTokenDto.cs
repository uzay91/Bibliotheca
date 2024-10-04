using Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.Dtos
{
    public class RefreshTokenDto :IDto
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
