﻿using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Concrete
{
    public class Result : IResult
    {
        public Result(bool success, string message, int status) : this(success, message)
        {
            this.Status = status;
        }
        public Result(bool success, int status) : this(success)
        {
            this.Status = status;
        }
        public Result(bool success, string message) : this(success)
        {
            this.Message = message;            
        }
        public Result(bool success)
        {
            this.Success = success;
        }
        public bool Success { get; }
        public string Message { get; }
        public int Status { get; }
    }
}
