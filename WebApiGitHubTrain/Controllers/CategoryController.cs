using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiGitHubTrain.Models;
using WebApiGitHubTrain.Repositories;

namespace WebApiGitHubTrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory cateRepo;

        public CategoryController(ICategory _cateRepo)
        {
            cateRepo = _cateRepo;
        }
        [Authorize]
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await cateRepo.GetAllCategories());
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await cateRepo.GetCategoryById(id));
        }
        [Authorize]
        [HttpGet("{name:alpha}")]
        public ActionResult GetByName([FromRoute] string name)
        {
            return Ok(cateRepo.GetCategoryByName(name));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Category model)
        {
            if (ModelState.IsValid)
            {
                await cateRepo.Add_Category(model);
                return Ok(model);
            }
            return BadRequest(ModelState);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, Category model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
            {
                await cateRepo.Update_Category(id, model);
                return Ok(model);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> delete([FromRoute] int id)
        {
            await cateRepo.Delete_Category(id);
            return Ok();
        }

    }
}
