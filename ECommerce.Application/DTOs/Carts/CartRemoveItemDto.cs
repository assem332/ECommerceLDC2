using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Carts
{
    public class CartRemoveItemDto
    {
        [Required]
        public Guid CartId { get; set; }

        [Required]
        public Guid ProductId { get; set; }
    }
}
