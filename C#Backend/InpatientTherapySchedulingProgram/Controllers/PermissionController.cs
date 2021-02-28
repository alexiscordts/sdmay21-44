using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgram.Exceptions.PermissionExceptions;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;

namespace InpatientTherapySchedulingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        // GET: api/Permission
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permission>>> GetPermission()
        {
            var allPermissions = await _permissionService.GetAllPermissions();

            return Ok(allPermissions);
        }

        // GET: api/Permission/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<Permission>> GetPermission(int userId)
        {
            var permission = await _permissionService.GetPermissionByUserId(userId);

            if (permission == null)
            {
                return NotFound();
            }

            return Ok(permission);
        }

        // POST: api/Permission
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Permission>> PostPermission(Permission permission)
        {
            try
            {
                await _permissionService.AddPermission(permission);
            }
            catch (PermissionRoleIsInvalidException e)
            {
                return BadRequest(e);
            }
            catch (UserDoesNotExistException e)
            {
                return BadRequest(e);
            }
            catch (PermissionAlreadyExistsException e)
            {
                return Conflict(e);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetPermission", new { id = permission.UserId, permission.Role }, permission);
        }

        // DELETE: api/Permission/5
        [HttpDelete("{userId}")]
        public async Task<ActionResult<Permission>> DeletePermission(int userId)
        {
            Permission permission;

            try
            {
                permission = await _permissionService.DeletePermission(userId);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            if (permission is null)
            {
                return NotFound(); 
            }

            return Ok(permission);
        }
    }
}
