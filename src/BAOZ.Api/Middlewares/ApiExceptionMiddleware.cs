using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using BAOZ.Api.Models;
using System;

namespace BAOZ.Api.Middlewares
{
    public class ResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseWrapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            Stopwatch stopWatch = Stopwatch.StartNew();
            Stream originalBody = context.Response.Body;

            try
            {
                if (!context.Request.Path.Value.Contains("api"))
                {
                    await _next(context);
                    return;
                }

                var originBody = context.Response.Body;

                var newBody = new MemoryStream();

                context.Response.Body = newBody;

                await _next(context);

                newBody.Seek(0, SeekOrigin.Begin);

                string json = await new StreamReader(newBody).ReadToEndAsync();

                var modifiedJson = JsonConvert.DeserializeObject<ApiResponse>(json);
                modifiedJson.ResponseTime = stopWatch.ElapsedMilliseconds.ToString();
                context.Response.Body = originBody;
                newBody.Seek(0, SeekOrigin.Begin);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(modifiedJson));
                newBody.Seek(0, SeekOrigin.Begin);


            }

            catch (Exception e)
            {

            }


        }
    }

}
