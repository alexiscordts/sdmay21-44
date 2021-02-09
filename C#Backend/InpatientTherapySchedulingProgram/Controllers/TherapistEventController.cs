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
    public class TherapistEventController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public TherapistEventController(CoreDbContext context)
        {
            _context = context;
        }

        // GET: api/TherapistEvent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TherapistEvent>>> GetTherapistEvent()
        {
            return await _context.TherapistEvent.ToListAsync();
        }

        // GET: api/TherapistEvent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TherapistEvent>> GetTherapistEvent(DateTime id)
        {
            var therapistEvent = await _context.TherapistEvent.FindAsync(id);

            if (therapistEvent == null)
            {
                return NotFound();
            }

            return therapistEvent;
        }

        // PUT: api/TherapistEvent/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTherapistEvent(DateTime id, TherapistEvent therapistEvent)
        {
            if (id != therapistEvent.StartDatetime)
            {
                return BadRequest();
            }

            _context.Entry(therapistEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TherapistEventExists(id))
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

        // POST: api/TherapistEvent
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TherapistEvent>> PostTherapistEvent(TherapistEvent therapistEvent)
        {
            _context.TherapistEvent.Add(therapistEvent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TherapistEventExists(therapistEvent.StartDatetime))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTherapistEvent", new { id = therapistEvent.StartDatetime }, therapistEvent);
        }

        // DELETE: api/TherapistEvent/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TherapistEvent>> DeleteTherapistEvent(DateTime id)
        {
            var therapistEvent = await _context.TherapistEvent.FindAsync(id);
            if (therapistEvent == null)
            {
                return NotFound();
            }

            _context.TherapistEvent.Remove(therapistEvent);
            await _context.SaveChangesAsync();

            return therapistEvent;
        }

        private bool TherapistEventExists(DateTime id)
        {
            return _context.TherapistEvent.Any(e => e.StartDatetime == id);
        }
    }
}
