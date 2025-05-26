using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.HandleResponses
{
    public class Response
    {
        public Response(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetStatusDefaultMessage(statusCode);
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }

        private string GetStatusDefaultMessage(int statusCode) 
            => statusCode switch
            {
                >= 100 and < 200 => "Informational response",
                200 => "OK",
                201 => "Created",
                202 => "Accepted",
                204 => "No Content",
                >= 200 and < 300 => "Success",
                301 => "Moved Permanently",
                302 => "Found",
                304 => "Not Modified",
                >= 300 and < 400 => "Redirection",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                405 => "Method Not Allowed",
                409 => "Conflict",
                422 => "Unprocessable Entity",
                >= 400 and < 500 => "Client Error",
                500 => "Internal Server Error",
                501 => "Not Implemented",
                502 => "Bad Gateway",
                503 => "Service Unavailable",
                504 => "Gateway Timeout",
                505 => "HTTP Version Not Supported",
                >= 500 and < 600 => "Server Error",
                _ => "Unknown Status Code" // default
            };

    }
}
