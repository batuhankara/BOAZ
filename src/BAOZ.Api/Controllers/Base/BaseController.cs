using EventFlow;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.DependencyInjection;
using EventFlow.Queries;

namespace BAOZ.Api.Controllers.Base
{
    public abstract class BaseController : ControllerBase
    {
        private Lazy<ICommandBus> _commandBus => HttpContext.RequestServices.GetService<Lazy<ICommandBus>>();
        private Lazy<IQueryProcessor> _queryProcessor => HttpContext.RequestServices.GetService<Lazy<IQueryProcessor>>();

        public ICommandBus CommandBus { get { return _commandBus.Value; } }
        public IQueryProcessor QueryProcessor { get { return _queryProcessor.Value; } }
    }
}
