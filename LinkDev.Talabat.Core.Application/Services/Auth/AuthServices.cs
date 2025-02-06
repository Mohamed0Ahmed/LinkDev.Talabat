using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Common;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces.Auth;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Application.Extensions;
using LinkDev.Talabat.Core.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinkDev.Talabat.Core.Application.Services.Auth
{
    public class AuthServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings, IMapper mapper) : IAuthServices
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {

            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                throw new UnauthorizedException("Invalid Login ..");

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

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



        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {

            var user = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

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



            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokenObj = new JwtSecurityToken(

                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                claims: claims,
                signingCredentials: signingCredentials


                );

            return new JwtSecurityTokenHandler().WriteToken(tokenObj);
        }

        public async Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var user = await userManager.FindUserWithAddress(claimsPrincipal);

            return new UserDto()
            {
                DisplayName = user!.DisplayName,
                Email = user.Email!,
                Id = user.Id,
                Token = await GenerateTokenAsync(user),
            };
        }

        public async Task<AddressDto?> GetUserAddress(ClaimsPrincipal claimsPrincipal)
        {

            var user = await userManager.FindUserWithAddress(claimsPrincipal);

            var address = mapper.Map<AddressDto>(user!.Address);
            return address;

        }

        public async Task<AddressDto> UpdateUserAddress(ClaimsPrincipal claims, AddressDto addressDto)
        {

            var user = await userManager.FindUserWithAddress(claims);
            var updatedAddress = mapper.Map<Address>(addressDto);


            if (user!.Address is not null)
            {
                updatedAddress.Id = user.Address.Id;
                user.Address.FirstName = updatedAddress.FirstName;
                user.Address.LastName = updatedAddress.LastName;
                user.Address.Street = updatedAddress.Street;
                user.Address.City = updatedAddress.City;
                user.Address.Country = updatedAddress.Country;
            }
            else
            {
                user.Address = updatedAddress;
            }

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new BadRequestException(
                    result.Errors.Select(error => error.Description)
                                 .Aggregate((x, y) => $"{x}, {y}")
                );

            return mapper.Map<AddressDto>(user.Address);
        }

        public async Task<bool> EmailExists(string email)
        {

            return await userManager.FindByEmailAsync(email!) is not null;
        }
    }
}
