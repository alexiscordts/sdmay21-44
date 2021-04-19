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

        //Add Room
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

        //Delete Room
        public async Task<Room> DeleteRoom(Room room)
        {
            var curRoom = await _context.Room.FindAsync(room);

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

        //Update Room
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

        //Get All Rooms
        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _context.Room.ToListAsync();
        }

        //Get All Rooms by Location
        public async Task<IEnumerable<Room>> GetAllRoomsByLocationId(int location_id)
        {
            return await _context.Room.Where(r => r.LocationId == location_id).ToListAsync();
        }

        private async Task<bool> RoomExists(Room room)
        {
            return await _context.Room.FindAsync(room) != null;
        }

    }
}
