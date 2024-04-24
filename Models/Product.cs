using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGuard.Models
{
    public abstract class Product
    {
        protected Product()
        {
        }

        public Product(string name, string brand, decimal price, string description, bool available, string image)
        {
            Name = name;
            Brand = brand;
            Price = price;
            Description = description;
            Available = available;
            Image = image;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Uuid { get; set; } 

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Availability status is required")]
        public bool Available { get; set; }

        public string Image { get; set; }
    }
}
