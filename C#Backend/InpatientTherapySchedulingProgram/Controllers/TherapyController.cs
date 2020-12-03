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
    public class TherapyController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public TherapyController(CoreDbContext context)
        {
            _context = context;
        }

        // GET: api/Therapy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Therapy>>> GetTherapy()
        {
            return await _context.Therapy.ToListAsync();
        }

        // GET: api/Therapy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Therapy>> GetTherapy(string id)
        {
            var therapy = await _context.Therapy.FindAsync(id);

            if (therapy == null)
            {
                return NotFound();
            }

            return therapy;
        }

        // PUT: api/Therapy/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTherapy(string id, Therapy therapy)
        {
            if (id != therapy.Adl)
            {
                return BadRequest();
            }

            _context.Entry(therapy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TherapyExists(id))
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

        // POST: api/Therapy
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Therapy>> PostTherapy(Therapy therapy)
        {
            _context.Therapy.Add(therapy);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TherapyExists(therapy.Adl))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTherapy", new { id = therapy.Adl }, therapy);
        }

        // DELETE: api/Therapy/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Therapy>> DeleteTherapy(string id)
        {
            var therapy = await _context.Therapy.FindAsync(id);
            if (therapy == null)
            {
                return NotFound();
            }

            _context.Therapy.Remove(therapy);
            await _context.SaveChangesAsync();

            return therapy;
        }

        private bool TherapyExists(string id)
        {
            return _context.Therapy.Any(e => e.Adl == id);
        }
    }
}
