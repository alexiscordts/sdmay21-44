using InpatientTherapySchedulingProgram.Exceptions.PermissionExceptions;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly CoreDbContext _context;
        HashSet<string> _validRoles;

        public PermissionService(CoreDbContext context)
        {
            _context = context;
            _validRoles = new HashSet<string>
            {
                "therapist",
                "nurse",
                "admin"
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
                throw new UserDoesNotExistException("User with id: " + permission.UserId + " does not exists");
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
            var permission = await _context.Permission.FindAsync(userId);

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

        public async Task<string> GetRoleOfUser(int userId)
        {
            return await _context.Permission.Where(p => p.UserId == userId)
                .Select(p => p.Role).SingleOrDefaultAsync();
        }

        private async Task<bool> UserExists(int id)
        {
            return await _context.User.FindAsync(id) != null;
        }

        private async Task<bool> PermissionExists(Permission permission)
        {
            return await _context.Permission.FirstOrDefaultAsync(p => p.UserId == permission.UserId && 
                p.Role.Equals(permission.Role)) != null;
        }
    }
}
