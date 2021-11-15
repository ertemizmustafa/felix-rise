using Felix.Common.Extensions;
using Felix.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Gateway.Middleware
{
    public class GatewayRequestHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public GatewayRequestHandler(RequestDelegate next)
        {
            _next = next;
            _logger = LogHelper.GetApplicationLogger<GatewayRequestHandler>();
            #if DEBUG
            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
            #endif
        }


        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path != "/" && !context.IsSwagger())
            {
                var startTime = DateTime.Now;
                var requestJson = "";
                var responseJson = "";


                try
                {

                    var originalBody = context.Response.Body;
                    await using var newBody = new MemoryStream();
                    context.Response.Body = newBody;
                    requestJson = await context.Request.ReadRequestBody();
                    await _next(context);

                    newBody.Seek(0, SeekOrigin.Begin);
                    var bodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    newBody.Seek(0, SeekOrigin.Begin);
                    await newBody.CopyToAsync(originalBody);
                    responseJson = bodyText?.Trim()?.Substring(0, 1000) ?? "";

                    //Logla
                }
                catch (Exception ex)
                {
                    //Logla
                    // _logger.Write("adsfdsaf");

                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
