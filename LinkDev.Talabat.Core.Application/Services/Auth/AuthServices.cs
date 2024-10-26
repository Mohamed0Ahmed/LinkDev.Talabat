using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Auth;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinkDev.Talabat.Core.Application.Services.Auth
{
    public class AuthServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAuthServices
    {


        public async Task<UserDto> LoginAsync(LoginDto model)
        {

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
                throw new UnauthorizedException("Invalid Login ..");

            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);

            if (result.IsNotAllowed)
                throw new UnauthorizedException("Account Not confirmed yet ..");

            if (result.IsLockedOut)
                throw new UnauthorizedException("Account Is Locked ..");

            //if (result.RequiresTwoFactor)
            //    throw new UnAuthorizedException("Require Two-Factor Authentication ..");

            if (!result.Succeeded)
                throw new UnauthorizedException("Invalid Login ..");

            var response = new UserDto
            {

                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user),
            };


            return response;
        }



        public async Task<UserDto> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                throw new ValidationException() { Errors = result.Errors.Select(E => E.Description) };
            var response = new UserDto
            {

                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user),
            };

            return response;

        }


        public async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var rolesAsClaims = new List<Claim>();


            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                rolesAsClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }


            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.PrimarySid , user.Id),
                new Claim(ClaimTypes.Email , user.Email!),
                new Claim(ClaimTypes.GivenName , user.DisplayName),

            }.Union(userClaims).Union(rolesAsClaims);



            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-256-bit-secretsdfsdfsdfds-gfdgdfg"));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokenObj = new JwtSecurityToken(

                issuer: "TalabatIdentity",
                audience: "TalabatUsers",
                expires: DateTime.UtcNow.AddMinutes(10),
                claims:claims,
                signingCredentials: signingCredentials
                

                );

            return new JwtSecurityTokenHandler().WriteToken(tokenObj);
        }

    }
}
