using System.Collections.Generic;
using System.Threading.Tasks;
using InpatientTherapySchedulingProgram.Models;

namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointments(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAllAppointmentsByTherapistId(Appointment appointment);
        Task<Appointment> UpdateAppointment(int appointmentId, Appointment appointment);
        Task<Appointment> DeleteAppointment(int appointmentId);
        Task<Appointment> AddAppointment(Appointment appointment);
    }
}
