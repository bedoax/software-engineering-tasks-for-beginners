using BackendTask4.Model;
using BackendTasks.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendTask4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get all orders
        [HttpGet]
        public IActionResult Get()
        {
            var orders = _dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .ToList();

            if (!orders.Any())
            {
                return NotFound();
            }

            var orderDetails = orders.Select(o => new
            {
                o.Id,
                o.User.Username,
                o.OrderDate,
                OrderDetails = o.OrderItems.Select(oi => new
                {
                    oi.ProductId,
                    oi.Quantity,
                    oi.UnitPrice
                }).ToList()
            }).ToList();

            return Ok(orderDetails);
        }

        // Get order by ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.User) // Include User
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDetails = order.OrderItems.Select(oi => new
            {
                order.User.Username, // Direct access to Username
                oi.ProductId,
                oi.Quantity,
                oi.UnitPrice
            }).ToList();

            return Ok(orderDetails);
        }

        // Create a new order
        [HttpPost]
        public IActionResult Post([FromBody] OrderDto orderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                // Create the order
                var order = new Order
                {
                    UserId = orderRequest.UserId,
                    OrderDate = DateTime.UtcNow,
                };

                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges(); // Save to get the OrderId

                // Create order items
                var orderItems = orderRequest.OrderItemDtos.Select(item =>
                {
                

                    return new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice // Use the product's price
                    };
                }).ToList();

                _dbContext.OrderItems.AddRange(orderItems);
                _dbContext.SaveChanges(); // Save all order items

                transaction.Commit(); // Commit the transaction

                return Ok(new { message = "Order created successfully", order });
            }
            catch (Exception e)
            {
                transaction.Rollback(); // Rollback on error
                return BadRequest(new { message = e.Message });
            }
        }

        // Update an existing order
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] OrderUpdateDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingOrder = _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            // Validate products
            var productIds = orderDto.OrderDetails.Select(od => od.ProductId).ToList();
            var products = _dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToList();

            if (products.Count != productIds.Distinct().Count())
            {
                return BadRequest("One or more products not found.");
            }

            // Update order
            existingOrder.UserId = orderDto.UserId;
            _dbContext.OrderItems.RemoveRange(existingOrder.OrderItems);

            existingOrder.OrderItems = orderDto.OrderDetails.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = products.First(p => p.Id == item.ProductId).Price // Use product's price
            }).ToList();

            _dbContext.SaveChanges();
            return Ok(new { message = "Order updated successfully!" });
        }

        // Delete an order
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            _dbContext.OrderItems.RemoveRange(order.OrderItems);
            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();

            return Ok(new { message = "Order deleted successfully!" });
        }
    }
}