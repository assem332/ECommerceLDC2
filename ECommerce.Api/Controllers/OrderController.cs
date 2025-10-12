using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces.UniteOfWork;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetByCustomer(Guid customerId)
        {
            var orders = await _unitOfWork.Orders.GetByCustomerIdAsync(customerId);
            if (!orders.Any())
                return NotFound("No orders found for this customer.");

            var response = orders.Select(o => new OrderResponseDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                Status = o.Status,
                IsDeleted = o.IsDeleted,
                Items = o.OrderItems.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? "",
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,

                }).ToList()
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetById (Guid id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return NotFound("Order not found.");

            var response = new OrderResponseDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                IsDeleted = order.IsDeleted,
                Items = order.OrderItems.Select(i => new OrderItemResponseDto
                {
                    ProductId= i.ProductId,
                    ProductName =i.Product?.Name??"",
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,

                }).ToList()
            };
            return Ok(response);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = dto.CustomerId,
                OrderDate =DateTime.Now,
                Status= "Pending",
                IsDeleted = false
            };

            foreach (var itemDto in dto.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    return NotFound($"Product with ID {itemDto.ProductId} not found.");
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };
                order.OrderItems.Add(orderItem);

            }
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();
            return Ok(new { Message = "Order created successfully", OrderId = order.Id });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return NotFound("Order not found.");

            await _unitOfWork.Orders.SoftDeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return Ok("Order soft-deleted successfully.");
        }
    }
}
