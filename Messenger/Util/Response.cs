using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Util
{
    public class Response<T> 
    {
        public bool Success { get; protected set; }
        public string Message { get; protected set; }
        public T Data { get; protected set; }

        private Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        private Response(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public Response(T data) : this(true, string.Empty, data)
        { }

        public Response(string message) : this(false, message)
        { }
    }
}
