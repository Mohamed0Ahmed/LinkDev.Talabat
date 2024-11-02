using LinkDev.Talabat.Application.Abstraction.Interfaces;
using System.Security.Claims;

namespace LinkDev.Talabat.APIs.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string UserId { get; private set; }


        public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            // Use ClaimTypes.NameIdentifier to retrieve the UserId claim
            UserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }


    }
}
