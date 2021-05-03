using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Exceptions.RoomException;
using InpatientTherapySchedulingProgram.Services.Interfaces;


namespace InpatientTherapySchedulingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _service;

        public RoomController(IRoomService service)
        {
            _service = service;
        }


        // GET: api/room
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            var allRooms = await _service.GetAllRooms();

            return Ok(allRooms);
        }

        // GET: api/room/getRoomsByLocationId/1
        [HttpGet("getRoomsByLocationId/{locationId}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomsByLocationId(int locationId)
        {
            var allRoomByLocations = await _service.GetAllRoomsByLocationId(locationId);

            if (allRoomByLocations.Count() == 0) {
                return NotFound();
            }
           
            return Ok(allRoomByLocations);
        }

        // POST: api/room/223+{JSONObject}
        [HttpPut("{number}")]
        public async Task<IActionResult> PutRoom(int number, Room room)
        {
            try
            {
                await _service.UpdateRoom(number, room);
            }
            catch (RoomNumbersDoNotMatchException e)
            {
                return BadRequest(e);
            }
            catch (RoomDoesNotExistException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            try
            {
                await _service.AddRoom(room);
            }
            catch (RoomAlreadyExistsException e)
            {
                return Conflict(e);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetRooms", new { number = room.Number }, room);
        }

        [HttpPost("deleteRoom/")]
        public async Task<ActionResult<Room>> DeleteRoom(Room room)
        {
            try
            {
                room = await _service.DeleteRoom(room);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }
    }
}
