using System.Collections.Generic;
using System.Threading.Tasks;
using InpatientTherapySchedulingProgram.Models;


namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllPatients();

        Task<Patient> GetPatientByPatientId(int pid);

        Task<Patient> UpdatePatient(int pid, Patient patient);

        Task<Patient> AddPatient(Patient patient);

        Task<Patient> DeletePatient(int pid);


    }
}
