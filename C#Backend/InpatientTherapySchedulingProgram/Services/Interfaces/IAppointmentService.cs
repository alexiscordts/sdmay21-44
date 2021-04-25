using InpatientTherapySchedulingProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointments(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAllAppointmentsByTherapostId(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAllAppointmetnsByPatientId(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAppointmentsByRoomNumber(Appointment appointment);
        Task<Appointment> UpdateAppointment(int appointmentId, Appointment appointment);
        Task<Appointment> DeleteAppointment(int appointmentId);
        Task<Appointment> AddAppointment(Appointment appointment);
    }
}
