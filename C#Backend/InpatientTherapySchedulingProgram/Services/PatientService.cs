using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using InpatientTherapySchedulingProgram.Exceptions.PatientExceptions;
using System.Reflection;

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
            if (await PatientExists(patient.PatientId)) {
                throw new PatientIdAlreadyExistException();
            }

            patient.Active = true;

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

        public async Task<Patient> DeletePatient(int patientId)
        {
            var patient = await _context.Patient.FindAsync(patientId);

            if (patient == null)
            {
                return null;
            }

            patient.Active = false;

            var local = _context.Set<Patient>()
                .Local
                .FirstOrDefault(p => p.PatientId == patient.PatientId);

            _context.Entry(local).State = EntityState.Detached;

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return patient;
        }

        public async Task<IEnumerable<Patient>> GetAllPatients()
        {
            return await _context.Patient.Where(p => p.Active).ToListAsync();
        }

        public async Task<Patient> GetPatientByPatientId(int patientId)
        {
            return await _context.Patient.FirstOrDefaultAsync(p => p.PatientId == patientId && p.Active);
        }

        public async Task<Patient> UpdatePatient(int patientId, Patient patient)
        {
            if (patientId != patient.PatientId)
            {
                throw new PatientPidsDoNotMatchException();
            }
            if (!await PatientExists(patientId))
            {
                throw new PatientDoesNotExistException();
            }

            var local = await _context.Patient.FindAsync(patientId);

            if (local is null)
            {
                throw new PatientDoesNotExistException();
            }

            UpdateNonNullAndNonEmptyFields(local, patient);

            /*var local = _context.Set<Patient>()
                .Local
                .FirstOrDefault(p => p.PatientId == patient.PatientId);*/

            _context.Entry(local).State = EntityState.Modified;

            //_context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return patient;
        }

        private void UpdateNonNullAndNonEmptyFields(Patient local, Patient patient)
        {
            foreach (PropertyInfo prop in typeof(Patient).GetProperties())
            {
                if (prop.GetValue(patient) != null && (prop.PropertyType != typeof(string) || !prop.GetValue(patient).Equals("")))
                {
                    prop.SetValue(local, prop.GetValue(patient));
                }
            }
        }

        private async Task<bool> PatientExists(int patientId)
        {
            return await _context.Patient.FirstOrDefaultAsync(p => p.PatientId == patientId && p.Active) != null;
        }
    }
}
