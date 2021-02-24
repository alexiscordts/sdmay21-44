﻿using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using InpatientTherapySchedulingProgram.Exceptions.LocationExceptions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InpatientTherapySchedulingProgram.Services
{
    public class LocationService : ILocationService
    {
        private readonly CoreDbContext _context;

        public LocationService(CoreDbContext context)
        {
            _context = context;
        }

        public async Task<Location> AddLocation(Location location)
        {
            if(await LocationExists(location.LocationId))
            {
                throw new LocationIdAlreadyExistsException();
            }
            if(await LocationExists(location.Name))
            {
                throw new LocationNameAlreadyExistsException();
            }

            _context.Location.Add(location);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw;
            }

            return location;
        }

        public async Task<Location> DeleteLocation(int id)
        {
            var location = await _context.Location.FindAsync(id);

            if (location == null)
            {
                return null;
            }

            _context.Location.Remove(location);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return location;
        }

        public async Task<IEnumerable<string>> GetAllLocationNames()
        {
            return await _context.Location.Select(l => l.Name).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetAllLocations()
        {
            return await _context.Location.ToListAsync();
        }

        public async Task<Location> GetLocationByLocationId(int id)
        {
            return await _context.Location.FindAsync(id);
        }

        public async Task<Location> GetLocationByName(string name)
        {
            return await _context.Location.FirstOrDefaultAsync(l => l.Name.Equals(name));
        }

        public async Task<Location> UpdateLocation(int id, Location location)
        {
            if(location.LocationId != id)
            {
                throw new LocationIdsDoNotMatchException();
            }
            if(!await LocationExists(location.LocationId))
            {
                throw new LocationDoesNotExistException();
            }

            var local = _context.Set<Location>()
                .Local
                .FirstOrDefault(l => l.LocationId == id);

            _context.Entry(local).State = EntityState.Detached;

            _context.Entry(location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return location;
        }

        private async Task<bool> LocationExists(int id)
        {
            return await _context.Location.FindAsync(id) != null;
        }

        private async Task<bool> LocationExists(string name)
        {
            return await _context.Location.FirstOrDefaultAsync(l => l.Name.Equals(name)) != null;
        }
    }
}