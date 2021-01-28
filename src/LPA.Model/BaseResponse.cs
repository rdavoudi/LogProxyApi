using System;

namespace LPA.Model
{
    public abstract class BaseResponse
    {
        public int Code { get; protected set; }

        public string Message { get; protected set; }

        public BaseResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
