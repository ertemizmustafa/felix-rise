using Felix.Common.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Common.Helpers
{
    public static class ApplicationContext
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext Current => _httpContextAccessor.HttpContext;

        public static string TransactionId => _httpContextAccessor?.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();

        public static string UserName => _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? Environment.UserDomainName + "//" + Environment.UserName;

        public static int UserId => _httpContextAccessor?.HttpContext?.User?.Identity?.Name.TryConvertTo<int>() ?? 0;

        public static string ClientIp => _httpContextAccessor?.HttpContext?.Request?.Headers["Client-IP"].ToString() ?? "Unknown"; 
    }
}
