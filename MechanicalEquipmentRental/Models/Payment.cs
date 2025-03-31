using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManagementSystem.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required]
        public int ContractID { get; set; }

        public DateTime PaymentDate { get; set; }
        public float Amount { get; set; }

        [ForeignKey("ContractID")]
        public Contract Contract { get; set; }
    }
}
