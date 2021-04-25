using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;

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
            var user = await _context.User.FindAsync(id);

            if(user == null)
            {
                return null;
            }

            user.Active = false;

            var local = _context.Set<User>()
                .Local
                .FirstOrDefault(u => u.UserId == user.UserId);

            _context.Entry(local).State = EntityState.Detached;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return user;
        }
        
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.User.Where(u => u.Active).ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.UserId == id && u.Active);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Username.Equals(username) && u.Active);
        }

        public async Task<User> AddUser(User user)
        {
            if (await UserExists(user.Username))
            {
                throw new UsernameAlreadyExistException();
            }

            user.Password = Hash(user.Password);
            user.Active = true;

            _context.User.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw;
            }

            return user;
        }

        public async Task<User> LoginUser(User user)
        {
            string hashPassword = Hash(user.Password);

            return await _context.User.FirstOrDefaultAsync(u => u.Username.Equals(user.Username)
             && u.Password.Equals(Hash(user.Password))
             && u.Active);
        }

        public async Task<User> UpdateUser(int id, User user)
        {
            if(id != user.UserId)
            {
                throw new UserIdsDoNotMatchException();
            }
            if(!await UserExists(id))
            {
                throw new UserDoesNotExistException();
            }

            if (user.Password != null && !user.Password.Equals(""))
            {
                user.Password = Hash(user.Password);
            }

            var local = _context.Set<User>()
                .Local
                .FirstOrDefault(u => u.UserId == user.UserId);

            updateNonNullAndNonEmptyFields(local, user);

            _context.Entry(local).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return user;
        }

        private void updateNonNullAndNonEmptyFields(User local, User user)
        {
            foreach(PropertyInfo prop in typeof(User).GetProperties())
            {
                if (prop.GetValue(user) != null && (prop.PropertyType != typeof(string) || !prop.GetValue(user).Equals("")))
                {
                    prop.SetValue(local, prop.GetValue(user));
                }
            }
        }

        private async Task<bool> UserExists(int id)
        {
            return await _context.User.FindAsync(id) != null;
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Username.Equals(username)) != null;
        }

        private static string Hash(string psw)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(psw));
            return string.Concat(hash.Select(s => s.ToString("x2")));
        }
    }
}
