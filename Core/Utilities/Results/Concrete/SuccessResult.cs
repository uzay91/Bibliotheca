using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Concrete
{
    public class SuccessResult : Result
    {
        public SuccessResult(string message, int status) : base(true, message, status)
        {

        }
        public SuccessResult(string message) : base(true, message)
        {

        }
        public SuccessResult(int status) : base(true, status)
        {


        }
        public SuccessResult() : base(true)
        {

        }
    }
}
