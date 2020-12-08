using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;

namespace InpatientTherapySchedulingProgram.Services
{
    public class UserService : IUserService
    {
        private readonly CoreDbContext _context;

        public UserService(CoreDbContext context)
        {
            _context = context;
        }

        public async Task<User> DeleteUser(int id)
        {
            if(!UserExists(id))
            {
                throw new UserDoesNotExistException();
            }

            var user = await _context.User.FindAsync(id);

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> AddUser(User user)
        {
            if (UserExists(user.Uid))
            {
                throw new UserIdAlreadyExistsException();
            }
            if (UserExists(user.Username))
            {
                throw new UsernameAlreadyExistsException();
            }
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUser(int id, User user)
        {
            if(id != user.Uid)
            {
                throw new UserIdsDoNotMatchException();
            }
            if(!UserExists(id))
            {
                throw new UserDoesNotExistException();
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(u => u.Uid == id);
        }

        private bool UserExists(string username)
        {
            return _context.User.Any(u => u.Username == username);
        }
    }
}
