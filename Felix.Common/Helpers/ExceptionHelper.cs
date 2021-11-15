using Felix.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Felix.Common.Helpers
{
    public class ExceptionHelper
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;


        public ExceptionHelper(RequestDelegate next)
        {
            _next = next;
            _logger = LogHelper.GetApplicationLogger<ExceptionHelper>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path != "/" && !context.IsSwagger() && !context.IsHangfire())
            {
                var startTime = DateTime.Now;
                var requestJson = "";
                

                try
                {
                    requestJson = await context.Request.ReadRequestBody();
                    await _next(context);

                    //Logla 
                }
                catch (Exception ex)
                {
                    // _logger.Write("adsfdsaf");
                    await HandleException(context, ex);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            var result = JsonSerializer.Serialize(EnvelopeHelper.ToEnvelope("", Enums.ResponseEnum.InternalError, false, ex.Message));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)Enums.ResponseEnum.InternalError;
            return context.Response.WriteAsync(result);
        }
    }
}
