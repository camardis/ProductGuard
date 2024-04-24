using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGuard.Models
{
    public class RAM : Product
    {
        public RAM(string name, string brand, decimal price, string description, bool available, string image,
                       int capacity, string type, int speed)
            : base(name, brand, price, description, available, image)
        {
            Capacity = capacity;
            Type = type;
            Speed = speed;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive integer")]
        public int Capacity { get; set; } // Capacity in GB

        [Required(ErrorMessage = "Type is required")]
        [RegularExpression("^(DDR3|DDR4|DDR5)$", ErrorMessage = "Invalid RAM type")]
        public string Type { get; set; } // DDR3, DDR4, etc.

        [Required(ErrorMessage = "Speed is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Speed must be a positive integer")]
        public int Speed { get; set; } // Speed in MHz
    }
}
