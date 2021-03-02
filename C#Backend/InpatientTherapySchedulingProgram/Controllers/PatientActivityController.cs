using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgram.Exceptions.PatientActivityExceptions;

namespace InpatientTherapySchedulingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientActivityController : ControllerBase
    {
        private readonly IPatientActivity _service;

        public PatientActivityController(IPatientActivity service)
        {
            _service = service;
        }

        // GET: api/PatientActivity
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientActivity>>> GetPatientActivity()
        {
            var allPatientActivity = await _service.GetAllPatientActivity();
            
            return Ok(allPatientActivity);
        }

        // GET: api/PatientActivity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientActivity>> GetPatientActivity(string name)
        {
            var patientActivity = await _service.GetPatientActivityByName(name);

            if (patientActivity == null)
            {
                return NotFound();
            }

            return patientActivity;
        }

<<<<<<< HEAD
=======
        // PUT: api/PatientActivity/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatientActivity(string id, PatientActivity patientActivity)
        {
            if (id != patientActivity.ActivityName)
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

>>>>>>> master
        // POST: api/PatientActivity
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PatientActivity>> PostPatientActivity(PatientActivity patientActivity)
        {
            try
            {
                await _service.AddPatientActivity(patientActivity);
            }
            catch(PatientActivityNameAlreadyExistsException e)
            {
                return BadRequest(e);
            }
            catch (DbUpdateException)
            {
<<<<<<< HEAD
                throw;
=======
                if (PatientActivityExists(patientActivity.ActivityName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
>>>>>>> master
            }

            return CreatedAtAction("GetPatientActivity", new { id = patientActivity.ActivityName }, patientActivity);
        }

        // DELETE: api/PatientActivity/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PatientActivity>> DeletePatientActivity(string activityName)
        {
            PatientActivity patientActivity;

            try
            {
                patientActivity = await _service.DeletePatientActivity(activityName);

                if (patientActivity == null)
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

<<<<<<< HEAD
            return Ok(patientActivity);
=======
        private bool PatientActivityExists(string id)
        {
            return _context.PatientActivity.Any(e => e.ActivityName.Equals(id));
>>>>>>> master
        }
    }
}
