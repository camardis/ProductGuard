using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGuard.Models
{
    public class Motherboard : ProductBase
    {
        public enum MotherboardFormFactor
        {
            ExtendedATX,
            ATX,
            MicroATX,
            MiniITX,
        }

        public Motherboard(string name, string brand, decimal price, string description, bool available, string image,
                       string socket, string formFactor, int maxMemory)
            : base(name, brand, price, description, available, image)
        {
            Socket = socket;
            FormFactor = formFactor;
            MaxMemory = maxMemory;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

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
