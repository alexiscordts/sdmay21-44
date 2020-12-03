using InpatientTherapySchedulingProgram.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    interface IUserService
    {
        Task<ActionResult<IEnumerable<User>>> GetUser();
        Task<ActionResult<User>> GetUser(int id);
        Task<IActionResult> PutUser(int id, User user);
        Task<ActionResult<User>> PostUser(User user);
        Task<ActionResult<User>> DeleteUser(int id);
        bool UserExists(int id);
        bool UserExists(string username, string password);
    }
}
