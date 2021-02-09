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
    public class HoursWorkedController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public HoursWorkedController(CoreDbContext context)
        {
            _context = context;
        }

        // GET: api/HoursWorkeds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoursWorked>>> GetHoursWorked()
        {
            return await _context.HoursWorked.ToListAsync();
        }

        // GET: api/HoursWorkeds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HoursWorked>> GetHoursWorked(DateTime id)
        {
            var hoursWorked = await _context.HoursWorked.FindAsync(id);

            if (hoursWorked == null)
            {
                return NotFound();
            }

            return hoursWorked;
        }

        // PUT: api/HoursWorkeds/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoursWorked(DateTime id, HoursWorked hoursWorked)
        {
            if (id != hoursWorked.StartDatetime)
            {
                return BadRequest();
            }

            _context.Entry(hoursWorked).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoursWorkedExists(id))
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

        // POST: api/HoursWorkeds
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HoursWorked>> PostHoursWorked(HoursWorked hoursWorked)
        {
            _context.HoursWorked.Add(hoursWorked);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HoursWorkedExists(hoursWorked.StartDatetime))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHoursWorked", new { id = hoursWorked.StartDatetime }, hoursWorked);
        }

        // DELETE: api/HoursWorkeds/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HoursWorked>> DeleteHoursWorked(DateTime id)
        {
            var hoursWorked = await _context.HoursWorked.FindAsync(id);
            if (hoursWorked == null)
            {
                return NotFound();
            }

            _context.HoursWorked.Remove(hoursWorked);
            await _context.SaveChangesAsync();

            return hoursWorked;
        }

        private bool HoursWorkedExists(DateTime id)
        {
            return _context.HoursWorked.Any(e => e.StartDatetime == id);
        }
    }
}
