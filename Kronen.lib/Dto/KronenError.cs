using System;
using System.Net;

namespace Kronen.lib.Dto
{
    public class KronenError
    {
        public string message {get;set;}
        public string exceptionMessage {get;set;}
        public HttpStatusCode statusCode {get;set;}
        public Enum EnumError {get;set;}
    }
}