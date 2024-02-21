using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiGitHubTrain.Dto;
using WebApiGitHubTrain.Models;
using WebApiGitHubTrain.Repositories;

namespace WebApiGitHubTrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {


        private readonly IProduct prodRepo;

        public ProductController(IProduct _prodRepo)
        {
            prodRepo = _prodRepo;
        }
        [HttpGet("GetAll")]
        [Authorize]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await prodRepo.GetAllProducts());
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await prodRepo.GetProductById(id));
        }
        [Authorize]
        [HttpGet("{name:alpha}")]
        public ActionResult GetByName([FromRoute] string name)
        {
            return Ok(prodRepo.GetProductByName(name));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] AddProductDto model)
        {
            if (ModelState.IsValid)
            {
                Product returned = await prodRepo.Add_Product(model);
                //model.Id = returned.Id;
                return Ok(returned);
            }
            return BadRequest(ModelState);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, Product model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
            {
                Product returned = await prodRepo.Update_Product(id, model);
                returned.Id = id;
                return Ok(returned);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> delete([FromRoute] int id)
        {
            await prodRepo.Delete_Product(id);
            return Ok();
        }

    }
}

