using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiGitHubTrain.Models;
using WebApiGitHubTrain.Repositories;

namespace WebApiGitHubTrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrand brandRepo;

        public BrandController(IBrand _brandRepo)
        {
            brandRepo = _brandRepo;
        }
        [HttpGet("GetAll")]
        [Authorize]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await brandRepo.GetAllBrands());
        }
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await brandRepo.GetBrandById(id));
        }
        [Authorize]
        [HttpGet("{name:alpha}")]
        public ActionResult GetByName([FromRoute] string name)
        {
            return Ok(brandRepo.GetBrandByName(name));
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Brand model)
        {
            if (ModelState.IsValid)
            {
                await brandRepo.Add_Brand(model);
                return Ok(model);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update([FromRoute] int id, Brand model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
            {
                await brandRepo.Update_Brand(id, model);
                return Ok(model);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> delete([FromRoute] int id)
        {
            await brandRepo.Delete_Brand(id);
            return Ok();
        }

    }
}
