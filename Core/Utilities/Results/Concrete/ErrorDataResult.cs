using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Concrete
{
    public class ErrorDataResult<T> : DataResult<T>, IErrorResult
    {

        public ErrorDataResult(int status, string message, string errorCode) : base(default, false, message, status)
        {
            Status = status;
            Message = message;
            ErrorCode = errorCode;
        }
        public ErrorDataResult(int status, string message) : base(default, false, message, status)
        {
            Status = status;
            Message = message;

        }

        public ErrorDataResult(string message) : base(default, false, message)
        {
            Message = message;

        }
        public ErrorDataResult(int status) : base(default, false, status)
        {
            Status = status;
        }
        public ErrorDataResult() : base(default, false)
        {

        }
        public int Status { get; }
        public string Message { get; }
        public string ErrorCode { get; }


    }
}

