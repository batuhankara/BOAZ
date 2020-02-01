using BAOZ.Common.Helpers;
using EventFlow.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using User.Application.Dtos;
using User.Application.Modules;
using User.Application.Queries;
using User.Core.Domain.Entities;
using User.Core.Domain.Repositories;

namespace User.Application.QueryHandlers
{
    public class UserQueryHandler : IUserQueryHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        public UserQueryHandler(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }
        public async Task<UserDto> ExecuteQueryAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(x => x.Id == query.Id);
            return new UserDto(user.Id, user.FirstName);
        }
        public async Task<AuthTokenDto> ExecuteQueryAsync(UserLoginQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(x => x.FullPhoneNumber == query.FullPhoneNumber);

            if (user == null)
            {
                return null;
            }

            if (!_passwordService.VerifyPassword(query.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return new AuthTokenDto("Token üretildi", "tip");

        }
    }
}
