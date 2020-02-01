using EventFlow.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using User.Application.Dtos;
using User.Application.Queries;

namespace User.Application.Modules
{
    public interface IUserQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>,
        IQueryHandler<UserLoginQuery, AuthTokenDto>
    {
    }
}
