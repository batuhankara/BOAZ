using BAOZ.Api.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAOZ.Api.Extensions
{
    public static class WebApiMiddlewares
    {
        public static IApplicationBuilder AddWebApiMiddlewares(this IApplicationBuilder builder)
        {
            return builder.UseRequestResponseLogging();
        }

        private static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}


