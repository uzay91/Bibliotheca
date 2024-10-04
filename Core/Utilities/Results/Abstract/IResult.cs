using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Abstract
{
    public interface IResult
    {
        bool Success { get;  }
        string Message { get;  }
        int Status { get; } //http status kodları örneğin 200OK, 400BadRequest
    }
}
