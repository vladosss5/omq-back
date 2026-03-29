using AskAgainApi.Exceptions;

namespace AskAgainApi.Helpers
{
    public class MiddlewareExceptions
    {
        private readonly RequestDelegate _next;

        public MiddlewareExceptions(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (HttpException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, HttpException httpException)
        {
            var code = httpException.Code;
            var result = string.Empty;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;

            if (result == string.Empty)
            {
                result = System.Text.Json.JsonSerializer.Serialize(new { error = httpException.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }

}

