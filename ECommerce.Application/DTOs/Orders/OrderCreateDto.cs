using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Orders
{
    public class OrderCreateDto
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public List<OrderItemCreateDto> Items { get; set; } = new();
    }
}
