using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Carts
{
    public class CartResponseDto
    {
        public Guid Id { get; set; }

        public Guid CustomerId {  get; set; }  

        public List<CartItemResponseDto> Items { get; set; } = new();
    }
}
