using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InpatientTherapySchedulingProgram.Models;


namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface IRoomService
    {
        //Add Room
        Task<Room> AddRoom(Room room);

        //Delete Room
        Task<Room> DeleteRoom(Room room);

        //Update Room
        Task<Room> UpdateRoom(int number, Room room);

        //Get All Rooms
        Task<IEnumerable<Room>> GetAllRooms();

        //Get All Rooms by Location
        Task<IEnumerable<Room>> GetAllRoomsByLocationId(int location_id);

    }
}
