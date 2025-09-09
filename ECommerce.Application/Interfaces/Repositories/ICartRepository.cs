using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetByCustomerIdAsync(Guid customerId);

        Task AddItemAsync(Guid cartId, CartItem item);
        Task UpdateItemAsync(Guid cartId, Guid productId, int quantity);
        Task RemoveItemAsync(Guid cartId, Guid productId);
    }
}
