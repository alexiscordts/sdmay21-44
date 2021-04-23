using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using InpatientTherapySchedulingProgram.Exceptions.RoomException;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services
{
    public class RoomService : IRoomService
    {
        private readonly CoreDbContext _context;

        public RoomService(CoreDbContext context)
        {
            _context = context;
        }

        public async Task<Room> AddRoom(Room room)
        {
            if (await RoomExists(room)) {
                throw new RoomAlreadyExistsException();
            }

            _context.Room.Add(room);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                throw;
            }

            return room;
        }

        public async Task<Room> DeleteRoom(Room room)
        {
            var curRoom = await _context.Room.FindAsync(room.Number, room.LocationId);

            if (curRoom == null) {
                return null;
            }

            _context.Room.Remove(curRoom);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                throw;
            }

            return curRoom;
        }

        public async Task<Room> UpdateRoom(int number, Room room)
        {
            if (number != room.Number) {
                throw new RoomNumbersDoNotMatchException();
            }
            if (!await RoomExists(room)) {
                throw new RoomDoesNotExistException();
            }

            var local = _context.Set<Room>().Local.FirstOrDefault(r => r.Number == room.Number);

            _context.Entry(local).State = EntityState.Detached;
            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                throw;
            }

            return room;
        }

        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _context.Room.Where(r => r.Active).ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetAllRoomsByLocationId(int location_id)
        {
            return await _context.Room.Where(r => r.LocationId == location_id && r.Active).ToListAsync();
        }

        private async Task<bool> RoomExists(Room room)
        {
            return await _context.Room.FirstOrDefaultAsync(r => r.Number == room.Number && r.LocationId == room.LocationId && r.Active) != null;
        }

    }
}
