using System.ComponentModel.DataAnnotations;
using ProductGuard.Enums;

namespace ProductGuard.Models
{
    public class StorageDevice : ProductBase
    {

        public StorageDevice(string name, string brand, decimal price, string description, bool available, string image,
                                  string type, int capacity, string @interface)
            : base(name, brand, price, description, available, image)
        {
            Type = type;
            Capacity = capacity;
            Interface = @interface;
        }

        [Required(ErrorMessage = "Type is required")]
        [EnumDataType(typeof(StorageDeviceTypeEnums), ErrorMessage = "Invalid storage device type")]
        public string Type { get; set; } // HDD, SSD, NVMe, etc.

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number")]
        public int Capacity { get; set; } // Capacity in GB

        [Required(ErrorMessage = "Interface is required")]
        [EnumDataType(typeof(StorageInterfaceEnums), ErrorMessage = "Invalid storage device interface")]
        public string Interface { get; set; } // SATA, PCIe, etc.
    }
}
