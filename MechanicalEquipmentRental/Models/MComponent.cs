using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManagementSystem.Models
{
    public class MComponent
    {
        [Key]
        public int MComponentID { get; set; }

        [Required]
        [ForeignKey("Machine")]
        public int MachineID { get; set; }

        [Required]
        [ForeignKey("Component")]
        public int ComponentID { get; set; }

        // Navigation properties
        public Machine? Machine { get; set; }
        public Component? Component { get; set; }
    }
}
