using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalManagementSystem.Models
{
    public class Machine
    {
        [Key]
        public int MachineID { get; set; }

        [Required, MaxLength(100)]
        public string MachineName { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "Available";  // Mặc định là "Available"

        // Quan hệ 1-N với RentalDetail
        public ICollection<RentalDetail>? RentalDetails { get; set; }

        // Quan hệ 1-N với Maintenance
        public ICollection<Maintenance>? Maintenances { get; set; }

        // Quan hệ N-N với Component thông qua bảng trung gian MComponent
        public ICollection<MComponent>? MComponents { get; set; }
    }
}
