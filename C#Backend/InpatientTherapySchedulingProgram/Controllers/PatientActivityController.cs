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
    public class PatientActivityController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public PatientActivityController(CoreDbContext context)
        {
            _context = context;
        }

        // GET: api/PatientActivity
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientActivity>>> GetPatientActivity()
        {
            return await _context.PatientActivity.ToListAsync();
        }

        // GET: api/PatientActivity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientActivity>> GetPatientActivity(string id)
        {
            var patientActivity = await _context.PatientActivity.FindAsync(id);

            if (patientActivity == null)
            {
                return NotFound();
            }

            return patientActivity;
        }

        // PUT: api/PatientActivity/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatientActivity(string id, PatientActivity patientActivity)
        {
            if (id != patientActivity.Name)
            {
                return BadRequest();
            }

            _context.Entry(patientActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientActivityExists(id))
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

        // POST: api/PatientActivity
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PatientActivity>> PostPatientActivity(PatientActivity patientActivity)
        {
            _context.PatientActivity.Add(patientActivity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PatientActivityExists(patientActivity.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPatientActivity", new { id = patientActivity.Name }, patientActivity);
        }

        // DELETE: api/PatientActivity/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PatientActivity>> DeletePatientActivity(string id)
        {
            var patientActivity = await _context.PatientActivity.FindAsync(id);
            if (patientActivity == null)
            {
                return NotFound();
            }

            _context.PatientActivity.Remove(patientActivity);
            await _context.SaveChangesAsync();

            return patientActivity;
        }

        private bool PatientActivityExists(string id)
        {
            return _context.PatientActivity.Any(e => e.Name == id);
        }
    }
}
