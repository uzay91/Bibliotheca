using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Concrete
{
    public class ErrorResult : Result, IErrorResult
    {
        public ErrorResult(int status, string message, string errorCode) : base(false, message)
        {
            Status = status;
            ErrorCode = errorCode;
        }
        public ErrorResult(int status, string message) : base(false, message)
        {
            Status = status;
        }

        public ErrorResult(string message) : base(false, message)
        {

        }
        public ErrorResult(int status) : base(false, status)
        {
            Status = status;

        }
        public ErrorResult() : base(false)
        {

        }
        public int Status { get; }
        public string Message { get; }
        public string ErrorCode { get; }

    }
}
