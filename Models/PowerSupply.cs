using ProductGuard.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProductGuard.Models
{
    public class PowerSupply : ProductBase
    {
        public PowerSupply(string name, string brand, decimal price, string description, bool available, string image,
                       int wattage, string formFactor, string efficiencyRating)
            : base(name, brand, price, description, available, image)
        {
            Wattage = wattage;
            FormFactor = formFactor;
            EfficiencyRating = efficiencyRating;
        }

        [Required(ErrorMessage = "Wattage is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Wattage must be a positive integer")]
        public int Wattage { get; set; }

        [Required(ErrorMessage = "Form factor is required")]
        [EnumDataType(typeof(PowerSupplyFormFactor), ErrorMessage = "Invalid form factor")]
        public string FormFactor { get; set; }

        [Required(ErrorMessage = "Efficiency rating is required")]
        [EnumDataType(typeof(PowerSupplyEfficiencyStandardEnums), ErrorMessage = "Invalid efficiency rating")]
        public string EfficiencyRating { get; set; }
    }
}
