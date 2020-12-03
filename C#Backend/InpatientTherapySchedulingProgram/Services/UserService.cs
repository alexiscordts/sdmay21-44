using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services
{
    public class UserService : IUserService
    {
        public Task<ActionResult<User>> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<User>> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<User>> PostUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> PutUser(int id, User user)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
