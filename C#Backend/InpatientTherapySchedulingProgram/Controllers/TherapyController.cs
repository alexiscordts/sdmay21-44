using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgram.Exceptions.TherapyExceptions;
using InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions;

namespace InpatientTherapySchedulingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapyController : ControllerBase
    {
        private readonly ITherapyService _service;

        public TherapyController(ITherapyService service)
        {
            _service = service;
        }

        // GET: api/Therapy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Therapy>>> GetTherapy()
        {
            var allTherapies = await _service.GetAllTherapies();

            return Ok(allTherapies);
        }

        // GET: api/Therapy/adl
        [HttpGet("adl")]
        public async Task<ActionResult<IEnumerable<Therapy>>> GetTherapyAdl()
        {
            var allAdls = await _service.GetAllAdls();

            return Ok(allAdls);
        }

        // GET: api/Therapy/type
        [HttpGet("type")]
        public async Task<ActionResult<IEnumerable<Therapy>>> GetTherapyType()
        {
            var allTypes = await _service.GetAllTypes();

            return Ok(allTypes);
        }

        // GET: api/Therapy/ovm
        [HttpGet("{adl}")]
        public async Task<ActionResult<Therapy>> GetTherapy(string adl)
        {
            var therapy = await _service.GetTherapyByAdl(adl);

            if (therapy == null)
            {
                return NotFound();
            }

            return Ok(therapy);
        }

        // GET: api/Therapy/abbreviation/ABRV
        [HttpGet("abbreviation/{abbreviation}")]
        public async Task<ActionResult<Therapy>> GetTherapyByAbbreviation(string abbreviation)
        {
            var therapy = await _service.GetTherapyByAbbreviation(abbreviation);

            if (therapy == null)
            {
                return NotFound();
            }

            return Ok(therapy);
        }

        // PUT: api/Therapy/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{adl}")]
        public async Task<IActionResult> PutTherapy(string adl, Therapy therapy)
        {
            try
            {
                await _service.UpdateTherapy(adl, therapy);
            }
            catch (TherapyAdlsDoNotMatchException e)
            {
                return BadRequest(e);
            }
            catch (TherapyMainDoesNotExistException e)
            {
                return BadRequest(e);
            }
            catch (TherapyDoesNotExistException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Therapy
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Therapy>> PostTherapy(Therapy therapy)
        {
            try
            {
                await _service.AddTherapy(therapy);
            }
            catch (TherapyAdlAlreadyExistException e)
            {
                return Conflict(e);
            }
            catch (TherapyAbbreviationAlreadyExistException e)
            {
                return Conflict(e);
            }
            catch (TherapyMainDoesNotExistException e)
            {
                return BadRequest(e);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetTherapy", new { id = therapy.Adl }, therapy);
        }

        // DELETE: api/Therapy/5
        [HttpDelete("{adl}")]
        public async Task<ActionResult<Therapy>> DeleteTherapy(string adl)
        {
            Therapy therapy;

            try
            {
                therapy = await _service.DeleteTherapy(adl);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            if (therapy == null)
            {
                return NotFound();
            }

            return Ok(therapy);
        }
    }
}
