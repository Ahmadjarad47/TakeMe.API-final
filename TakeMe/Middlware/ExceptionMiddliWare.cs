using System.Net;
using System.Text.Json;
using TakeMe.Error;

namespace TakeMe.Middlware
{
    public class ExceptionMiddliWare
    {
        private readonly RequestDelegate request;
        private readonly ILogger<ExceptionMiddliWare> logger;
        private readonly IHostEnvironment environment;

        public ExceptionMiddliWare(RequestDelegate request, ILogger<ExceptionMiddliWare> logger, IHostEnvironment environment)
        {
            this.request = request;
            this.logger = logger;
            this.environment = environment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await request(context);
                logger.LogInformation("Success");
            }
            catch (System.Exception ex)
            {
                logger.LogError($"this exception come form Middle Ware{ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var resp = environment.IsDevelopment() ?
                    new APIException((int)HttpStatusCode.InternalServerError,
                    ex.Message, ex.StackTrace.ToString()) :
                    new APIException((int)HttpStatusCode.InternalServerError);
                ;
                var op = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var Json = JsonSerializer.Serialize(resp, op);
                await context.Response.WriteAsync(Json);
            }
        }
    }

}
