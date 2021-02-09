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
    public class PatientEventController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public PatientEventController(CoreDbContext context)
        {
            _context = context;
        }

        // GET: api/PatientEvent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientEvent>>> GetPatientEvent()
        {
            return await _context.PatientEvent.ToListAsync();
        }

        // GET: api/PatientEvent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientEvent>> GetPatientEvent(DateTime id)
        {
            var patientEvent = await _context.PatientEvent.FindAsync(id);

            if (patientEvent == null)
            {
                return NotFound();
            }

            return patientEvent;
        }

        // PUT: api/PatientEvent/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatientEvent(DateTime id, PatientEvent patientEvent)
        {
            if (id != patientEvent.StartDatetime)
            {
                return BadRequest();
            }

            _context.Entry(patientEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientEventExists(id))
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

        // POST: api/PatientEvent
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PatientEvent>> PostPatientEvent(PatientEvent patientEvent)
        {
            _context.PatientEvent.Add(patientEvent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PatientEventExists(patientEvent.StartDatetime))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPatientEvent", new { id = patientEvent.StartDatetime }, patientEvent);
        }

        // DELETE: api/PatientEvent/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PatientEvent>> DeletePatientEvent(DateTime id)
        {
            var patientEvent = await _context.PatientEvent.FindAsync(id);
            if (patientEvent == null)
            {
                return NotFound();
            }

            _context.PatientEvent.Remove(patientEvent);
            await _context.SaveChangesAsync();

            return patientEvent;
        }

        private bool PatientEventExists(DateTime id)
        {
            return _context.PatientEvent.Any(e => e.StartDatetime == id);
        }
    }
}
