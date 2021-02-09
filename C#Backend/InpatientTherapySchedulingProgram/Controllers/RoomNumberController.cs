using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InpatientTherapySchedulingProgram.Models;

namespace InpatientTherapySchedulingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomNumberController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public RoomNumberController(CoreDbContext context)
        {
            _context = context;
        }

        // GET: api/RoomNumber
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomNumber>>> GetRoomNumber()
        {
            return await _context.RoomNumber.ToListAsync();
        }

        // GET: api/RoomNumber/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomNumber>> GetRoomNumber(int id)
        {
            var roomNumber = await _context.RoomNumber.FindAsync(id);

            if (roomNumber == null)
            {
                return NotFound();
            }

            return roomNumber;
        }

        // PUT: api/RoomNumber/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomNumber(int id, RoomNumber roomNumber)
        {
            if (id != roomNumber.Number)
            {
                return BadRequest();
            }

            _context.Entry(roomNumber).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomNumberExists(id))
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

        // POST: api/RoomNumber
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RoomNumber>> PostRoomNumber(RoomNumber roomNumber)
        {
            _context.RoomNumber.Add(roomNumber);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RoomNumberExists(roomNumber.Number))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRoomNumber", new { id = roomNumber.Number }, roomNumber);
        }

        // DELETE: api/RoomNumber/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomNumber>> DeleteRoomNumber(int id)
        {
            var roomNumber = await _context.RoomNumber.FindAsync(id);
            if (roomNumber == null)
            {
                return NotFound();
            }

            _context.RoomNumber.Remove(roomNumber);
            await _context.SaveChangesAsync();

            return roomNumber;
        }

        private bool RoomNumberExists(int id)
        {
            return _context.RoomNumber.Any(e => e.Number == id);
        }
    }
}
