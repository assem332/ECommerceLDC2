using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.UniteOfWork;
using ECommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository Users { get; }
        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }
        public ICartRepository Carts { get; }

        public UnitOfWork (ApplicationDbContext context, IUserRepository userRepository, IProductRepository productRepository,
            IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _context = context;
            Users = userRepository;
            Products = productRepository;
            Orders = orderRepository;
            Carts = cartRepository;
        }

        
        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();


        public void Dispose() => _context.Dispose();
          



    }
}
