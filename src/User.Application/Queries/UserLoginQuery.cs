using BAOZ.DDD.CQRS;
using User.Application.Dtos;

namespace User.Application.Queries
{
    public class UserLoginQuery : IBaseQuery<AuthTokenDto>
    {
        public UserLoginQuery(string fullPhoneNumber, string password)
        {
            FullPhoneNumber = fullPhoneNumber;
            Password = password;
        }

        public string FullPhoneNumber { get; set; }
        public string Password { get; set; }

    }
}
