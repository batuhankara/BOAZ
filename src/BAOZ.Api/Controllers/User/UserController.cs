

using BAOZ.Api.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using User.Application.Queries;
using User.Core.Domain.Commands;
using User.Core.Domain.Repositories;

namespace BAOZ.Api.Controllers.User
{
    [Route("api/user")]

    public class UserController : BaseController
    {

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]


        [Route("register")]
        public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
        {
            var result = await CommandBus.PublishAsync(command, CancellationToken.None).ConfigureAwait(false);
            if (result.IsSuccess)
            {
                return Created(string.Empty, command);
            }
            return BadRequest("oluşturualmadı");
        }
        [HttpPut("")]
        public async Task<IActionResult> Put([FromBody]UpdateUserCommand command)
        {
            var result = await CommandBus.PublishAsync(command, CancellationToken.None).ConfigureAwait(false);
            if (result.IsSuccess)
            {
                return Created(string.Empty, command);
            }
            return BadRequest("oluşturualmadı");
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await QueryProcessor.ProcessAsync(query, CancellationToken.None);
            return Ok(result);
        }

    }
}
