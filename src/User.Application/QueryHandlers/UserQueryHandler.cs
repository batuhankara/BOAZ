using EventFlow.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using User.Application.Dtos;
using User.Application.Queries;
using User.Core.Domain.Entities;
using User.Core.Domain.Repositories;

namespace User.Application.QueryHandlers
{
    public class UserQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        public UserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDto> ExecuteQueryAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(x => x.Id == query.Id);
            return new UserDto(user.Id, user.FirstName);
        }
    }
}
