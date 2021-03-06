﻿using Microsoft.AspNetCore.Http;
using Sentry;
using Sentry.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAOZ.Api.Sentry
{
    public class EventProcessor : ISentryEventProcessor
    {
        private readonly IHttpContextAccessor _httpContext;

        public EventProcessor(IHttpContextAccessor httpContext) => _httpContext = httpContext;

        public SentryEvent Process(SentryEvent @event)
        {
            //Here I can modify the event, while taking dependencies via DI

           @event.SetExtra("Response:HasStarted", _httpContext.HttpContext.Response.HasStarted);
            return @event;
        }
    }
}
