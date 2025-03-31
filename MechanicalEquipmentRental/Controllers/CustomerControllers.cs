using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManagementSystem.DTOs;
using RentalManagementSystem.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Threading.Tasks;
using System.Linq;

namespace RentalManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách tất cả khách hàng
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // Lấy thông tin khách hàng theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }

        // Tạo mới khách hàng (CRUD)
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerID }, customer);
        }

        // Cập nhật thông tin khách hàng
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Customers.Any(e => e.CustomerID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Xóa khách hàng
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 📌 API ĐĂNG KÝ KHÁCH HÀNG
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Mật khẩu không được để trống!");
            }

            // Kiểm tra email đã tồn tại chưa
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerEmail == model.CustomerEmail);
            if (existingCustomer != null)
            {
                return BadRequest("Email này đã được sử dụng!");
            }

            // Mã hóa mật khẩu
            var (hashedPassword, salt) = HashPassword(model.Password);

            // Tạo khách hàng mới
            var customer = new Customer
            {
                CustomerName = model.CustomerName,
                CustomerPhone = model.CustomerPhone,
                CustomerEmail = model.CustomerEmail,
                CustomerAddress = model.CustomerAddress,
                PasswordHash = hashedPassword,
                PasswordSalt = salt
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công!" });
        }

        // 📌 Hàm mã hóa mật khẩu
        private (string hashedPassword, string salt) HashPassword(string password)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            string salt = Convert.ToBase64String(saltBytes);

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            return (hashedPassword, salt);
        }

        // 📌 API ĐĂNG NHẬP KHÁCH HÀNG
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerEmail == model.CustomerEmail);

            if (customer == null)
            {
                return Unauthorized("Email hoặc mật khẩu không đúng.");
            }

            // Kiểm tra mật khẩu
            if (!VerifyPassword(model.Password, customer.PasswordHash, customer.PasswordSalt))
            {
                return Unauthorized("Email hoặc mật khẩu không đúng.");
            }

            // Trả về thông tin khách hàng
            return Ok(new
            {
                CustomerID = customer.CustomerID,
                CustomerName = customer.CustomerName,
                CustomerEmail = customer.CustomerEmail,
                CustomerPhone = customer.CustomerPhone,
                CustomerAddress = customer.CustomerAddress
            });
        }

        // 📌 Hàm kiểm tra mật khẩu đã mã hóa
        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            return hashedPassword == storedHash;
        }

    }
}
