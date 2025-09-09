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
        Task<User> GetByIdAsync (Guid id);
        Task <User> GetByEmailAsync(string email);

        Task<IEnumerable<User>> GetAllAsync();

        Task AddAsync(User user);
    }
}
