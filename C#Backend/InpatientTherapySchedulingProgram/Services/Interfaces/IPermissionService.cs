using InpatientTherapySchedulingProgram.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<Permission>> GetAllPermissions();
        Task<string> GetRoleOfUser(int userId);
        Task<Permission> AddPermission(Permission permission);
        Task<Permission> DeletePermission(int userId);
    }
}
