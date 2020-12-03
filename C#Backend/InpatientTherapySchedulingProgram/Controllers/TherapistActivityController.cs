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
    public class TherapistActivityController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public TherapistActivityController(CoreDbContext context)
        {
            _context = context;
        }

        // GET: api/TherapistActivity
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TherapistActivity>>> GetTherapistActivity()
        {
            return await _context.TherapistActivity.ToListAsync();
        }

        // GET: api/TherapistActivity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TherapistActivity>> GetTherapistActivity(string id)
        {
            var therapistActivity = await _context.TherapistActivity.FindAsync(id);

            if (therapistActivity == null)
            {
                return NotFound();
            }

            return therapistActivity;
        }

        // PUT: api/TherapistActivity/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTherapistActivity(string id, TherapistActivity therapistActivity)
        {
            if (id != therapistActivity.Name)
            {
                return BadRequest();
            }

            _context.Entry(therapistActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TherapistActivityExists(id))
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

        // POST: api/TherapistActivity
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TherapistActivity>> PostTherapistActivity(TherapistActivity therapistActivity)
        {
            _context.TherapistActivity.Add(therapistActivity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TherapistActivityExists(therapistActivity.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTherapistActivity", new { id = therapistActivity.Name }, therapistActivity);
        }

        // DELETE: api/TherapistActivity/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TherapistActivity>> DeleteTherapistActivity(string id)
        {
            var therapistActivity = await _context.TherapistActivity.FindAsync(id);
            if (therapistActivity == null)
            {
                return NotFound();
            }

            _context.TherapistActivity.Remove(therapistActivity);
            await _context.SaveChangesAsync();

            return therapistActivity;
        }

        private bool TherapistActivityExists(string id)
        {
            return _context.TherapistActivity.Any(e => e.Name == id);
        }
    }
}
