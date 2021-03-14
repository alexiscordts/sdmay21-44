using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgram.Exceptions.LocationExceptions;

namespace InpatientTherapySchedulingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        // GET: api/Location
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocation()
        {
            var allLocations = await _locationService.GetAllLocations();

            return Ok(allLocations);
        }

        // GET: api/Location/name
        [HttpGet("name")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationNames()
        {
            var allLocationNames = await _locationService.GetAllLocationNames();

            return Ok(allLocationNames);
        }

        // GET: api/Location/5
        [HttpGet("{lid}")]
        public async Task<ActionResult<Location>> GetLocation(int lid)
        {
            var location = await _locationService.GetLocationByLocationId(lid);

            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        // GET: api/Location/Hopkins
        [HttpGet("{name}")]
        public async Task<ActionResult<Location>> GetLocation(string name)
        {
            var location = await _locationService.GetLocationByName(name);

            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        // PUT: api/Location/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{lid}")]
        public async Task<IActionResult> PutLocation(int lid, Location location)
        {
            try
            {
                await _locationService.UpdateLocation(lid, location);
            }
            catch (LocationIdsDoNotMatchException e)
            {
                return BadRequest(e);
            }
            catch (LocationDoesNotExistException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Location
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location)
        {
            try
            {
                await _locationService.AddLocation(location);
            }
            catch (LocationIdAlreadyExistsException e)
            {
                return Conflict(e);
            }
            catch (LocationNameAlreadyExistsException e)
            {
                return Conflict(e);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetLocation", new { id = location.LocationId }, location);
        }

        // DELETE: api/Location/5
        [HttpDelete("{lid}")]
        public async Task<ActionResult<Location>> DeleteLocation(int lid)
        {
            Location location;

            try
            {
                location = await _locationService.DeleteLocation(lid);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            if(location is null)
            {
                return NotFound();
            }

            return Ok(location);
        }
    }
}
