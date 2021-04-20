using InpatientTherapySchedulingProgram.Exceptions.HoursWorkedExceptions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services
{
    public class HoursWorkedService : IHoursWorkedService
    {
        private readonly CoreDbContext _context;

        public HoursWorkedService(CoreDbContext context)
        {
            _context = context;
        }

        public async Task<HoursWorked> DeleteHoursWorked(int hoursWorkedId)
        {
            var hoursWorked = await _context.HoursWorked.FindAsync(hoursWorkedId);

            if (hoursWorked == null)
            {
                return null;
            }

            //_context.HoursWorked.Remove(hoursWorked);
            hoursWorked.Active = false;

            var local = _context.Set<HoursWorked>()
                .Local
                .FirstOrDefault(u => u.HoursWorkedId == hoursWorked.HoursWorkedId);

            _context.Entry(local).State = EntityState.Detached;

            _context.Entry(hoursWorked).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return hoursWorked;
        }

        public async Task<HoursWorked> GetHoursWorkedById(int hoursWorkedId)
        {
            return await _context.HoursWorked.FindAsync(hoursWorkedId);
        }

        
        public async Task<HoursWorked> GetHoursWorkedByUserId(int userId)
        {
            return await _context.HoursWorked.FirstOrDefaultAsync(h => h.UserId == userId && h.Active);
        }

        public async Task<HoursWorked> AddHoursWorked(HoursWorked hoursWorked)
        {
            hoursWorked.Active = true;

            _context.HoursWorked.Add(hoursWorked);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return hoursWorked;
        }

        public async Task<HoursWorked> UpdateHoursWorked(int hoursWorkedId, HoursWorked hoursWorked)
        {
            if (hoursWorkedId != hoursWorked.HoursWorkedId)
            {
                throw new HoursWorkedIdsDoNotMatchException();
            }
            
            if (!await HoursWorkedExists(hoursWorkedId))
            {
                throw new HoursWorkedDoesNotExistException();
            }

            var local = _context.Set<HoursWorked>()
                .Local
                .FirstOrDefault(h => h.UserId == hoursWorked.UserId);

            _context.Entry(local).State = EntityState.Detached;

            _context.Entry(hoursWorked).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return hoursWorked;
        }

        private async Task<bool> HoursWorkedExists(int hoursWorkedId)
        {
            return _context.HoursWorked.Any(h => h.HoursWorkedId == hoursWorkedId && h.Active);
        }
    }
}

