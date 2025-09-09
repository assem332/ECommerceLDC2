using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Products
{
    public class ProductCreateDto
    {
        [Required]

        public string Name {  get; set; }

        [Required]

        public string Description { get; set; }

        [Required]

        public decimal Price { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        [Required]
        public int StockQuantity {  get; set; }

        [Required]

        public Guid CategoryId { get; set; }




    }
}
