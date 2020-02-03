using BAOZ.Api.Middlewares;
using BAOZ.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
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
            return builder
        .UseMiddleware<ResponseWrapperMiddleware>();
        }
    }
}


