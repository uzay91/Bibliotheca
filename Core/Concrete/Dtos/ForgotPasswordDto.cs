using Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.Dtos
{
    public class ForgotPasswordDto : IDto
    {
        public string ForgotUrl { get; set; }
    }
}
