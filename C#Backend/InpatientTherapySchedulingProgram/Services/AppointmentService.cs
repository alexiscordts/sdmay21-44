using InpatientTherapySchedulingProgram.Exceptions.AppointmentExceptions;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly CoreDbContext _context;

        public AppointmentService(CoreDbContext _context)
        {
            this._context = _context;
        }
        public async Task<Appointment> AddAppointment(Appointment appointment)
        {
            if (await AppointmentExistsById(appointment.AppointmentId))
            {
                throw new AppointmentIdAlreadyExistsException();
            }
            if (!await UserExists(appointment.TherapistId))
            {
                throw new UserDoesNotExistException("Therapist does not exist");
            }
            if (!await IsTherapist(appointment.TherapistId))
            {
                throw new UserIsNotATherapistException();
            }
            if (!await UserExists(appointment.PmrPhysicianId))
            {
                throw new UserDoesNotExistException("PMR Physician does not exist");
            }
            if (appointment.EndTime < appointment.StartTime)
            {
                throw new AppointmentCannotEndBeforeStartTimeException();
            }

            appointment.Active = true;

            _context.Appointment.Add(appointment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return appointment;
        }

        public async Task<Appointment> DeleteAppointment(int appointmentId)
        {
            var appointment = await _context.Appointment.FindAsync(appointmentId);

            if (appointment is null)
            {
                return null;
            }

            appointment.Active = false;

            var local = _context.Appointment.Local.FirstOrDefault(a => a.AppointmentId == appointmentId);

            _context.Entry(local).State = EntityState.Detached;

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointments(Appointment appointment)
        {
            return await _context.Appointment
                .Where(a => a.StartTime >= appointment.StartTime && a.EndTime <= appointment.EndTime && a.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsByTherapistId(Appointment appointment)
        {
            return await _context.Appointment
                .Where(a => a.StartTime >= appointment.StartTime
                && a.EndTime <= appointment.EndTime 
                && a.Active 
                && a.TherapistId == appointment.TherapistId)
                .ToListAsync();
        }

        public async Task<Appointment> UpdateAppointment(int appointmentId, Appointment appointment)
        {
            if (appointmentId != appointment.AppointmentId)
            {
                throw new AppointmentIdsDoNotMatchException();
            }
            if (!await UserExists(appointment.TherapistId))
            {
                throw new UserDoesNotExistException("Therapist does not exist");
            }
            if (!await IsTherapist(appointment.TherapistId))
            {
                throw new UserIsNotATherapistException();
            }
            if (!await UserExists(appointment.PmrPhysicianId))
            {
                throw new UserDoesNotExistException("PMR physician does not exist");
            }
            if (appointment.EndTime < appointment.StartTime)
            {
                throw new AppointmentCannotEndBeforeStartTimeException();
            }

            var local = await _context.Appointment.FindAsync(appointmentId);

            if (local is null)
            {
                throw new AppointmentDoesNotExistException();
            }

            UpdateNonNullAndNonEmptyFields(local, appointment);

            _context.Entry(local).State = EntityState.Modified;

            //_context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return appointment;
        }

        private void UpdateNonNullAndNonEmptyFields(Appointment local, Appointment appointment)
        {
            foreach (PropertyInfo prop in typeof(Appointment).GetProperties())
            {
                if (prop.GetValue(appointment) != null && (prop.PropertyType != typeof(string) || !prop.GetValue(appointment).Equals("")))
                {
                    prop.SetValue(local, prop.GetValue(appointment));
                }
            }
        }

        private async Task<bool> AppointmentExistsById(int appointmentId)
        {
            return await  _context.Appointment.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.Active) != null;
        }

        private async Task<bool> UserExists(int? therapistId)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == therapistId && u.Active == true);

            if (user is null || !user.Active)
            {
                return false;
            }

            return true;
        }
        private async Task<bool> IsTherapist(int? therapistId)
        {
            return await _context.Permission.FirstOrDefaultAsync(p => p.UserId == therapistId && p.Role.Equals("therapist")) != null;
        }
    }
}
