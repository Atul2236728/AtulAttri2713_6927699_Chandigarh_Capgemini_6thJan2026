using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }
    }
}