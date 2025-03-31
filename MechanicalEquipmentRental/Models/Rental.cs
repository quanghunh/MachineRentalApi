using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManagementSystem.Models
{
    public class Rental
    {
        [Key]
        public int RentalID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public float TotalCost { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }

        public ICollection<RentalDetail> RentalDetails { get; set; }
    }
}
