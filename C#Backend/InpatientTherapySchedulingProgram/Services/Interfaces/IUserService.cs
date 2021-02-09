using InpatientTherapySchedulingProgram.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByUsername(string username);
        Task<User> UpdateUser(int id, User user);
        Task<User> AddUser(User user);
        Task<User> DeleteUser(int id);
    }
}
