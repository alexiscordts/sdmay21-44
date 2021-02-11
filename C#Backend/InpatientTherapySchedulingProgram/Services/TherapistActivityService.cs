using InpatientTherapySchedulingProgram.Exceptions.TherapistActivityExceptions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services
{
    public class TherapistActivityService : ITherapistActivityService
    {
        private readonly CoreDbContext _context;

        public TherapistActivityService(CoreDbContext context)
        {
            _context = context;
        }

        public async Task<TherapistActivity> AddTherapistActivity(TherapistActivity therapistActivity)
        {
            if(await TherapistActivityExistsByName(therapistActivity.Name))
            {
                throw new TherapistActivityAlreadyExistsException();
            }

            _context.TherapistActivity.Add(therapistActivity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw;
            }

            return therapistActivity;
        }

        public async Task<TherapistActivity> DeleteTherapistActivity(string name)
        {
            var therapistActivity = await _context.TherapistActivity.FindAsync(name);

            if(therapistActivity == null)
            {
                return null;
            }

            _context.TherapistActivity.Remove(therapistActivity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return therapistActivity;
        }

        public async Task<IEnumerable<TherapistActivity>> GetAllTherapistActivities()
        {
            return await _context.TherapistActivity.ToListAsync();
        }

        public async Task<TherapistActivity> GetTherapistActivityByName(string name)
        {
            return await _context.TherapistActivity.FindAsync(name);
        }

        public async Task<TherapistActivity> UpdateTherapistActivity(string name, TherapistActivity therapistActivity)
        {
            if(name != therapistActivity.Name)
            {
                throw new TherapistActivityNamesDoNotMatchException();
            }
            if(!await TherapistActivityExistsByName(name))
            {
                throw new TherapistActivityDoesNotExistException();
            }

            var local = _context.Set<TherapistActivity>()
                .Local
                .FirstOrDefault(t => t.Name == name);

            _context.Entry(local).State = EntityState.Detached;

            _context.Entry(therapistActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return therapistActivity;
        }
        
        private async Task<bool> TherapistActivityExistsByName(string name)
        {
            return await _context.TherapistActivity.FindAsync(name) != null;
        }
    }
}
