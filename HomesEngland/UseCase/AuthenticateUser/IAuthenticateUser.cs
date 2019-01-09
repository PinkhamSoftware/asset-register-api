using HomesEngland.Boundary.UseCase;
using HomesEngland.UseCase.AuthenticateUser.Models;

namespace HomesEngland.UseCase.AuthenticateUser
{
    public interface IAuthenticateUser : IAsyncUseCaseTask<AuthenticateUserRequest, AuthenticateUserResponse>
    {
    }
}
