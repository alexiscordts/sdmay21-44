using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgramTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;

namespace InpatientTherapySchedulingProgramTests.ControllerTests
{
    [TestClass]
    public class UserControllerTests
    {
        private static List<User> _testUsers;
        private Mock<IUserService> _fakeUserService;
        private UserController _testUserController;

        [ClassInitialize()]
        public static void ClassSetup(TestContext context)
        {
            _testUsers = new List<User>();

            for (var i = 0; i < 10; i++)
            {
                var user = ModelFakes.UserFake.Generate();
                user.UserId = i;
                _testUsers.Add(user);
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            _fakeUserService = new Mock<IUserService>();
            _fakeUserService.SetupAllProperties();
            _fakeUserService.Setup(s => s.GetAllUsers()).ReturnsAsync(_testUsers);
            _fakeUserService.Setup(s => s.GetUserById(It.IsAny<int>())).ReturnsAsync(_testUsers[0]);
            _fakeUserService.Setup(s => s.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(_testUsers[0]);
            _fakeUserService.Setup(s => s.LoginUser(It.IsAny<User>())).ReturnsAsync(_testUsers[0]);
            _fakeUserService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).ReturnsAsync(_testUsers[0]);
            _fakeUserService.Setup(s => s.AddUser(It.IsAny<User>())).ReturnsAsync(_testUsers[0]);
            _fakeUserService.Setup(s => s.DeleteUser(It.IsAny<int>())).ReturnsAsync(_testUsers[0]);

            _testUserController = new UserController(_fakeUserService.Object);
        }

        [TestMethod]
        public async Task ValidGetAllUsersReturnsOkResponse()
        {
            var response = await _testUserController.GetUser();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllReturnsCorrectType()
        {
            var response = await _testUserController.GetUser();
            var responseResult = response.Result as OkObjectResult;
            
            responseResult.Value.Should().BeOfType<List<User>>();
        }

        [TestMethod]
        public async Task ValidGetUserByUserIdReturnsOkResponse()
        {
            var response = await _testUserController.GetUser(_testUsers[0].UserId);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetUserByUserIdReturnsCorrectType()
        {
            var response = await _testUserController.GetUser(_testUsers[0].UserId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task GetNonExistingUserByUserIdReturnsNotFoundResponse()
        {
            _fakeUserService.Setup(s => s.GetUserById(It.IsAny<int>())).ReturnsAsync((User)null);

            var response = await _testUserController.GetUser(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidGetUserByUsernameReturnsOkResponse()
        {
            var response = await _testUserController.GetUser(_testUsers[0].Username);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetUserByUsernameReturnsCorrectType()
        {
            var response = await _testUserController.GetUser(_testUsers[0].Username);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task GetNonExistingUserByUsernameReturnsNotFoundResponse()
        {
            _fakeUserService.Setup(s => s.GetUserByUsername(It.IsAny<string>())).ReturnsAsync((User)null);

            var response = await _testUserController.GetUser("-1");
            var responseResult = response.Result;
            
            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidLoginReturnsOkResponse()
        {
            var response = await _testUserController.LoginUser(_testUsers[0]);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidLoginReturnsCorrectType()
        {
            var response = await _testUserController.LoginUser(_testUsers[0]);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task NullLoginReturnsNotFoundResponse()
        {
            _fakeUserService.Setup(s => s.LoginUser(It.IsAny<User>())).ReturnsAsync((User)null);

            var fakeUser = ModelFakes.UserFake.Generate();

            var response = await _testUserController.LoginUser(fakeUser);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutUserReturnsNoContentResponse()
        {
            var response = await _testUserController.PutUser(_testUsers[0].UserId, _testUsers[0]);
            
            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task NonMatchingUserIdPutUserReturnsBadRequestResponse()
        {
            _fakeUserService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).ThrowsAsync(new UserIdsDoNotMatchException());

            var response = await _testUserController.PutUser(-1, _testUsers[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingUserPostUserReturnsNotFoundResponse()
        {
            _fakeUserService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).ThrowsAsync(new UserDoesNotExistException());

            var response = await _testUserController.PutUser(_testUsers[0].UserId, new User());

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionPutUserShouldThrowDbUpdateConcurrencyException()
        {
            _fakeUserService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testUserController.Invoking(c => c.PutUser(_testUsers[0].UserId, _testUsers[0])).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

        [TestMethod]
        public async Task ValidPostUserReturnsCreatedAtActionResponse()
        {
            var response = await _testUserController.PostUser(_testUsers[0]);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostUserReturnsCorrectType()
        {
            var response = await _testUserController.PostUser(_testUsers[0]);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task ExistingUsernamePostUserReturnsConflictResponse()
        {
            _fakeUserService.Setup(s => s.AddUser(It.IsAny<User>())).ThrowsAsync(new UsernameAlreadyExistException());

            var response = await _testUserController.PostUser(new User());
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionPostUserThrowsDbUpdateException()
        {
            _fakeUserService.Setup(s => s.AddUser(It.IsAny<User>())).ThrowsAsync(new DbUpdateException());

            await _testUserController.Invoking(c => c.PostUser(new User())).Should().ThrowAsync<DbUpdateException>();
        }

        [TestMethod]
        public async Task ValidDeleteUserReturnsOkResponse()
        {
            var response = await _testUserController.DeleteUser(_testUsers[0].UserId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteUserReturnsCorrectType()
        {
            var response = await _testUserController.DeleteUser(_testUsers[0].UserId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task NonExistingUserDeleteUserReturnsNotFoundResponse()
        {
            _fakeUserService.Setup(s => s.DeleteUser(It.IsAny<int>())).ReturnsAsync((User)null);

            var response = await _testUserController.DeleteUser(-1);
            var responseResult = response.Result;
            
            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionDeleteUserThrowsDbUpdateConcurrencyException()
        {
            _fakeUserService.Setup(s => s.DeleteUser(It.IsAny<int>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testUserController.Invoking(c => c.DeleteUser(-1)).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }

}
