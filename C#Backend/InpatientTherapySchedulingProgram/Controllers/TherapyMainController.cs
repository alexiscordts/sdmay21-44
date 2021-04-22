using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InpatientTherapySchedulingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapyMainController : ControllerBase
    {
        private readonly ITherapyMainService _therapyMainService;

        public TherapyMainController(ITherapyMainService therapyMainService)
        {
            _therapyMainService = therapyMainService;
        }

        // GET: api/TherapyMain
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TherapyMain>>> GetTherapyMain()
        {
            var allTherapyMains = await _therapyMainService.GetAllTherapyMains();

            return Ok(allTherapyMains);
        }

        // GET api/TherapyMain/getTherapyMain/"X Therapy"
        [HttpGet("getTherapyMainByType/{type}")]
        public async Task<ActionResult<TherapyMain>> GetTherapyMainByType(string type)
        {
            var therapyMain = await _therapyMainService.GetTherapyMainByType(type);

            if (therapyMain == null)
            {
                return NotFound();
            }

            return Ok(therapyMain);
        }

        // GET api/TherapyMain/getTherapyMainByAbbreviation/"X Therapy"
        [HttpGet("getTherapyMainByAbbreviation/{abbreviation}")]
        public async Task<ActionResult<TherapyMain>> GetTherapyMainByAbbreviation(string abbreviation)
        {
            var therapyMain = await _therapyMainService.GetTherapyMainByAbbreviation(abbreviation);

            if (therapyMain == null)
            {
                return NotFound();
            }

            return Ok(therapyMain);
        }

        // POST api/TherapyMain
        [HttpPost]
        public async Task<ActionResult<TherapyMain>> PostTherapyMain(TherapyMain therapyMain)
        {
            try
            {
                await _therapyMainService.AddTherapyMain(therapyMain);
            }
            catch (TherapyMainTypeAlreadyExistsException e)
            {
                return Conflict(e);
            }
            catch (TherapyMainTypeAbbreviationAlreadyExistsException e)
            {
                return Conflict(e);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetTherapyMain", new { id = therapyMain.Type }, therapyMain);
        }

        // PUT api/TherapyMain/"X therapy"
        [HttpPut("{type}")]
        public async Task<IActionResult> PutTherapyMain(string type, TherapyMain therapyMain)
        {
            try
            {
                await _therapyMainService.UpdateTherapyMain(type, therapyMain);
            }
            catch (TherapyMainTypesDoNotMatchException e)
            {
                return BadRequest(e);
            }
            catch (TherapyMainDoesNotExistException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // DELETE api/TherapyMain/"X therapy"
        [HttpDelete("{type}")]
        public async Task<ActionResult<TherapyMain>> DeleteTherapyMain(string type)
        {
            TherapyMain therapyMain;

            try
            {
                therapyMain = await _therapyMainService.DeleteTherapyMain(type);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            if(therapyMain == null)
            {
                return NotFound();
            }

            return Ok(therapyMain);
        }
    }
}
