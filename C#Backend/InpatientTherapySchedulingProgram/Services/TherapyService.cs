using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgram.Exceptions.TherapyExceptions;
using InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InpatientTherapySchedulingProgram.Services
{
    public class TherapyService : ITherapyService
    {
        private readonly CoreDbContext _context;

        public TherapyService(CoreDbContext context)
        {
            _context = context;
        }

        public async Task<Therapy> AddTherapy(Therapy therapy)
        {
            if (await TherapyExistsByAdl(therapy.Adl))
            {
                throw new TherapyAdlAlreadyExistException();
            }
            if (await TherapyExistsByAbbreviation(therapy.Abbreviation))
            {
                throw new TherapyAbbreviationAlreadyExistException();
            }
            if (!await TherapyMainExists(therapy.Type))
            {
                throw new TherapyMainDoesNotExistException();
            }

            therapy.Active = true;

            _context.Therapy.Add(therapy);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return therapy;
        }

        public async Task<Therapy> DeleteTherapy(string adl)
        {
            var therapy = await _context.Therapy.FindAsync(adl);

            if (therapy == null)
            {
                return null;
            }

            therapy.Active = false;

            var local = _context.Therapy.Local.FirstOrDefault(t => t.Adl.Equals(adl));

            _context.Entry(local).State = EntityState.Detached;

            _context.Entry(therapy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return therapy;
        }

        public async Task<IEnumerable<string>> GetAllAdls()
        {
            return await _context.Therapy.Where(t => t.Active).Select(t => t.Adl).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<Therapy>> GetAllTherapies()
        {
            return await _context.Therapy.Where(t => t.Active).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllTypes()
        {
            return await _context.Therapy.Where(t => t.Active).Select(t => t.Type).Distinct().ToListAsync();
        }

        public async Task<Therapy> GetTherapyByAdl(string adl)
        {
            return await _context.Therapy.FirstOrDefaultAsync(t => t.Adl.Equals(adl) && t.Active);
        }

        public async Task<Therapy> GetTherapyByAbbreviation(string abbreviation)
        {
            return await _context.Therapy.FirstOrDefaultAsync(t => t.Abbreviation.Equals(abbreviation) && t.Active);
        }

        public async Task<Therapy> UpdateTherapy(string adl, Therapy therapy)
        {
            if (therapy.Adl != adl)
            {
                throw new TherapyAdlsDoNotMatchException();
            }
            if (!await TherapyExistsByAdl(therapy.Adl))
            {
                throw new TherapyDoesNotExistException();
            }
            if (!await TherapyMainExists(therapy.Type))
            {
                throw new TherapyMainDoesNotExistException();
            }

            var local = await _context.Therapy.FindAsync(adl);

            if (local == null)
            {
                throw new TherapyDoesNotExistException();
            }

            UpdateNonNullAndNonEmptyFields(local, therapy);

            _context.Entry(local).State = EntityState.Modified;

            //_context.Entry(therapy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return therapy;
        }

        private void UpdateNonNullAndNonEmptyFields(Therapy local, Therapy therapy)
        {
            foreach (PropertyInfo prop in typeof(Therapy).GetProperties())
            {
                if (prop.GetValue(therapy) != null && (prop.PropertyType != typeof(string) || !prop.GetValue(therapy).Equals("")))
                {
                    prop.SetValue(local, prop.GetValue(therapy));
                }
            }
        }

        private async Task<bool> TherapyExistsByAdl(string adl)
        {
            return await _context.Therapy.FirstOrDefaultAsync(t => t.Adl.Equals(adl) && t.Active) != null;
        }

        private async Task<bool> TherapyExistsByAbbreviation(string abbreviation)
        {
            return await _context.Therapy.FirstOrDefaultAsync(t => t.Abbreviation.Equals(abbreviation) && t.Active) != null;
        }

        private async Task<bool> TherapyMainExists(string therapyMainType)
        {
            return await _context.TherapyMain.FirstOrDefaultAsync(t => t.Type.Equals(therapyMainType) && t.Active) != null;
        }
    }
}
