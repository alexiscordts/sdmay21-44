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

namespace InpatientTherapySchedulingProgramTests
{
    [TestClass]
    public class UserControllerTests
    {
        private static List<User> _testUsers;
        private Mock<IUserService> _fakeService;
        private UserController _testController;

        [ClassInitialize()]
        public static void ClassSetup(TestContext context)
        {
            _testUsers = new List<User>();

            for (var i = 0; i < 10; i++)
            {
                var user = ModelFakes.UserFake.Generate();
                _testUsers.Add(user);
            }
        }

        [TestInitialize]
        public void Initialize()
        {
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

        /*[TestMethod]
        public async Task ValidGetUserByUserIdReturnsOkResponse()
        {
            var response = await _testController.GetUser(_testUsers[0].UserId);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetUserByUserIdReturnsCorrectType()
        {
            var response = await _testController.GetUser(_testUsers[0].UserId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task GetNonExistingUserByUserIdReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.GetUserById(It.IsAny<int>())).ReturnsAsync((User)null);

            var response = await _testController.GetUser(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }*/

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
        public async Task GetNonExistingUserByUsernameReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.GetUserByUsername(It.IsAny<string>())).ReturnsAsync((User)null);

            var response = await _testController.GetUser("-1");
            var responseResult = response.Result;
            
            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutUserReturnsNoContentResponse()
        {
            var response = await _testController.PutUser(_testUsers[0].UserId, _testUsers[0]);
            
            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task NonMatchingUserIdPutUserReturnsBadRequest()
        {
            _fakeService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).ThrowsAsync(new UserIdsDoNotMatchException());

            var response = await _testController.PutUser(-1, _testUsers[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingUserPostUserReturnsNotFound()
        {
            _fakeService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).ThrowsAsync(new UserDoesNotExistException());

            var response = await _testController.PutUser(_testUsers[0].UserId, new User());

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionPutUserShouldThrowDbUpdateConcurrencyException()
        {
            _fakeService.Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<User>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testController.Invoking(c => c.PutUser(_testUsers[0].UserId, _testUsers[0])).Should().ThrowAsync<DbUpdateConcurrencyException>();
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
        public async Task ExistingUserIdPostUserReturnsConflictResponse()
        {
            _fakeService.Setup(s => s.AddUser(It.IsAny<User>())).ThrowsAsync(new UserIdAlreadyExistException());

            var response = await _testController.PostUser(new User());
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task ExistingUsernamePostUserReturnsConflictResponse()
        {
            _fakeService.Setup(s => s.AddUser(It.IsAny<User>())).ThrowsAsync(new UsernameAlreadyExistException());

            var response = await _testController.PostUser(new User());
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionPostUserThrowsDbUpdateException()
        {
            _fakeService.Setup(s => s.AddUser(It.IsAny<User>())).ThrowsAsync(new DbUpdateException());

            await _testController.Invoking(c => c.PostUser(new User())).Should().ThrowAsync<DbUpdateException>();
        }

        [TestMethod]
        public async Task ValidDeleteUserReturnsOkResponse()
        {
            var response = await _testController.DeleteUser(_testUsers[0].UserId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteUserReturnsCorrectType()
        {
            var response = await _testController.DeleteUser(_testUsers[0].UserId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task NonExistingUserDeleteUserReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.DeleteUser(It.IsAny<int>())).ReturnsAsync((User)null);

            var response = await _testController.DeleteUser(-1);
            var responseResult = response.Result;
            
            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionDeleteUserThrowsDbUpdateConcurrencyException()
        {
            _fakeService.Setup(s => s.DeleteUser(It.IsAny<int>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testController.Invoking(c => c.DeleteUser(-1)).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }

}
