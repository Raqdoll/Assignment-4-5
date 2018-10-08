using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Assignment2
{
    public class AuthMiddleware
    {
        
        RequestDelegate _next;
        IConfiguration _configuration;
        public AuthMiddleware(RequestDelegate next, IConfiguration configuration){
            _next = next;
            _configuration = configuration;
        }
        public async Task Invoke(HttpContext context){
            var apiKeyValue = _configuration["ApiKey"];
            var userApiKeyValue = context.Request.Headers["x-api-key"];

            if(userApiKeyValue == ""){
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Bad request");
            }

            if(userApiKeyValue == apiKeyValue){
                await _next.Invoke(context);
            }else{
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Forbidden");
            }
        }
    }
}