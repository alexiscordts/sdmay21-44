﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InpatientTherapySchedulingProgram.Models;
using Microsoft.AspNetCore.Authorization;
using InpatientTherapySchedulingProgram.Services;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;
using InpatientTherapySchedulingProgram.Services.Interfaces;

namespace InpatientTherapySchedulingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var allUsers = await _userService.GetAllUsers();

            return Ok(allUsers);
        }


        // GET: api/User/5
        [HttpGet("getUserByUserId/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("getUserByUsername/{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var user = await _userService.GetUserByUsername(username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/User/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            try
            {
                await _userService.UpdateUser(id, user);
            }
            catch (UserIdsDoNotMatchException e)
            {
                return BadRequest(e);
            }
            catch (UserDoesNotExistException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                await _userService.AddUser(user);
            }
            catch (UsernameAlreadyExistException e)
            {
                return Conflict(e);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginUser(User user)
        {
            var foundUser = await _userService.LoginUser(user);

            if (foundUser is null)
            {
                return NotFound();
            }

            return Ok(foundUser);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            User user;

            try
            {
                user = await _userService.DeleteUser(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
