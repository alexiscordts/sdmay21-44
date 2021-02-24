using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using InpatientTherapySchedulingProgram.Exceptions.PatientExceptions;

namespace InpatientTherapySchedulingProgram.Services
{
    public class PatientService : IPatientService
    {
        private readonly CoreDbContext _context;

        public PatientService(CoreDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> AddPatient(Patient patient)
        {
            if (await PatientExists(patient.Pid)) {
                throw new PidAlreadyExistsException();
            }

            _context.Patient.Add(patient);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return patient;
        }

        public Task<Patient> DeletePatient(int pid)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Patient>> GetAllPatients()
        {
            throw new System.NotImplementedException();
        }

        public Task<Patient> GetPatientByPid(int pid)
        {
            throw new System.NotImplementedException();
        }

        public Task<Patient> UpdatePatient(int pid, Patient patient)
        {
            throw new System.NotImplementedException();
        }

        private async Task<bool> PatientExists(int pid) 
        {
            return await _context.Patient.FindAsync(pid) != null;
        }
    }
}
