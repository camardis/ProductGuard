using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGuard.Models
{
    public abstract class ProductBase
    {
        protected ProductBase()
        {

        }

        public ProductBase(string name, string brand, decimal price, string description, bool available, string image)
        {
            Name = name;
            Brand = brand;
            Price = price;
            Description = description;
            Available = available;
            Image = image;
            Category = GetType().Name;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Uuid { get; set; }

        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Short description is required")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Availability status is required")]
        public bool Available { get; set; }

        public string Image { get; set; }

        [Required]
        public int StockAmount { get; set; }

        [Required]
        public string Category { get; set; }

    }
}
