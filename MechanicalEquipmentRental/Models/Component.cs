using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalManagementSystem.Models
{
    public class Component
    {
        [Key]
        public int ComponentID { get; set; }

        [Required, MaxLength(100)]
        public string ComponentName { get; set; }

        public ICollection<MComponent>? MComponents { get; set; }
    }
}
