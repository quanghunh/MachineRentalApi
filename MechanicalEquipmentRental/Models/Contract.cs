using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManagementSystem.Models
{
    public class Contract
    {
        [Key]
        public int ContractID { get; set; }

        [Required]
        public int RentalDetailID { get; set; }

        public DateTime SignDate { get; set; }
        public DateTime DueDate { get; set; }

        [ForeignKey("RentalDetailID")]
        public RentalDetail RentalDetail { get; set; }
    }
}

