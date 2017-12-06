using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductShop.Models
{
    public class Category
    {
        public Category()
        {
            this.Products = new List<CategoryProducts>();
        }

        public int CategoryId { get; set; }

        [MinLength(3)]
        public string Name { get; set; }

        public ICollection<CategoryProducts> Products { get; set; }
    }
}