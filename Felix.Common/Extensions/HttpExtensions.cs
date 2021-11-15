using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Felix.Common.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<string> ReadRequestBody(this HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);
            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            string requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $"TraceIdentifier: {request.HttpContext?.TraceIdentifier ?? ""} " +
                    $"Scheme: {request.Scheme} " +
                    $"Host: {request.Host} " +
                    $"Path: {request.Path} " +
                    $"Method: {request.Method} " +
                    $"QueryString: {request.QueryString} " +
                    $"Body: {requestBody} ";
        }

        public static async Task<string> ReadResponseBody(this HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return $"{responseBody}";
        }

        public static bool IsSwagger(this HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");
        }

        public static bool IsHangfire(this HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/Hangfire");
        }
    }
}
