using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context) => _context = context;

        public async Task<Cart> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }
        public async Task AddItemAsync(Guid cartId, CartItem item)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart != null)
            {
                cart.Items.Add(item);
            }
        }

        public async Task UpdateItemAsync(Guid cartId, Guid productId, int quantity)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                _context.CartItems.Update(item);
            }
        }

        public async Task RemoveItemAsync(Guid cartId, Guid productId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductId == productId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
            }
        }


    }
}
