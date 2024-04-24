using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGuard.Models
{
    public class CPU : Product
    {
        public CPU(string name, string brand, decimal price, string description, bool available, string image,
            string socket, int cores, int threads, double clockSpeed)
            : base(name, brand, price, description, available, image)
        {
            Socket = socket;
            Cores = cores;
            Threads = threads;
            ClockSpeed = clockSpeed;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required(ErrorMessage = "Socket is required")]
        public string Socket { get; set; }

        [Required(ErrorMessage = "Number of cores is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of cores must be a positive integer")]
        public int Cores { get; set; }

        [Required(ErrorMessage = "Number of threads is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of threads must be a positive integer")]
        public int Threads { get; set; }

        [Required(ErrorMessage = "Clock speed is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Clock speed must be a positive number")]
        public double ClockSpeed { get; set; }
    }
}
