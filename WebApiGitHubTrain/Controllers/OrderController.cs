using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiGitHubTrain.Dto;
using WebApiGitHubTrain.Repositories;

namespace WebApiGitHubTrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder orderRepo;

        public OrderController(IOrder _orderRepo)
        {
            orderRepo = _orderRepo;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            if (await orderRepo.GetAllOrders() is null)

                return BadRequest("there is no orders");

            else
                return Ok(await orderRepo.GetAllOrders());
        }
        [Authorize]
        [HttpGet("GetOrderDetails{id:int}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            if (ModelState.IsValid)
                if (await orderRepo.GetOrderDetails(id) is null)
                {
                    return BadRequest($"there is no oder with this id :{id}");
                }
                else
                    return Ok(await orderRepo.GetOrderDetails(id));
            return BadRequest(ModelState);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MakeOrder(MakeOrderDto model)
        {
            if (ModelState.IsValid)
            {
                OrderDetailsDto detail = await orderRepo.MakeOrder(model);
                return Ok(detail);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("AddToOrder{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AddToOrder([FromRoute] int id, [FromBody] MakeOrderDto model)
        {
            if (ModelState.IsValid)
            {
                OrderDetailsDto detail = await orderRepo.AddToOrder(id, model);
                return Ok(detail);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("UpdateProductInOrder{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] ProductInOrder model)
        {
            if (ModelState.IsValid)
            {
                OrderDetailsDto detail = await orderRepo.UpdateOrder(id, model);
                return Ok(detail);
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteProduct{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteItemInOrder([FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                OrderDetailsDto detail = await orderRepo.DeleteItemInOrder(id);
                return Ok(detail);
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteOrder{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                bool result = await orderRepo.DeleteOrder(id);
                if (result)
                    return Ok("Order Deleted successfully");
                return BadRequest("failed \n order Not Found");
            }
            return BadRequest(ModelState);
        }
    }
}
