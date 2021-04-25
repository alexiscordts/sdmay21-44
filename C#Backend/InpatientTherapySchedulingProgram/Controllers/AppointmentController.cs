using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgram.Exceptions.AppointmentExceptions;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;

namespace InpatientTherapySchedulingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        //private readonly CoreDbContext _context;
       
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appoinmentService)
        {
            _appointmentService = appoinmentService;
        }

        // GET: api/Appointments/getAppointments
        [HttpPost("getAppointments")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest();
            }

            var allAppointments = await _appointmentService.GetAllAppointments(appointment);

            return Ok(allAppointments);
        }

        // GET: api/Appointments/getAppointmentsByTherapistId
        [HttpPost("getAppointmentsByTherapistId")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentByTherapistId(Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest();
            }

            var allAppointments = await _appointmentService.GetAllAppointmentsByTherapistId(appointment);

            return Ok(allAppointments);
        }

        // GET: api/Appointments/getAppointmentsByPatientId
        [HttpPost("getAppointmentsByPatientId")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentByPatientId(Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest();
            }

            var allAppointments = await _appointmentService.GetAllAppointmetnsByPatientId(appointment);

            return Ok(allAppointments);
        }

        // GET: api/Appointments/getAppointmentsByRoomNumber
        [HttpPost("getAppointmentsByRoomNumber")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentByRoomNumber(Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest();
            }

            var allAppointments = await _appointmentService.GetAppointmentsByRoomNumber(appointment);

            return Ok(allAppointments);
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{appointmentId}")]
        public async Task<IActionResult> PutAppointment(int appointmentId, Appointment appointment)
        {
            try
            {
                await _appointmentService.UpdateAppointment(appointmentId, appointment);
            }
            catch (AppointmentIdsDoNotMatchException e)
            {
                return BadRequest(e);
            }
            catch (UserDoesNotExistException e)
            {
                return BadRequest(e);
            }
            catch (UserIsNotATherapistException e)
            {
                return BadRequest(e);
            }
            catch (AppointmentCannotEndBeforeStartTimeException e)
            {
                return BadRequest(e);
            }

            return NoContent();
        }

        // POST: api/Appointments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
            try
            {
                await _appointmentService.AddAppointment(appointment);
            }
            catch (AppointmentIdsDoNotMatchException e)
            {
                return Conflict(e);
            }
            catch (UserDoesNotExistException e)
            {
                return BadRequest(e);
            }
            catch (UserIsNotATherapistException e)
            {
                return BadRequest(e);
            }
            catch (AppointmentCannotEndBeforeStartTimeException e)
            {
                return BadRequest(e);
            }
            catch (AppointmentDoesNotExistException e)
            {
                return BadRequest(e);
            }

            return CreatedAtAction("GetAppointment", new { id = appointment.AppointmentId }, appointment);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{appointmentId}")]
        public async Task<ActionResult<Appointment>> DeleteAppointment(int appointmentId)
        {
            Appointment appointment;

            try
            {
                appointment = await _appointmentService.DeleteAppointment(appointmentId);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            if (appointment is null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }
    }
}
