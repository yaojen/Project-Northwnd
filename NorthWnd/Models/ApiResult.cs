using System;
using System.Linq;
using System.Web;

namespace NorthWnd.Models
{
    public class ApiResult<T>
    {
        public bool Succ { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        public DateTime DateTime { get; set; }

        public T Data { get; set; }

        public ApiResult()
        {
        }

        public ApiResult(T data)
        {
            Code = "0000";
            Succ = true;
            this.DateTime = DateTime.Now;
            Data = data;
        }
    }

    public class ApiError : ApiResult<object>
    {

        public ApiError(string code, string message)
        {
            Code = code;
            Succ = false;
            this.DateTime = DateTime.Now;
            Message = message;
        }

    }
}