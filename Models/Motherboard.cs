using System.ComponentModel.DataAnnotations;
using ProductGuard.Enums;

namespace ProductGuard.Models
{
    public class Motherboard : ProductBase
    {
        public Motherboard(string name, string brand, decimal price, string description, bool available, string image,
                       string socket, string formFactor, int maxMemory)
            : base(name, brand, price, description, available, image)
        {
            Socket = socket;
            FormFactor = formFactor;
            MaxMemory = maxMemory;
        }

        [Required(ErrorMessage = "Socket is required")]
        public string Socket { get; set; }

        [Required(ErrorMessage = "Form factor is required")]
        [EnumDataType(typeof(MotherboardFormFactor), ErrorMessage = "Invalid form factor")]
        public string FormFactor { get; set; }

        [Required(ErrorMessage = "Maximum memory is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Maximum memory must be a positive integer")]
        public int MaxMemory { get; set; } // Maximum supported memory in GB
    }
}
