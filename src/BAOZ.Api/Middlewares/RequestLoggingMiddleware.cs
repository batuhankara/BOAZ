using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IO;
using System.IO;
using System.Text;
using BAOZ.Api.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace BAOZ.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }
        public async Task Invoke(HttpContext context)
        {
            if (IsShouldLogRequest(context.Request.Path))
            {
                Stopwatch stopWatch = Stopwatch.StartNew();
                context.Request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
                await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                var requestBody = Encoding.UTF8.GetString(buffer);
                context.Request.Body.Seek(0, SeekOrigin.Begin);


                var requestModel = new RequestLogModel()
                {
                    Body = requestBody,
                    Url = context.Request.Path,
                    Headers = GetHeaders(context.Request.Headers)

                };
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await _next(context);

                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    await responseBody.CopyToAsync(originalBodyStream);

                    var responseModel = new ResponseLogModel()
                    {
                        Body = response,
                        Headers = GetHeaders(context.Response.Headers)
                    };
                    stopWatch.Stop();
                    var requestResponseModel = new RequestResponseLogModel()
                    {
                        ResponseTime = stopWatch.ElapsedMilliseconds.ToString(),
                        RequestModel = requestModel,
                        ResponseModel = responseModel
                    };
                    _logger.LogInformation(JsonConvert.SerializeObject(requestResponseModel));

                }
            }
            else
            {
                await _next(context);
            }

        }


        private List<KeyValuePair<string, string>> GetHeaders(IHeaderDictionary headers)
        {
            return headers.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList();
        }

        private static bool IsShouldLogRequest(string uri)
        {
            string[] notAllowedUrls = new string[] { "swagger" };
            return !notAllowedUrls.Any(x => uri.Contains(x));

        }
    }

}
