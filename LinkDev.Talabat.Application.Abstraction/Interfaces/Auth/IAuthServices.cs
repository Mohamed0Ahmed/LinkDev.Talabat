using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Common;
using System.Security.Claims;

namespace LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Auth
{
    public interface IAuthServices
    {

        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal);
        Task<AddressDto?> GetUserAddress(ClaimsPrincipal claimsPrincipal);

        Task<AddressDto> UpdateUserAddress(ClaimsPrincipal principal, AddressDto addressDto);

        Task<bool> EmailExists(string email);

    }
}
