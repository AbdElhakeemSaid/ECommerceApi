using Microsoft.EntityFrameworkCore;
using WebApiGitHubTrain.Dto;
using WebApiGitHubTrain.Models;

namespace WebApiGitHubTrain.Repositories
{
    public class OrderRepo : IOrder
    {
        private readonly DbAppContext context;
        private readonly IAuthService authService;
        static string? messageOfOrderProduct;

        public OrderRepo(DbAppContext _context, IAuthService authService)
        {
            context = _context;
            this.authService = authService;

        }
        public async Task<List<Order>> GetAllOrders()
        {
            List<Order> orders = await context.Orders.ToListAsync();
            if (orders.Count <= 0)
                return null;
            return orders;
        }

        public async Task<OrderDetailsDto> GetOrderDetails(int id)
        {
            Order? order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            OrderItem? items = await context.OrderItems.FirstOrDefaultAsync(item => item.Order_Id == id);
            if (order is null || items is null)
                return null;
            string CustomerName = await authService.GetUserName(order.custmer_Id);

            OrderDetailsDto details = new OrderDetailsDto();
            details.OrderDate = order.OrderDate;
            details.CustomerName = CustomerName;
            details.Price = await context.OrderItems.Where(o => o.Order_Id == id).SumAsync(pr => pr.Price);
            details.products =
                await context.OrderItems.Where(o => o.Order_Id == id)
                .GroupBy(g => g.product.Name)
                .Select(s => new { name = s.Key, quantity = s.Sum(p => p.Quantity) })
                .ToDictionaryAsync(d => d.name, q => q.quantity);

            details.Message = "You can pay cach or by card";
            return details;
        }

        public async Task<OrderDetailsDto> MakeOrder(MakeOrderDto model)
        {
            if (!await authService.UserIsExist(model.CustomerId))
            {
                return new OrderDetailsDto { Message = "please Login if you Have ِAccount" };
            }

            Order order = new Order()
            {
                OrderDate = DateTime.Now,
                custmer_Id = model.CustomerId
            };

            if (!await AddListOfProducts(model.Products, order))
                return new OrderDetailsDto { Message = messageOfOrderProduct };

            OrderDetailsDto detail = await GetOrderDetails(order.Id);
            detail.Message = "You can pay cach or by card";
            return detail;
        }
        public async Task<OrderDetailsDto> AddToOrder(int id, MakeOrderDto model)
        {
            Order? order = await context.Orders.SingleOrDefaultAsync(o => o.Id == id);
            if (order is not null)
            {
                order.OrderDate = DateTime.Now;
                OrderItem item = new OrderItem()
                {
                    Product_Id = (await context.Products.FirstOrDefaultAsync(p => p.Name == model.Products[0].ProductName)).Id,
                    Order_Id = order.Id,
                    Quantity = model.Products[0].Quantity,
                };
                item.Price = (await context.Products.FirstOrDefaultAsync(p => p.Name == model.Products[0].ProductName)).Price * item.Quantity;
                await context.OrderItems.AddAsync(item);
                await context.SaveChangesAsync();
                OrderDetailsDto detail = await GetOrderDetails(order.Id);
                detail.Message = "You can pay cach or by card";
                return detail;
            }

            return new OrderDetailsDto { Message = "there no order with this id" };
        }
        public async Task<OrderDetailsDto> UpdateOrder(int id, ProductInOrder model)
        {
            OrderItem? orderItem = await context.OrderItems.FirstOrDefaultAsync(oi => oi.Id == id);
            Product? oldProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == orderItem.Product_Id);
            if (orderItem is null)
                return new OrderDetailsDto { Message = "this order item not found please make sure you input correct data" };
            oldProduct.Quantity += orderItem.Quantity;
            await context.SaveChangesAsync();
            if (await CheckProductAndQuantity(model.ProductName, model.Quantity))
            {
                var product = (await context.Products.FirstOrDefaultAsync(o => o.Name == model.ProductName));
                orderItem.Product_Id = product.Id;
                orderItem.Quantity = model.Quantity;
                orderItem.Price = product.Price;
                product.Quantity -= orderItem.Quantity;
                await context.SaveChangesAsync();
                return await GetOrderDetails(orderItem.Order_Id);
            }

            return new OrderDetailsDto { Message = messageOfOrderProduct };
        }
        public async Task<OrderDetailsDto> DeleteItemInOrder(int id)
        {
            OrderItem? orderItem = await context.OrderItems.FirstOrDefaultAsync(oi => oi.Id == id);
            var orderId = orderItem.Order_Id;
            Product? oldProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == orderItem.Product_Id);
            if (orderItem is null)
                return new OrderDetailsDto { Message = "this order item not found please make sure you input correct data" };
            oldProduct.Quantity += orderItem.Quantity;
            context.OrderItems.Remove(orderItem);
            await context.SaveChangesAsync();
            return await GetOrderDetails(orderId);

        }
        public async Task<bool> DeleteOrder(int id)
        {
            Order? order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order is null) return false;
            if (await RemoveAllOrderItemsInOrder(id))
            {
                context.Orders.Remove(order);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        private async Task<bool> AddListOfProducts(List<ProductInOrder> products, Order order)
        {
            foreach (var item in products)
            {
                if (!await CheckProductAndQuantity(item.ProductName, item.Quantity))//!(await context.Products.AnyAsync(p => p.Name == item.ProductName)))
                    return false;
                await context.Orders.AddAsync(order);
                await context.SaveChangesAsync();
                var orderId = await context.Orders.FirstOrDefaultAsync(o => o.OrderDate == order.OrderDate && o.custmer_Id == order.custmer_Id);

                OrderItem model = new OrderItem()
                {
                    Product_Id = (await context.Products.FirstOrDefaultAsync(p => p.Name == item.ProductName)).Id,
                    Quantity = item.Quantity,
                    Order_Id = orderId.Id
                };
                model.Price = (await context.Products.FirstOrDefaultAsync(p => p.Name == item.ProductName)).Price * item.Quantity;
                (await context.Products.FirstOrDefaultAsync(p => p.Name == item.ProductName)).Quantity -= model.Quantity;
                await context.OrderItems.AddAsync(model);
                await context.SaveChangesAsync();
            }
            return true;
        }
        private async Task<bool> CheckProductAndQuantity(string name, int quantity)
        {
            if ((await context.Products.AnyAsync(o => o.Name == name)))
            {
                var product = (await context.Products.FirstOrDefaultAsync(o => o.Name == name));

                if (quantity <= 0 || quantity > (await context.Products.FirstOrDefaultAsync(p => p.Name == name))?.Quantity)
                {
                    messageOfOrderProduct = "we  don't have this quantity";
                    return false;
                }

                return true;
            }
            messageOfOrderProduct = $"this product : {name} is un avilable";
            return false;
        }
        private async Task<bool> RemoveAllOrderItemsInOrder(int id)
        {
            List<OrderItem> items = await context.OrderItems.Where(o => o.Order_Id == id).ToListAsync();
            if (items.Count == 0) return false;
            foreach (var item in items)
            {
                context.OrderItems.Remove(item);
                await context.SaveChangesAsync();
            }
            return true;
        }

    }
}
