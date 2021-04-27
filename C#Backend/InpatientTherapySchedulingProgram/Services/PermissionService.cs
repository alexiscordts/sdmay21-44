using InpatientTherapySchedulingProgram.Exceptions.PermissionExceptions;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly CoreDbContext _context;
        private readonly HashSet<string> _validRoles;

        public PermissionService(CoreDbContext context)
        {
            _context = context;
            _validRoles = new HashSet<string>
            {
                "therapist",
                "nurse",
                "admin",
                "physician"
            };
        }

        public async Task<Permission> AddPermission(Permission permission)
        {
            if (!_validRoles.Contains(permission.Role))
            {
                throw new PermissionRoleIsInvalidException("Role: " + permission.Role + " is not valid");
            }
            if (!await UserExists(permission.UserId))
            {
                throw new UserDoesNotExistException("User with id: " + permission.UserId + " does not exist");
            }
            if (await PermissionExists(permission))
            {
                throw new PermissionAlreadyExistsException();
            }

            _context.Permission.Add(permission);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return permission;
        }

        public async Task<Permission> DeletePermission(int userId)
        {
            var permission = await _context.Permission.FirstOrDefaultAsync(p => p.UserId == userId);

            if (permission == null)
            {
                return null;
            }

            _context.Permission.Remove(permission);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return permission;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissions()
        {
            return await _context.Permission.ToListAsync();
        }

        public async Task<Permission> GetPermissionByUserId(int userId)
        {
            return await _context.Permission.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        private async Task<bool> UserExists(int userId)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.UserId == userId && u.Active) != null;
        }

        private async Task<bool> PermissionExists(Permission permission)
        {
            return await _context.Permission.FirstOrDefaultAsync(p => p.UserId == permission.UserId && 
                p.Role.Equals(permission.Role)) != null;
        }
    }
}
