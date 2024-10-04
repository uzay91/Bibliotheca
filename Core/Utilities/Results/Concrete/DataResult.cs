using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Concrete
{

    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T data, bool success, string message, int status) : base(success, message, status)
        {
            Data = data;
        }
        public DataResult(T data, bool success, string message) : base(success, message)
        {
            Data = data;
        }
        public DataResult(T data, bool success, int status) : base(success, status)
        {
            Data = data;
        }
        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }
        public T Data { get; }

    }
}
