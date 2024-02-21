using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiGitHubTrain.Dto;
using WebApiGitHubTrain.Repositories;

namespace WebApiGitHubTrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcountController : ControllerBase
    {
        private readonly IAuthService authRepo;

        public AcountController(IAuthService _authRepo)
        {
            authRepo = _authRepo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] RegisterModelDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authRepo.RegisterAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authRepo.LoginAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRole([FromBody] AddToRoleModelDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await authRepo.AddtoRole(model);
            if (result != null)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("Get Roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            List<string> roles = await authRepo.GetAllRoles();
            if (roles is not null)
                return Ok(roles);
            else
                return BadRequest("there is no roles assigned");
        }
        [HttpGet("Get Role{name:alpha}")]
        public async Task<IActionResult> GetRoleByName(string name)
        {
            IdentityRole role = await authRepo.GetRoleByName(name);
            if (role is not null)
                return Ok(role);
            else
                return BadRequest($"there is no role assigned with this name : {name}");
        }
        [HttpDelete("Delete Role{name:alpha}")]
        public async Task<IActionResult> Delete(string name)
        {
            if (await authRepo.Delete(name))
                return Ok("role Deleted Successfully");
            else
                return BadRequest($"there is no role assigned with this id : {name} or there is users withe this role");
        }
        [HttpPost("add Role{name:alpha}")]
        public async Task<IActionResult> AddRole(string name)
        {
            if (await authRepo.GetRoleByName(name) is null)
            {
                if (ModelState.IsValid)
                {
                    IdentityRole role = await authRepo.AddRole(name);
                    return Ok(role);
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            else
            {
                return BadRequest("this role is exist");
            }
        }

    }
}
