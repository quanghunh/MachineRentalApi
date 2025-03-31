using System.ComponentModel.DataAnnotations;

namespace RentalManagementSystem.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required, MaxLength(100)]
        public string CustomerName { get; set; }

        [Required, MaxLength(20)]
        public string CustomerPhone { get; set; }

        [Required, EmailAddress]
        public string CustomerEmail { get; set; }

        public string CustomerAddress { get; set; }

        [Required]
        public string PasswordHash { get; set; }  // Mật khẩu đã mã hóa

        [Required]
        public string PasswordSalt { get; set; }  // Salt để mã hóa mật khẩu
    }
}
