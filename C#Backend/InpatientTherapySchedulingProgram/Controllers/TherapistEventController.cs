using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;
using InpatientTherapySchedulingProgram.Exceptions.TherapistEventExceptions;

namespace InpatientTherapySchedulingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistEventController : ControllerBase
    {
        private readonly ITherapistEventService _therapistEventService;

        public TherapistEventController(ITherapistEventService therapistEventService)
        {
            _therapistEventService = therapistEventService;
        }

        // GET: api/TherapistEvent
        // Maybe get all therapist event in a given week?
        // Force a JSON object to be passed in
        // Force route to use getByTherapistId
        [HttpPost("getTherapistEvent")]
        public async Task<ActionResult<IEnumerable<TherapistEvent>>> GetTherapistEvents(TherapistEvent therapistEvent)
        {
            if (therapistEvent == null)
            {
                return BadRequest();
            }

            var allTherapistEvents = await _therapistEventService.GetAllTherapistEvents(therapistEvent);

            return Ok(allTherapistEvents);
        }

        // GET: api/TherapistEvent/5
        // Probably need to get all therapist event's within a given time frame for therapist id
        [HttpPost("getTherapistEventsByTherapistId")]
        public async Task<ActionResult<IEnumerable<TherapistEvent>>> GetTherapistEventsByTherapistId(TherapistEvent therapistEvent)
        {
            if (therapistEvent == null)
            {
                return BadRequest();
            }

            var allTherapistEvents = await _therapistEventService.GetAllTherapistEventsByTherapistId(therapistEvent);

            return Ok(allTherapistEvents);
        }

        // PUT: api/TherapistEvent/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{eventId}")]
        public async Task<IActionResult> PutTherapistEvent(int eventId, TherapistEvent therapistEvent)
        {
            try
            {
                await _therapistEventService.UpdateTherapistEvent(eventId, therapistEvent);
            }
            catch (TherapistEventEventIdsDoNotMatchException e)
            {
                return BadRequest(e);
            }
            catch (TherapistEventDoesNotExistException e)
            {
                return NotFound(e);
            }
            catch (UserDoesNotExistException e)
            {
                return BadRequest(e);
            }
            catch (UserIsNotATherapistException e)
            {
                return BadRequest(e);
            }
            catch (TherapistEventCannotEndBeforeStartTimeException e)
            {
                return BadRequest(e);
            }

            return NoContent();
        }

        // POST: api/TherapistEvent
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TherapistEvent>> PostTherapistEvent(TherapistEvent therapistEvent)
        {
            try
            {
                await _therapistEventService.AddTherapistEvent(therapistEvent);
            }
            catch (TherapistEventEventIdAlreadyExistsException e)
            {
                return Conflict(e);
            }
            catch (UserDoesNotExistException e)
            {
                return NotFound(e);
            }
            catch (UserIsNotATherapistException e)
            {
                return BadRequest(e);
            }
            catch (TherapistEventCannotEndBeforeStartTimeException e)
            {
                return BadRequest(e);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetTherapistEvents", new { id = therapistEvent.EventId }, therapistEvent);
        }

        // DELETE: api/TherapistEvent/5
        [HttpDelete("{eventId}")]
        public async Task<ActionResult<TherapistEvent>> DeleteTherapistEvent(int eventId)
        {
            TherapistEvent therapistEvent;

            try
            {
                therapistEvent = await _therapistEventService.DeleteTherapistEvent(eventId);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            if (therapistEvent is null)
            {
                return NotFound();
            }

            return Ok(therapistEvent);
        }
    }
}
