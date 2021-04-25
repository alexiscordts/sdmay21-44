﻿using InpatientTherapySchedulingProgram.Exceptions.TherapistActivityExceptions;
using InpatientTherapySchedulingProgram.Exceptions.TherapistEventExceptions;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services
{
    public class TherapistEventService : ITherapistEventService
    {
        private readonly CoreDbContext _context;

        public TherapistEventService(CoreDbContext _context)
        {
            this._context = _context;
        }

        /// <summary>
        /// Adds a therapist event to the database
        /// </summary>
        /// <param name="therapistEvent">The therapist event to be added</param>
        /// <returns>The therapist event that was added to the database</returns>
        /// <exception cref="TherapistEventEventIdAlreadyExistsException">Thrown if therapist event event id already exist</exception>
        /// <exception cref="TherapistActivityDoesNotExistException">Thrown if therapist event activity name does not exist</exception>
        /// <exception cref="UserDoesNotExistException">Thrown if therapist event therapist id does not exist</exception>
        /// <exception cref="DbUpdateException">Thrown if an exception occured while trying to save changes to database</exception>
        public async Task<TherapistEvent> AddTherapistEvent(TherapistEvent therapistEvent)
        {
            if (await TherapistEventExistsById(therapistEvent.EventId))
            {
                throw new TherapistEventEventIdAlreadyExistsException();
            }
            if (!await UserExists(therapistEvent.TherapistId))
            {
                throw new UserDoesNotExistException();
            }
            if (!await IsTherapist(therapistEvent.TherapistId))
            {
                throw new UserIsNotATherapistException();
            }
            if (therapistEvent.EndTime < therapistEvent.StartTime)
            {
                throw new TherapistEventCannotEndBeforeStartTimeException();
            }

            therapistEvent.Active = true;

            _context.TherapistEvent.Add(therapistEvent);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            
            return therapistEvent;
        }

        /// <summary>
        /// Deletes a therapist event from the database
        /// </summary>
        /// <param name="eventId">The event id of the therapist event to delete</param>
        /// <returns>The therapist event id that was deleted</returns>
        /// <exception cref="DbUpdateConcurrencyException">Thrown if an exception occured while trying to save changes to database</exception>
        public async Task<TherapistEvent> DeleteTherapistEvent(int eventId)
        {
            var therapistEvent = await _context.TherapistEvent.FindAsync(eventId);

            if (therapistEvent is null)
            {
                return null;
            }

            therapistEvent.Active = false;

            var local = _context.TherapistEvent.Local.FirstOrDefault(t => t.EventId == eventId);

            _context.Entry(local).State = EntityState.Detached;

            _context.Entry(therapistEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return therapistEvent;
        }

        /// <summary>
        /// Gets all therapist events within given date range of parameter therapist event
        /// </summary>
        /// <param name="therapistEvent">The therapist event to compare against existing therapist event records</param>
        /// <returns>All therapist event records within date range</returns>
        public async Task<IEnumerable<TherapistEvent>> GetAllTherapistEvents(TherapistEvent therapistEvent)
        {
            return await _context.TherapistEvent
                .Where(d => d.StartTime >= therapistEvent.StartTime && d.EndTime <= therapistEvent.EndTime && d.Active)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all therapist events within given date range of parameter therapist event
        /// and match therapist ids
        /// </summary>
        /// <param name="therapistEvent">The therapist event to compare against existing therapist event records</param>
        /// <returns>All therapist events records within date range and match therapist ids</returns>
        /// <exception cref="UserDoesNotExistException">Thrown if therapist id does not match a user</exception>
        public async Task<IEnumerable<TherapistEvent>> GetAllTherapistEventsByTherapistId(TherapistEvent therapistEvent)
        {
            if (!await UserExists(therapistEvent.TherapistId))
            {
                throw new UserDoesNotExistException();
            }

            return await _context.TherapistEvent
                .Where(d => d.StartTime >= therapistEvent.StartTime && d.EndTime <= therapistEvent.EndTime && d.TherapistId == therapistEvent.TherapistId && d.Active)
                .ToListAsync();
        }

        /// <summary>
        /// Updates a therapist event in the database
        /// </summary>
        /// <param name="eventId">The event id of the therapist event to update</param>
        /// <param name="therapistEvent">The therapist event to update</param>
        /// <returns>The updated therapist event</returns>
        /// <exception cref="TherapistEventEventIdsDoNotMatchException">Thrown if event ids do not match</exception>
        /// <exception cref="TherapistEventDoesNotExistException">Thrown if therapist event does not exist</exception>
        /// <exception cref="TherapistActivityDoesNotExistException">Thrown if activity name does not match a activity name</exception>
        /// <exception cref="UserDoesNotExistException">Thrown if therapist id does not match a user</exception>
        /// <exception cref="DbUpdateConcurrencyException">Thrown if an exception occured while trying to save changes to database</exception>
        public async Task<TherapistEvent> UpdateTherapistEvent(int eventId, TherapistEvent therapistEvent)
        {
            if (eventId != therapistEvent.EventId)
            {
                throw new TherapistEventEventIdsDoNotMatchException();
            }
            if (!await UserExists(therapistEvent.TherapistId))
            {
                throw new UserDoesNotExistException();
            }
            if (!await IsTherapist(therapistEvent.TherapistId))
            {
                throw new UserIsNotATherapistException();
            }
            if (therapistEvent.EndTime < therapistEvent.StartTime)
            {
                throw new TherapistEventCannotEndBeforeStartTimeException();
            }

            //var local = _context.TherapistEvent.Local.FirstOrDefault(t => t.EventId == eventId && t.Active);
            var local = await _context.TherapistEvent.FindAsync(eventId);

            if (local == null)
            {
                throw new TherapistEventDoesNotExistException();
            }

            _context.Entry(local).State = EntityState.Detached;

            _context.Entry(therapistEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return therapistEvent;
        }

        /// <summary>
        /// Checks to see if therapist event exists
        /// </summary>
        /// <param name="eventId">Event id of a therapist event</param>
        /// <returns>Whether or not a record exists in database that matches the event id</returns>
        private async Task<bool> TherapistEventExistsById(int eventId)
        {
            var therapistEvent = await _context.TherapistEvent.FindAsync(eventId);

            return await _context.TherapistEvent.FirstOrDefaultAsync(t => t.EventId == eventId && t.Active) != null;
        }

        /// <summary>
        /// Checks to see if therapist event therapist id exists
        /// </summary>
        /// <param name="therapistId">Therapist id to check against User database</param>
        /// <returns>Whether or not a record exists in the database that matches the therapist event id</returns>
        private async Task<bool> UserExists(int? therapistId)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == therapistId && u.Active == true);

            if (user is null || !user.Active)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks to see if user is a therapist
        /// </summary>
        /// <param name="therapistId">Therapist id</param>
        /// <returns></returns>
        private async Task<bool> IsTherapist(int? therapistId)
        {
            return await _context.Permission.FirstOrDefaultAsync(p => p.UserId == therapistId && p.Role.Equals("therapist")) != null;
        }
    }
}
