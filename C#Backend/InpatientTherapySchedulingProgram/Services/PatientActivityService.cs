using InpatientTherapySchedulingProgram.Exceptions.PatientActivityExceptions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services
{
    public class PatientActivityService : IPatientActivity

    {

        private readonly CoreDbContext _context;

        public PatientActivityService(CoreDbContext context)
        {
            _context = context;
        }

        public async Task<PatientActivity> AddPatientActivity(PatientActivity patientActivity)
        {
            if (await PatientActivityExists(patientActivity.ActivityName))
            {
                throw new PatientActivityNameAlreadyExistsException();
            }

            _context.PatientActivity.Add(patientActivity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return patientActivity;
        }

        public async Task<PatientActivity> DeletePatientActivity(string activityName)
        {
            var patientActivity = await _context.PatientActivity.FindAsync(activityName);

            if(patientActivity == null)
            {
                return null;
            }

            _context.PatientActivity.Remove(patientActivity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return patientActivity;
        }

        public async Task<IEnumerable<PatientActivity>> GetAllPatientActivity()
        {
            return await _context.PatientActivity.ToListAsync();
        }

        public async Task<PatientActivity> GetPatientActivityByName(string activityName)
        {
            return await _context.PatientActivity.FirstOrDefaultAsync(a => a.ActivityName.Equals(activityName));
        }

         public async Task<bool> PatientActivityExists(string name)
        {
            return await _context.PatientActivity.FindAsync(name) != null;
        }
    }
}
