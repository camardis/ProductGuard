using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGuard.Models
{
    public class StorageDevice : Product
    {

        public enum InterfaceType
        {
            SATA,
            PCIe3,
            PCIe4,
            PCIe5
        }

        public StorageDevice(string name, string brand, decimal price, string description, bool available, string image,
                                  string type, int capacity, string @interface)
            : base(name, brand, price, description, available, image)
        {
            Type = type;
            Capacity = capacity;
            Interface = @interface;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [RegularExpression("^(HDD|SSD|NVMe)$", ErrorMessage = "Invalid storage device type")]
        public string Type { get; set; } // HDD, SSD, NVMe, etc.

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number")]
        public int Capacity { get; set; } // Capacity in GB

        [Required(ErrorMessage = "Interface is required")]
        [RegularExpression("^(SATA|PCIe 3.0|PCIe 4.0|PCIe 5.0)$", ErrorMessage = "Invalid storage device interface")]
        public string Interface { get; set; } // SATA, PCIe, etc.
    }
}
