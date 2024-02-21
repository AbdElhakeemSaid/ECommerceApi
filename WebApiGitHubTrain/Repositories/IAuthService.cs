using Microsoft.AspNetCore.Identity;
using WebApiGitHubTrain.Dto;

namespace WebApiGitHubTrain.Repositories
{
    public interface IAuthService
    {
        public Task<AuthModelDto> RegisterAsync(RegisterModelDto model);
        public Task<AuthModelDto> LoginAsync(LoginModelDto model);
        public Task<string> AddtoRole(AddToRoleModelDto model);
        public Task<List<string>> GetAllRoles();
        public Task<IdentityRole> GetRoleByName(string name);
        public Task<IdentityRole> AddRole(string model);
        public Task<bool> Delete(string name);
        public Task<bool> UserIsExist(string id);
        public Task<string> GetUserName(string id);
    }
}
