using journeyMate.Api.Errors;
using System.Net;
using System.Text.Json;

namespace journeyMate.Api.MiddelWears
{
    public class ServerMiddelWear
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ServerMiddelWear> logger;
        private readonly IHostEnvironment env;

        public ServerMiddelWear(RequestDelegate next, ILogger<ServerMiddelWear> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,ex.Message);

                //edit head of request 
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode=(int) HttpStatusCode.InternalServerError;

                //edit body of request
                var serverResponse = env.IsDevelopment() ?
                    new ServerErrorResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ServerErrorResponse((int )(HttpStatusCode.InternalServerError), ex.Message);
                var option=new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json=JsonSerializer.Serialize(serverResponse, option);
                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}
