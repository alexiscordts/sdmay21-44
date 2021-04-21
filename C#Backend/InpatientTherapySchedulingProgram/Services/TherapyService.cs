using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgram.Exceptions.TherapyExceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            if(await TherapyExistsByAdl(therapy.Adl))
            {
                throw new TherapyAdlAlreadyExistException();
            }
            if(await TherapyExistsByAbbreviation(therapy.Abbreviation))
            {
                throw new TherapyAbbreviationAlreadyExistException();
            }

            therapy.Active = true;

            _context.Therapy.Add(therapy);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw;
            }

            return therapy;
        }

        public async Task<Therapy> DeleteTherapy(string adl)
        {
            var therapy = await _context.Therapy.FindAsync(adl);

            if(therapy == null)
            {
                return null;
            }

            _context.Therapy.Remove(therapy);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return therapy;
        }

        public async Task<IEnumerable<string>> GetAllAdls()
        {
            return await _context.Therapy.Select(t => t.Adl).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<Therapy>> GetAllTherapies()
        {
            return await _context.Therapy.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllTypes()
        {
            return await _context.Therapy.Select(t => t.Type).Distinct().ToListAsync();
        }

        public async Task<Therapy> GetTherapyByAdl(string adl)
        {
            return await _context.Therapy.FindAsync(adl);
        }

        public async Task<Therapy> GetTherapyByAbbreviation(string abbreviation)
        {
            return await _context.Therapy.FirstOrDefaultAsync(t => t.Abbreviation == abbreviation);
        }

        public async Task<Therapy> UpdateTherapy(string adl, Therapy therapy)
        {
            if(therapy.Adl != adl)
            {
                throw new TherapyAdlsDoNotMatchException();
            }
            if(!await TherapyExistsByAdl(therapy.Adl))
            {
                throw new TherapyDoesNotExistException();
            }

            var local = _context.Set<Therapy>()
                .Local
                .FirstOrDefault(t => t.Adl == adl);

            _context.Entry(local).State = EntityState.Detached;

            _context.Entry(therapy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return therapy;
        }

        private async Task<bool> TherapyExistsByAdl(string adl)
        {
            return await _context.Therapy.FindAsync(adl) != null;
        }

        private async Task<bool> TherapyExistsByAbbreviation(string abbreviation)
        {
            return await _context.Therapy.FirstOrDefaultAsync(t => t.Abbreviation.Equals(abbreviation)) != null;
        }
    }
}
