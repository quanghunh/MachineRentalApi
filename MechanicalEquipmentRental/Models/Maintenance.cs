using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManagementSystem.Models
{
    public class Maintenance
    {
        [Key]
        public int MaintenanceID { get; set; }

        [Required]
        [ForeignKey("Machine")]
        public int MachineID { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime? EndDate { get; set; }  // Có thể null nếu bảo trì chưa hoàn thành

        [Required, MaxLength(500)]
        public string Description { get; set; }

        // Navigation property
        public Machine? Machine { get; set; }
    }
}
