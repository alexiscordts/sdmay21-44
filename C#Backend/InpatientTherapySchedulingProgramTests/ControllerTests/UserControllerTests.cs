using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgramTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NotFoundResult = Microsoft.AspNetCore.Mvc.NotFoundResult;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;

namespace InpatientTherapySchedulingProgramTests
{
    [TestClass]
    public class UserControllerTests
    {
        List<User> _testUsers;
        Mock<IUserService> _fakeService;
        UserController _testController;

        [TestInitialize]
        public void Initialize()
        {
            _testUsers = new List<User>();
            for(var i = 0; i < 10; i++)
            {
                var user = ModelFakes.UserFake.Generate();
                _testUsers.Add(user);
            }

            _fakeService = new Mock<IUserService>();
            _fakeService.SetupAllProperties();
            _fakeService.Setup(s => s.GetAllUsers()).ReturnsAsync(_testUsers);
            _fakeService.Setup(s => s.GetUserById(It.IsAny<int>())).ReturnsAsync(_testUsers[0]);
            _fakeService.Setup(s => s.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(_testUsers[0]);
            _fakeService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).ReturnsAsync(_testUsers[0]);
            _fakeService.Setup(s => s.AddUser(It.IsAny<User>())).ReturnsAsync(_testUsers[0]);
            _fakeService.Setup(s => s.DeleteUser(It.IsAny<int>())).ReturnsAsync(_testUsers[0]);

            _testController = new UserController(_fakeService.Object);
        }

        [TestMethod]
        public async Task ValidGetAllReturnsOkResponse()
        {
            var response = await _testController.GetUser();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllReturnsCorrectType()
        {
            var response = await _testController.GetUser();
            var responseResult = response.Result as OkObjectResult;
            
            responseResult.Value.Should().BeOfType<List<User>>();
        }

        [TestMethod]
        public async Task ValidGetUserByUIDReturnsOkResponse()
        {
            var response = await _testController.GetUser(_testUsers[0].Uid);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetUserByUIDReturnsCorrectType()
        {
            var response = await _testController.GetUser(_testUsers[0].Uid);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task GetNonExistingUserByUIDReturnsNotFound()
        {
            _fakeService.Setup(s => s.GetUserById(It.IsAny<int>())).ReturnsAsync((User)null);

            var response = await _testController.GetUser(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidGetUserByUsernameReturnsOkResponse()
        {
            var response = await _testController.GetUser(_testUsers[0].Username);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetUserByUsernameReturnsCorrectType()
        {
            var response = await _testController.GetUser(_testUsers[0].Username);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task GetNonExistingUserByUsernameReturnsNotFound()
        {
            _fakeService.Setup(s => s.GetUserByUsername(It.IsAny<string>())).ReturnsAsync((User)null);

            var response = await _testController.GetUser("-1");
            var responseResult = response.Result;
            
            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPostUserReturnsNoContentResponse()
        {
            var response = await _testController.PutUser(_testUsers[0].Uid, _testUsers[0]);
            
            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task NonMatchingUserIdPostUserReturnsBadRequest()
        {
            _fakeService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).ThrowsAsync(new UserIdsDoNotMatchException());

            var response = await _testController.PutUser(-1, _testUsers[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingUserPostUserReturnsBadRequest()
        {
            _fakeService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).ThrowsAsync(new UserDoesNotExistException());

            var response = await _testController.PutUser(_testUsers[0].Uid, new User());

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionPostUserShouldThrowError()
        {
            _fakeService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testController.Invoking(c => c.PutUser(_testUsers[0].Uid, _testUsers[0])).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

        [TestMethod]
        public async Task ValidPostUserReturnsCreatedAtActionResponse()
        {
            var response = await _testController.PostUser(_testUsers[0]);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostUserReturnsCorrectType()
        {
            var response = await _testController.PostUser(_testUsers[0]);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task ExistingUIDPostUserReturnsConflict()
        {
            _fakeService.Setup(s => s.AddUser(It.IsAny<User>())).ThrowsAsync(new UserIdAlreadyExistsException());

            var response = await _testController.PostUser(new User());
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task ExistingUsernamePostUserReturnsConflict()
        {
            _fakeService.Setup(s => s.AddUser(It.IsAny<User>())).ThrowsAsync(new UsernameAlreadyExistsException());

            var response = await _testController.PostUser(new User());
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionPostUserThrowsError()
        {
            _fakeService.Setup(s => s.AddUser(It.IsAny<User>())).ThrowsAsync(new DbUpdateException());

            await _testController.Invoking(c => c.PostUser(new User())).Should().ThrowAsync<DbUpdateException>();
        }

        [TestMethod]
        public async Task ValidDeleteUserReturnsOkResponse()
        {
            var response = await _testController.DeleteUser(_testUsers[0].Uid);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteUserReturnsCorrectType()
        {
            var response = await _testController.DeleteUser(_testUsers[0].Uid);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task NonExistingDeleteUserReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.DeleteUser(It.IsAny<int>())).ReturnsAsync((User)null);

            var response = await _testController.DeleteUser(-1);
            var responseResult = response.Result;
            
            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionDeleteUserThrowsError()
        {
            _fakeService.Setup(s => s.DeleteUser(It.IsAny<int>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testController.Invoking(c => c.DeleteUser(-1)).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

        /*[TestMethod]
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
        */
    }

}
