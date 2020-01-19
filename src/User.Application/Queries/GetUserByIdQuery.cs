using BAOZ.DDD.CQRS;
using System;
using User.Application.Dtos;
using User.Core.Domain.Entities;

namespace User.Application.Queries
{
    public class GetUserByIdQuery : IBaseQuery<UserDto>
    {
        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }
}
