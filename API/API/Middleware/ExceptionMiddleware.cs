using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;


#region previous Code

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment evn)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = evn.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}

#endregion

#region new Code not approved
//public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
//{
//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await next(context);
//        }
//        catch (Exception ex)
//        {
//            logger.LogError(ex, ex.Message);

//            context.Response.ContentType = "application/json";
//            context.Response.StatusCode = ex switch
//            {
//                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
//                DbUpdateConcurrencyException => (int)HttpStatusCode.Conflict,
//                _ => (int)HttpStatusCode.InternalServerError
//            };

//            var response = env.IsDevelopment()
//                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
//                : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

//            var options = new JsonSerializerOptions
//            {
//                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//            };

//            var json = JsonSerializer.Serialize(response, options);
//            await context.Response.WriteAsync(json);
//        }
//    }
//}

#endregion




