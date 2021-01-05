using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgram.Exceptions.TherapistActivityExceptions;

namespace InpatientTherapySchedulingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistActivityController : ControllerBase
    {
        private readonly ITherapistActivityService _service;

        public TherapistActivityController(ITherapistActivityService service)
        {
            _service = service;
        }

        // GET: api/TherapistActivity
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TherapistActivity>>> GetTherapistActivity()
        {
            var allTherapistActivities = await _service.GetAllTherapistActivities();

            return Ok(allTherapistActivities);
        }

        // GET: api/TherapistActivity/5
        [HttpGet("{name}")]
        public async Task<ActionResult<TherapistActivity>> GetTherapistActivity(string name)
        {
            var therapistActivity = await _service.GetTherapistActivityByName(name);

            if (therapistActivity == null)
            {
                return NotFound();
            }

            return Ok(therapistActivity);
        }

        // PUT: api/TherapistActivity/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{name}")]
        public async Task<IActionResult> PutTherapistActivity(string name, TherapistActivity therapistActivity)
        {
            try
            {
                await _service.UpdateTherapistActivity(name, therapistActivity);
            }
            catch(TherapistActivityNamesDoNotMatchException e)
            {
                return BadRequest(e);
            }
            catch(TherapistActivityDoesNotExistException)
            {
                return NotFound();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/TherapistActivity
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TherapistActivity>> PostTherapistActivity(TherapistActivity therapistActivity)
        {
            try
            {
                await _service.AddTherapistActivity(therapistActivity);
            }
            catch(TherapistActivityAlreadyExistsException e)
            {
                return Conflict(e);
            }
            catch(DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetTherapistActivity", new { id = therapistActivity.Name }, therapistActivity);
        }

        // DELETE: api/TherapistActivity/5
        [HttpDelete("{name}")]
        public async Task<ActionResult<TherapistActivity>> DeleteTherapistActivity(string name)
        {
            TherapistActivity therapistActivity = null;

            try
            {
                therapistActivity = await _service.DeleteTherapistActivity(name);
            }
            catch(DbUpdateException)
            {
                throw;
            }

            if(therapistActivity == null)
            {
                return NotFound();
            }

            return Ok(therapistActivity);
        }
    }
}
