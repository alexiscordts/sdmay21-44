using InpatientTherapySchedulingProgram.Exceptions.AppointmentExceptions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly CoreDbContext _context;

        public AppointmentService(CoreDbContext _context) {
            this._context = _context;
        }

        Task<IEnumerable<Appointment>> IAppointmentService.GetAllAppointments(Appointment appointment)
        {
            throw new System.NotImplementedException();
        }

        Task<IEnumerable<Appointment>> IAppointmentService.GetAllAppointmentsByTherapostId(Appointment appointment)
        {
            throw new System.NotImplementedException();
        }

        Task<IEnumerable<Appointment>> IAppointmentService.GetAllAppointmetnsByPatientId(Appointment appointment)
        {
            throw new System.NotImplementedException();
        }

        Task<IEnumerable<Appointment>> IAppointmentService.GetAppointmentsByRoomNumber(Appointment appointment)
        {
            throw new System.NotImplementedException();
        }

        Task<Appointment> IAppointmentService.UpdateAppointment(int appointmentId, Appointment appointment)
        {
            throw new System.NotImplementedException();
        }

        Task<Appointment> IAppointmentService.DeleteAppointment(int appointmentId)
        {
            throw new System.NotImplementedException();
        }

        Task<Appointment> IAppointmentService.AddAppointment(Appointment appointment)
        {
            
            throw new System.NotImplementedException();
        }

        private async Task<bool> AppointmentExistById(int appointmentId) {
            return await _context.Appointment.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.Active) != null;
        }


    }
}
