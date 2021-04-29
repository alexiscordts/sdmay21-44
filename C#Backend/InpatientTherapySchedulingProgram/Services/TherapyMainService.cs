using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

namespace InpatientTherapySchedulingProgram.Services
{
    public class TherapyMainService : ITherapyMainService
    {
        private readonly CoreDbContext _context;

        public TherapyMainService(CoreDbContext context)
        {
            _context = context;
        }

        public async Task<TherapyMain> AddTherapyMain(TherapyMain therapyMain)
        {
            if (await TherapyMainExistsByType(therapyMain.Type))
            {
                throw new TherapyMainTypeAlreadyExistsException();
            }
            if (await TherapyMainExistsByAbbreviation(therapyMain.Abbreviation)) 
            {
                throw new TherapyMainTypeAbbreviationAlreadyExistsException();
            }

            therapyMain.Active = true;

            _context.TherapyMain.Add(therapyMain);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return therapyMain;
        }

        public async Task<TherapyMain> DeleteTherapyMain(string type)
        {
            var therapyMain = await _context.TherapyMain.FindAsync(type);

            if (therapyMain == null)
            {
                return null;
            }

            therapyMain.Active = false;

            var local = _context.TherapyMain.Local.FirstOrDefault(t => t.Type.Equals(type));

            _context.Entry(local).State = EntityState.Detached;

            _context.Entry(therapyMain).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return therapyMain;
        }

        public async Task<IEnumerable<TherapyMain>> GetAllTherapyMains()
        {
            return await _context.TherapyMain.Where(t => t.Active).ToListAsync();
        }

        public async Task<TherapyMain> GetTherapyMainByAbbreviation(string abbreviation)
        {
            return await _context.TherapyMain.FirstOrDefaultAsync(t => t.Abbreviation.Equals(abbreviation) && t.Active);
        }

        public async Task<TherapyMain> GetTherapyMainByType(string type)
        {
            return await _context.TherapyMain.FirstOrDefaultAsync(t => t.Type.Equals(type) && t.Active);
        }

        public async Task<TherapyMain> UpdateTherapyMain(string type, TherapyMain therapyMain)
        {
            if (!type.Equals(therapyMain.Type))
            {
                throw new TherapyMainTypesDoNotMatchException();
            }
            if (!await TherapyMainExistsByType(type))
            {
                throw new TherapyMainDoesNotExistException();
            }

            var local = await _context.TherapyMain.FindAsync(type);

            if (local == null)
            {
                throw new TherapyMainDoesNotExistException();
            }

            UpdateNonNullAndNonEmptyFields(local, therapyMain);

            _context.Entry(local).State = EntityState.Modified;

            //_context.Entry(therapyMain).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return therapyMain;
        }

        private void UpdateNonNullAndNonEmptyFields(TherapyMain local, TherapyMain therapyMain)
        {
            foreach (PropertyInfo prop in typeof(TherapyMain).GetProperties())
            {
                if (prop.GetValue(therapyMain) != null && (prop.PropertyType != typeof(string) || !prop.GetValue(therapyMain).Equals("")))
                {
                    prop.SetValue(local, prop.GetValue(therapyMain));
                }
            }
        }

        private async Task<bool> TherapyMainExistsByType(string type)
        {
            return await _context.TherapyMain.FindAsync(type) != null;
        }

        private async Task<bool> TherapyMainExistsByAbbreviation(string abbreviation)
        {
            return await _context.TherapyMain.FirstOrDefaultAsync(t => t.Abbreviation.Equals(abbreviation)) != null;
        }
    }
}
