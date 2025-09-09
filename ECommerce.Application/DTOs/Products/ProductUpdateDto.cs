using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Products
{
    public  class ProductUpdateDto : ProductCreateDto
    {
        [Required]
        public Guid Id { get; set; }

    }
}
