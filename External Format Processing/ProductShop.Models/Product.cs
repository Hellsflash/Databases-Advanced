using System.Collections.Generic;

namespace ProductShop.Models
{
    public class Product
    {
        public Product()
        {
            this.Categories = new List<CategoryProducts>();
        }

        public int ProductId { get; set; }

        public int? BuyerId { get; set; }
        public User Buyer { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int SellerId { get; set; }
        public User Seller { get; set; }

        public ICollection<CategoryProducts> Categories { get; set; }
    }
}