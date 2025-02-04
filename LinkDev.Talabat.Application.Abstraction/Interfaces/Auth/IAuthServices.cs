using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Auth;
using System.Security.Claims;

namespace LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Auth
{
    public interface IAuthServices
    {

        Task<UserDto> LoginAsync(LoginDto model);
        Task<UserDto> RegisterAsync(RegisterDto model);
        Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal);
    }
}
