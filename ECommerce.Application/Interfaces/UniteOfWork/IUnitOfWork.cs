using ECommerce.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.UniteOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }

        IProductRepository Products { get; }

        IOrderRepository Orders { get; }

        ICartRepository Carts { get; }

        Task<int> CompleteAsync();



    }
}
