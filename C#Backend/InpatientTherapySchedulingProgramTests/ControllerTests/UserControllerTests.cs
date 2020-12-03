using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgramTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Web.Http.Results;
using Microsoft.AspNetCore.Mvc;
using NotFoundResult = Microsoft.AspNetCore.Mvc.NotFoundResult;

namespace InpatientTherapySchedulingProgramTests
{
    [TestClass]
    public class UserControllerTests
    {
        List<User> _testUsers;
        CoreDbContext _testContext;
        UserController _testController;

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
                _testUsers.Add(newUser);
                _testContext.Add(newUser);
                _testContext.SaveChanges();
            }

            _testController = new UserController(_testContext);
        }

        [TestMethod]
        public async Task GetAllReturnsCorrectType()
        {
            var allUsers = await _testController.GetUser();

            allUsers.Value.Should().BeOfType<List<User>>();
            
        }

        [TestMethod]
        public async Task GetSingleUserByUIDReturnsCorrectType()
        {
            var singleUser = await _testController.GetUser(_testUsers[0].Uid);

            singleUser.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task GetSingleUserByUIDReturnsCorrectUser()
        {
            var singleUser = await _testController.GetUser(_testUsers[0].Uid);
            var singleUserValue = singleUser.Value;

            singleUserValue.Should().Be(_testUsers[0]);
        }

        [TestMethod]
        public async Task GetSingleUserByUsernameReturnsCorrectType()
        {
            var singleUser = await _testController.GetUser(_testUsers[0].Username);

            singleUser.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task GetSingleUserByUsernameReturnsCorrectUser()
        {
            var singleUser = await _testController.GetUser(_testUsers[0].Username);
            var singleUserValue = singleUser.Value;

            singleUserValue.Should().Be(_testUsers[0]);
        }

        [TestMethod]
        public async Task GetAllReturnsCorrectAmountOfUsers()
        {
            var allUsers = await _testController.GetUser();
            List<User> listOfUsers = (List<User>)allUsers.Value;

            listOfUsers.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task AddUserIncreasesCountOfUsers()
        {
            var newUser = ModelFakes.UserFake.Generate();
            await _testController.PostUser(newUser);
            var allUsers = await _testController.GetUser();
            List<User> listOfUsers = (List<User>)allUsers.Value;

            listOfUsers.Count.Should().Be(11);
        }

        [TestMethod]
        public async Task AddUserExistsInDatabase()
        {
            var newUser = ModelFakes.UserFake.Generate();
            await _testController.PostUser(newUser);

            var singleUser = await _testController.GetUser(newUser.Uid);
            var singleUserValue = (User)singleUser.Value;

            singleUserValue.Should().Be(newUser);
        }

        [TestMethod]
        public async Task DeleteUserDecrementsCount()
        {
            await _testController.DeleteUser(_testUsers[0].Uid);
            var allUsers = await _testController.GetUser();
            List<User> listOfUsers = (List<User>)allUsers.Value;

            listOfUsers.Count.Should().Be(9);
        }

        [TestMethod]
        public async Task DeleteUserRemovesUserFromDatabase()
        {
            await _testController.DeleteUser(_testUsers[0].Uid);
            var notFound = await _testController.GetUser(_testUsers[0].Uid);
            var notFoundValue = notFound.Value;

            notFoundValue.Should().NotBe(_testUsers[0]);
        }

        [TestMethod]
        public async Task DeleteUserReturnsCorrectUser()
        {
            var deleteUser = await _testController.DeleteUser(_testUsers[0].Uid);
            var deleteUserValue = deleteUser.Value;

            deleteUserValue.Should().Be(_testUsers[0]);
        }

        [TestMethod]
        public async Task UserNotInDatabaseReturnsNotFound()
        {
            var getUser = await _testController.GetUser(-1);
            var getUserResult = getUser.Result;

            getUserResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task AlteringUserDataChangesDataInDatabase()
        {
            var originalUser = await _testController.GetUser(_testUsers[0].Uid);
            var originalUsernameValue = originalUser.Value.Username;
            var newUsername = ModelFakes.UserFake.Generate().Username;
            _testUsers[0].Username = newUsername;

            await _testController.PutUser(_testUsers[0].Uid, _testUsers[0]);

            var alteredUser = await _testController.GetUser(_testUsers[0].Uid);
            var alteredUserValue = alteredUser.Value;

            alteredUserValue.Username.Should().NotBe(originalUsernameValue);
            alteredUserValue.Username.Should().Be(newUsername);
        }

    }

}
