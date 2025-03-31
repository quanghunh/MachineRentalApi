using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MachinesController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // 📌 Lấy danh sách máy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
        {
            var machines = await _context.Machines
                                         .AsNoTracking()
                                         .ToListAsync();
            return Ok(machines);
        }

        // 📌 Lấy thông tin máy theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Machine>> GetMachine(int id)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine == null)
            {
                return NotFound(new { message = "Machine not found" });
            }
            return Ok(machine);
        }

        // 📌 Thêm máy mới
        [HttpPost]
        public async Task<ActionResult<Machine>> CreateMachine([FromBody] Machine machine)
        {
            if (machine == null || string.IsNullOrWhiteSpace(machine.MachineName) || string.IsNullOrWhiteSpace(machine.Status))
            {
                return BadRequest(new { message = "Invalid request: Missing required fields" });
            }

            if (machine.PurchaseDate == default)
            {
                machine.PurchaseDate = DateTime.UtcNow;
            }

            try
            {
                _context.Machines.Add(machine);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetMachine), new { id = machine.MachineID }, machine);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Database error", error = ex.Message });
            }
        }

        // 📌 Cập nhật máy
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachine(int id, [FromBody] Machine machine)
        {
            if (id != machine.MachineID)
            {
                return BadRequest(new { message = "Machine ID mismatch" });
            }

            var existingMachine = await _context.Machines.FindAsync(id);
            if (existingMachine == null)
            {
                return NotFound(new { message = "Machine not found" });
            }

            try
            {
                _context.Entry(existingMachine).CurrentValues.SetValues(machine);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Concurrency error while updating" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Database update error", error = ex.Message });
            }
        }

        // 📌 Xóa máy
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine == null)
            {
                return NotFound(new { message = "Machine not found" });
            }

            try
            {
                _context.Machines.Remove(machine);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error deleting record", error = ex.Message });
            }
        }
    }
}
