using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManagementSystem.Models
{
    public class RentalDetail
    {
        [Key]
        public int RentalDetailID { get; set; }

        [Required]
        public int RentalID { get; set; }

        [Required]
        public int MachineID { get; set; }

        public float RentalFee { get; set; }

        [ForeignKey("RentalID")]
        public Rental Rental { get; set; }

        [ForeignKey("MachineID")]
        public Machine Machine { get; set; }
    }
}
