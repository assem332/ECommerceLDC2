using ECommerce.Application.DTOs.Carts;
using ECommerce.Application.Interfaces.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;


        
        private Guid GetCustomerId()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(customerId);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var customerId = GetCustomerId();
            var cart = await _unitOfWork.Carts.GetByCustomerIdAsync(customerId);

            if (cart == null)
                return NotFound("Cart not found");

            var response = new CartResponseDto
            {
                Id = cart.Id,
                CustomerId = cart.CustomerId,
                Items = cart.Items.Select(i => new CartItemResponseDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product.Price
                }).ToList()
            };

            return Ok(response);
        }

        
        [HttpPost("add-item")]
        public async Task<IActionResult> AddItem([FromBody] CartUpdateItemDto dto)
        {
            var customerId = GetCustomerId();
            var cart = await _unitOfWork.Carts.GetByCustomerIdAsync(customerId);

            if (cart == null)
            {
                cart = new Domain.Entities.Cart
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId
                };
                await _unitOfWork.Carts.AddItemAsync(cart.Id, new Domain.Entities.CartItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }
            else
            {
                await _unitOfWork.Carts.AddItemAsync(cart.Id, new Domain.Entities.CartItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }

            await _unitOfWork.CompleteAsync();
            return Ok("Item added successfully");
        }

       
        [HttpPut("update-item")]
        public async Task<IActionResult> UpdateItem([FromBody] CartUpdateItemDto dto)
        {
            var customerId = GetCustomerId();
            var cart = await _unitOfWork.Carts.GetByCustomerIdAsync(customerId);

            if (cart == null) return NotFound("Cart not found");

            await _unitOfWork.Carts.UpdateItemAsync(cart.Id, dto.ProductId, dto.Quantity);
            await _unitOfWork.CompleteAsync();

            return Ok("Item updated successfully");
        }

       
        [HttpDelete("remove-item/{productId}")]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {
            var customerId = GetCustomerId();
            var cart = await _unitOfWork.Carts.GetByCustomerIdAsync(customerId);

            if (cart == null) return NotFound("Cart not found");

            await _unitOfWork.Carts.RemoveItemAsync(cart.Id, productId);
            await _unitOfWork.CompleteAsync();

            return Ok("Item removed successfully");
        }

    }
}
