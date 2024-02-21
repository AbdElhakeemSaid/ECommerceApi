using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiGitHubTrain.Dto;
using WebApiGitHubTrain.Helper;
using WebApiGitHubTrain.Models;

namespace WebApiGitHubTrain.Repositories
{
    public class AuthRepo : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly JWT jwt;

        public AuthRepo(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, IOptions<JWT> _jwt)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            jwt = _jwt.Value;
        }


        public async Task<AuthModelDto> RegisterAsync(RegisterModelDto model)
        {
            if (await userManager.FindByEmailAsync(model.Email) != null)
            {
                return new AuthModelDto { Message = "Email is already Exist" };
            }
            if (await userManager.FindByNameAsync(model.UserName) != null)
            {
                return new AuthModelDto { Message = "UserName is already Exist" };
            }
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await userManager.CreateAsync(user, model.password);
            if (!result.Succeeded)
            {
                string errorMessage = string.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessage += $"{error.Description} ,";
                }
                return new AuthModelDto { Message = errorMessage };
            }
            await userManager.AddToRoleAsync(user, "User");
            var JwtToken = await CreateJwtToken(user);
            return new AuthModelDto
            {
                Email = user.Email,
                ExpireOn = JwtToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                UserName = user.UserName
            };
        }

        public async Task<AuthModelDto> LoginAsync(LoginModelDto model)
        {
            var authModel = new AuthModelDto();
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or password are wrong";
                return authModel;
            }
            var JwtToken = await CreateJwtToken(user);
            var rolesList = await userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(JwtToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.ExpireOn = JwtToken.ValidTo;
            authModel.Roles = rolesList.ToList();
            return authModel;

        }
        public async Task<string> AddtoRole(AddToRoleModelDto model)
        {
            var user = await userManager.FindByIdAsync(model.user_Id);
            if (user is null || !await roleManager.RoleExistsAsync(model.Role))
                return "id or Role is invalid";
            if (await userManager.IsInRoleAsync(user, model.Role))
                return $"user {model.user_Id} is already {model.Role}";
            var result = await userManager.AddToRoleAsync(user, model.Role);
            return result.Succeeded ? string.Empty : "failed";
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid",user.Id)
            }.Union(userClaims).Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
            audience: jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<List<string>> GetAllRoles()
        {
            return await roleManager.Roles.Select(r => r.Name).ToListAsync();
        }

        public async Task<IdentityRole> GetRoleByName(string name)
        {
            IdentityRole role = await roleManager.FindByNameAsync(name);
            if (role == null)

                return null;
            else
                return role;
        }

        public async Task<bool> Delete(string name)
        {
            IdentityRole role = await roleManager.FindByNameAsync(name);
            if (role == null)

                return false;
            else
            {
                if (!(await userManager.GetUsersInRoleAsync(name)).Any())
                {
                    await roleManager.DeleteAsync(role);
                    return true;
                }
                return false;
            }
        }

        public async Task<IdentityRole> AddRole(string modelName)
        {
            if (await roleManager.FindByNameAsync(modelName) == null)
            {

                IdentityRole model = new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = modelName,
                    NormalizedName = modelName.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                var result = await roleManager.CreateAsync(model);
                if (result.Succeeded)
                {
                    return (model);
                }

            }
            return null;
        }

        public async Task<bool> UserIsExist(string id)
        {
            if (await userManager.FindByIdAsync(id) is null)
                return false;
            return true;
        }

        public async Task<string> GetUserName(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user is not null)
                return $"{user.FirstName} {user.LastName}";
            else
                return string.Empty;
        }
    }
}
