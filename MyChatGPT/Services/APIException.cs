using System;

namespace MyChatGPT.Services
{
    public class APIException : Exception
    {
        public string StatusCode { get; set; }

        public string Content { get; set; }
    }
}

