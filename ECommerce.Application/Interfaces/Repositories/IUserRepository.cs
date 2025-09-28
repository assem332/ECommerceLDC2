using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<Customer> GetByIdAsync (Guid id);
        Task <Customer> GetByEmailAsync(string email);

        Task<IEnumerable<Customer>> GetAllAsync();

        Task AddAsync(Customer user);
    }
}
