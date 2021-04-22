using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgram.Exceptions.HoursWorkedExceptions;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;

namespace InpatientTherapySchedulingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoursWorkedController : ControllerBase
    {
        private readonly IHoursWorkedService _service;

        public HoursWorkedController(IHoursWorkedService service)
        {
            _service = service;
        }

        // GET: api/HoursWorkeds/5
        [HttpGet("single/{hoursWorkedId}")]
        public async Task<ActionResult<HoursWorked>> GetHoursWorked(int hoursWorkedId)
        {
            var hoursWorked = await _service.GetHoursWorkedById(hoursWorkedId);

            if (hoursWorked == null)
            {
               return NotFound();
            }

            return Ok(hoursWorked);
        }

        // GET: api/HoursWorkeds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HoursWorked>> GetHoursWorkedByUserId(int hoursWorkedId)
        {
            var hoursWorked = await _service.GetHoursWorkedByUserId(hoursWorkedId);

            if (hoursWorked == null)
            {
                return NotFound();
            }

            return Ok(hoursWorked);
        }

        // PUT: api/HoursWorkeds/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoursWorked(int id, HoursWorked hoursWorked)
        {
            try
            {
                await _service.UpdateHoursWorked(id, hoursWorked);
            }
            catch (HoursWorkedIdsDoNotMatchException e)
            {
                return BadRequest(e);
            }
            catch (HoursWorkedDoesNotExistException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/HoursWorkeds
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HoursWorked>> PostHoursWorked(HoursWorked hoursWorked)
        {
            try
            {
                await _service.AddHoursWorked(hoursWorked);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetHoursWorked", new { id = hoursWorked.HoursWorkedId }, hoursWorked);
        }

        // DELETE: api/HoursWorkeds/5
        [HttpDelete("{hoursWorkedId}")]
        public async Task<ActionResult<HoursWorked>> DeleteHoursWorked(int hoursWorkedId)
        {
            HoursWorked hoursWorked;

            try
            {
                hoursWorked = await _service.DeleteHoursWorked(hoursWorkedId);

                if (hoursWorked == null)
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(hoursWorked);
        }
    }
}
