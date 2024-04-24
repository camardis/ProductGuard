using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGuard.Models
{
    public class GPU : ProductBase
    {
        public GPU()
        {
        }

        public GPU(string name, string brand, decimal price, string description, bool available, string image,
            int vram, string chipset, int coreClock)
            : base(name, brand, price, description, available, image)
        {
            VRAM = vram;
            Chipset = chipset;
            CoreClock = coreClock;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required(ErrorMessage = "VRAM size is required")]
        public int VRAM { get; set; } // Video RAM in GB

        [Required(ErrorMessage = "Chipset is required")]
        public string Chipset { get; set; }

        [Required(ErrorMessage = "Core clock speed is required")]
        public int CoreClock { get; set; } // Core clock speed in MHz
    }
}
