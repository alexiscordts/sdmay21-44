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

namespace InpatientTherapySchedulingProgramTests.ServiceTests
{
    [TestClass]
    public class UserServiceTests
    {
        private List<User> _testUsers;
        private CoreDbContext _testContext;
        private UserService _testService;

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
                _testUsers.Add(ObjectExtensions.Copy(newUser));
                _testContext.Add(newUser);
                _testContext.SaveChanges();
            }

            _testService = new UserService(_testContext);
        }

        [TestMethod]
        public async Task GetAllReturnsCorrectType()
        {
            var allUsers = await _testService.GetAllUsers();
            
            allUsers.Should().BeOfType<List<User>>();
        }

        [TestMethod]
        public async Task GetAllReturnsCorrectNumberOfUsers()
        {
            var allUsers = await _testService.GetAllUsers();
            List<User> listOfUsers = (List<User>)allUsers;

            listOfUsers.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task GetAllReturnsCorrectListOfUsers()
        {
            var allUsers = await _testService.GetAllUsers();
            List<User> listOfUsers = (List<User>)allUsers;

            for(var i = 0; i < 10; i++)
            {
                _testUsers.Contains(listOfUsers[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetUserByUserIdReturnsCorrectType()
        {
            var returnUser = await _testService.GetUserById(_testUsers[0].Uid);

            returnUser.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task GetUserByUserIdReturnsCorrectUser()
        {
            var returnUser = await _testService.GetUserById(_testUsers[0].Uid);

            returnUser.Should().Be(_testUsers[0]);
        }

        [TestMethod]
        public async Task GetUserByUserIdReturnsNullIfUserDoesNotExist()
        {
            var returnUser = await _testService.GetUserById(-1);

            returnUser.Should().BeNull();
        }

        [TestMethod]
        public async Task GetUserByUsernameReturnsCorrectType()
        {
            var returnUser = await _testService.GetUserByUsername(_testUsers[0].Username);

            returnUser.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task GetUserByUsernameReturnsCorrectUser()
        {
            var returnUser = await _testService.GetUserByUsername(_testUsers[0].Username);

            returnUser.Should().Be(_testUsers[0]);
        }

        [TestMethod]
        public async Task GetUserByUsernameReturnsNullIfUserDoesNotExist()
        {
            var returnUser = await _testService.GetUserByUsername("-1");

            returnUser.Should().BeNull();
        }

        public async Task AddUserReturnsCorrectType()
        {
            var newUser = ModelFakes.UserFake.Generate();

            var returnUser = await _testService.AddUser(newUser);

            returnUser.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task AddUserIncreasesCountOfUsers()
        {
            var newUser = ModelFakes.UserFake.Generate();

            await _testService.AddUser(newUser);

            var allUsers = await _testService.GetAllUsers();
            List<User> listOfUsers = (List<User>)allUsers;

            listOfUsers.Count.Should().Be(11);
        }

        [TestMethod]
        public async Task AddUserCorrectlyAddsUserToDatabase()
        {
            var newUser = ModelFakes.UserFake.Generate();

            await _testService.AddUser(newUser);

            var returnUser = await _testService.GetUserById(newUser.Uid);

            returnUser.Should().Be(newUser);
        }

        [TestMethod]
        public async Task AddUserWithExistingIdThrowsError()
        {
            int existingId = _testUsers[0].Uid;
            var newUser = ModelFakes.UserFake.Generate();
            newUser.Uid = existingId;

            await _testService.Invoking(s => s.AddUser(newUser)).Should().ThrowAsync<UserIdAlreadyExistException>();
        }

        [TestMethod]
        public async Task AddUserWithExistingUsernameThrowsError()
        {
            string existingUsername = _testUsers[0].Username;
            var newUser = ModelFakes.UserFake.Generate();
            newUser.Username = existingUsername;

            await _testService.Invoking(s => s.AddUser(newUser)).Should().ThrowAsync<UsernameAlreadyExistException>();
        }

        [TestMethod]
        public async Task DeleteUserDecrementsCount()
        {
            await _testService.DeleteUser(_testUsers[0].Uid);

            var allUsers = await _testService.GetAllUsers();
            List<User> listOfUsers = (List<User>)allUsers;

            listOfUsers.Count.Should().Be(9);
        }

        [TestMethod]
        public async Task DeleteUserRemovesUserFromDatabase()
        {
            await _testService.DeleteUser(_testUsers[0].Uid);

            var allUsers = await _testService.GetAllUsers();
            List<User> listOfUsers = (List<User>)allUsers;

            listOfUsers.Contains(_testUsers[0]).Should().BeFalse();
        }

        [TestMethod]
        public async Task DeleteUserReturnsCorrectUser()
        {
            var returnUser = await _testService.DeleteUser(_testUsers[0].Uid);

            bool isEqual = returnUser.Equals(_testUsers[0]);

            returnUser.Should().Be(_testUsers[0]);
        }

        [TestMethod]
        public async Task DeleteUserThatDoesExistReturnsNull() {
            var returnUser = await _testService.DeleteUser(-1);

            returnUser.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateUserReturnsCorrectType()
        {
            var returnUser = await _testService.UpdateUser(_testUsers[0].Uid, _testUsers[0]);

            returnUser.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task UpdateUserReturnsCorrectUser()
        {
            var returnUser = await _testService.UpdateUser(_testUsers[0].Uid, _testUsers[0]);

            returnUser.Should().Be(_testUsers[0]);
        }

        [TestMethod]
        public async Task UpdateUserWithAlteredDataUpdatesCorrectlyInDatabase()
        {
            var newUsername = ModelFakes.UserFake.Generate().Username;
            _testUsers[0].Username = newUsername;

            await _testService.UpdateUser(_testUsers[0].Uid, _testUsers[0]);

            var returnUser = await _testService.GetUserById(_testUsers[0].Uid);

            returnUser.Username.Should().Be(newUsername);
        }

        [TestMethod]
        public async Task UpdateUserWithNonMatchingIdsThrowsError()
        {
            await _testService.Invoking(s => s.UpdateUser(_testUsers[1].Uid, _testUsers[0])).Should().ThrowAsync<UserIdsDoNotMatchException>();
        }

        [TestMethod]
        public async Task UpdateUserWithNonExistingUserThrowsError()
        {
            var user = ModelFakes.UserFake.Generate();

            await _testService.Invoking(s => s.UpdateUser(user.Uid, user)).Should().ThrowAsync<UserDoesNotExistException>();
        }
    }
}
