using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services;
using InpatientTherapySchedulingProgramTests.Fakes;
using System.Threading.Tasks;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;
using System;
using System.Linq;

namespace InpatientTherapySchedulingProgramTests.ServiceTests
{
    [TestClass]
    public class UserServiceTests
    {
        private List<User> _testUsers;
        private CoreDbContext _testContext;
        private UserService _testUserService;
        private User _nonActiveUser;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "UserDatabase")
                .Options;
            _testUsers = new List<User>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            for(var i = 0; i < 10; i++)
            {
                var newUser = ModelFakes.UserFake.Generate();
                _testContext.Add(newUser);
                _testContext.SaveChanges();
                _testUsers.Add(ObjectExtensions.Copy(_testContext.User.FirstOrDefault(u => u.Username.Equals(newUser.Username))));
            }

            _nonActiveUser = ModelFakes.UserFake.Generate();
            _nonActiveUser.Active = false;
            _testContext.Add(_nonActiveUser);
            _testContext.SaveChanges();
            _testUsers.Add(ObjectExtensions.Copy(_testContext.User.FirstOrDefault(u => u.Username.Equals(_nonActiveUser.Username))));

            _testUserService = new UserService(_testContext);
        }

        [TestMethod]
        public async Task GetAllUsersReturnsCorrectType()
        {
            var allUsers = await _testUserService.GetAllUsers();
            
            allUsers.Should().BeOfType<List<User>>();
        }

        [TestMethod]
        public async Task GetAllUsersReturnsCorrectNumberOfUsers()
        {
            var allUsers = await _testUserService.GetAllUsers();
            List<User> listOfUsers = (List<User>)allUsers;

            listOfUsers.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task GetAllUsersReturnsCorrectListOfUsers()
        {
            var allUsers = await _testUserService.GetAllUsers();
            List<User> listOfUsers = (List<User>)allUsers;

            for(var i = 0; i < 10; i++)
            {
                _testUsers.Contains(listOfUsers[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetUserByUserIdReturnsCorrectType()
        {
            var returnUser = await _testUserService.GetUserById(_testUsers[0].UserId);

            returnUser.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task GetUserByUserIdReturnsCorrectUser()
        {
            var returnUser = await _testUserService.GetUserById(_testUsers[0].UserId);

            returnUser.Should().Be(_testUsers[0]);
        }

        [TestMethod]
        public async Task GetUserByUserIdReturnsNullIfUserDoesNotExist()
        {
            var returnUser = await _testUserService.GetUserById(-1);

            returnUser.Should().BeNull();
        }

        [TestMethod]
        public async Task GetUserByUsernameReturnsCorrectType()
        {
            var returnUser = await _testUserService.GetUserByUsername(_testUsers[0].Username);

            returnUser.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task GetUserByUsernameReturnsCorrectUser()
        {
            var returnUser = await _testUserService.GetUserByUsername(_testUsers[0].Username);

            returnUser.Should().Be(_testUsers[0]);
        }

        [TestMethod]
        public async Task GetUserByUsernameReturnsNullIfUserDoesNotExist()
        {
            var returnUser = await _testUserService.GetUserByUsername("-1");

            returnUser.Should().BeNull();
        }

        public async Task AddUserReturnsCorrectType()
        {
            var newUser = ModelFakes.UserFake.Generate();

            var returnUser = await _testUserService.AddUser(newUser);

            returnUser.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task AddUserIncreasesCountOfUsers()
        {
            var newUser = ModelFakes.UserFake.Generate();

            await _testUserService.AddUser(newUser);

            var allUsers = await _testUserService.GetAllUsers();
            List<User> listOfUsers = (List<User>)allUsers;

            listOfUsers.Count.Should().Be(11);
        }

        [TestMethod]
        public async Task AddUserCorrectlyAddsUserToDatabase()
        {
            var newUser = ModelFakes.UserFake.Generate();

            await _testUserService.AddUser(newUser);

            var returnUser = await _testUserService.GetUserById(newUser.UserId);

            returnUser.Should().Be(newUser);
        }

        [TestMethod]
        public async Task AddUserWithExistingUsernameThrowsUsernameAlreadyExistException()
        {
            string existingUsername = _testUsers[0].Username;
            var newUser = ModelFakes.UserFake.Generate();
            newUser.Username = existingUsername;

            await _testUserService.Invoking(s => s.AddUser(newUser)).Should().ThrowAsync<UsernameAlreadyExistException>();
        }

        [TestMethod]
        public async Task ValidLoginUserReturnsUser()
        {
            var newUser = ModelFakes.UserFake.Generate();
            string pswBeforeHash = newUser.Password;

            await _testUserService.AddUser(newUser);

            newUser.Password = pswBeforeHash;

            var user = await _testUserService.LoginUser(newUser);

            user.Should().NotBeNull();
        }

        [TestMethod]
        public async Task ValidLoginUserReturnsCorrectUser()
        {
            var newUser = ModelFakes.UserFake.Generate();
            string pswBeforeHash = newUser.Password;

            await _testUserService.AddUser(newUser);

            newUser.Password = pswBeforeHash;

            var user = await _testUserService.LoginUser(newUser);

            user.Should().Be(newUser);
        }

        [TestMethod]
        public async Task NonActiveLoginUserReturnsNull()
        {
            var user = await _testUserService.LoginUser(_nonActiveUser);

            user.Should().BeNull();
        }

        [TestMethod]
        public async Task NonExistingLoginUserReturnsNull()
        {
            var fakeUser = ModelFakes.UserFake.Generate();

            var user = await _testUserService.LoginUser(fakeUser);

            user.Should().BeNull();
        }

        [TestMethod]
        public async Task DeleteUserReturnsCorrectType()
        {
            var returnUser = await _testUserService.DeleteUser(_testUsers[0].UserId);

            returnUser.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task DeleteUserReturnsCorrectUser()
        {
            var returnUser = await _testUserService.DeleteUser(_testUsers[0].UserId);

            returnUser.Should().Be(_testUsers[0]);
        }

        [TestMethod]
        public async Task DeleteUserDecrementsCount()
        {
            await _testUserService.DeleteUser(_testUsers[0].UserId);

            var allUsers = await _testUserService.GetAllUsers();
            List<User> listOfUsers = (List<User>)allUsers;

            listOfUsers.Count.Should().Be(9);
        }

        [TestMethod]
        public async Task DeleteUserRemovesUserFromDatabase()
        {
            await _testUserService.DeleteUser(_testUsers[0].UserId);

            var allUsers = await _testUserService.GetAllUsers();
            List<User> listOfUsers = (List<User>)allUsers;

            listOfUsers.Contains(_testUsers[0]).Should().BeFalse();
        }

        [TestMethod]
        public async Task DeleteUserThatDoesExistReturnsNull() {
            var returnUser = await _testUserService.DeleteUser(-1);

            returnUser.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateUserReturnsCorrectType()
        {
            var returnUser = await _testUserService.UpdateUser(_testUsers[0].UserId, _testUsers[0]);

            returnUser.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task UpdateUserReturnsCorrectUser()
        {
            var returnUser = await _testUserService.UpdateUser(_testUsers[0].UserId, _testUsers[0]);

            returnUser.Should().Be(_testUsers[0]);
        }

        [TestMethod]
        public async Task UpdateUserWithAlteredDataUpdatesCorrectlyInDatabase()
        {
            var newUsername = ModelFakes.UserFake.Generate().Username;
            _testUsers[0].Username = newUsername;


            await _testUserService.UpdateUser(_testUsers[0].UserId, _testUsers[0]);

            var returnUser = await _testUserService.GetUserById(_testUsers[0].UserId);

            returnUser.Username.Should().Be(newUsername);
        }

        [TestMethod]
        public async Task UpdateUserWithNonMatchingIdsThrowsError()
        {
            await _testUserService.Invoking(s => s.UpdateUser(_testUsers[1].UserId, _testUsers[0])).Should().ThrowAsync<UserIdsDoNotMatchException>();
        }

        [TestMethod]
        public async Task UpdateUserWithNonExistingUserThrowsError()
        {
            var fakeUser = ModelFakes.UserFake.Generate();

            await _testUserService.Invoking(s => s.UpdateUser(fakeUser.UserId, fakeUser)).Should().ThrowAsync<UserDoesNotExistException>();
        }
    }
}
