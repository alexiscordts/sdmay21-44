using FluentAssertions;
using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Exceptions.PermissionExceptions;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgramTests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgramTests.ControllerTests
{
    [TestClass]
    public class PermissionControllerTests
    {
        private static List<Permission> _testPermissions;
        private Mock<IPermissionService> _fakePermissionService;
        private PermissionController _testPermissionController;
        
        [ClassInitialize()]
        public static void Setup(TestContext context)
        {
            _testPermissions = new List<Permission>();
            
            for (var i = 0; i < 10; i++)
            {
                var newPermission = ModelFakes.PermissionFake.Generate();
                _testPermissions.Add(newPermission);
            }
        }

        [TestInitialize]
        public void Initalize()
        {
            _fakePermissionService = new Mock<IPermissionService>();
            _fakePermissionService.SetupAllProperties();
            _fakePermissionService.Setup(s => s.GetAllPermissions()).ReturnsAsync(_testPermissions);
            _fakePermissionService.Setup(s => s.GetPermissionByUserId(It.IsAny<int>())).ReturnsAsync(_testPermissions[0]);
            _fakePermissionService.Setup(s => s.AddPermission(It.IsAny<Permission>())).ReturnsAsync(_testPermissions[0]);
            _fakePermissionService.Setup(s => s.DeletePermission(It.IsAny<int>())).ReturnsAsync(_testPermissions[0]);

            _testPermissionController = new PermissionController(_fakePermissionService.Object);
        }

        [TestMethod]
        public async Task ValidGetAllPermissionsReturnsOkResponse()
        {
            var response = await _testPermissionController.GetPermission();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllPermissionsReturnsType()
        {
            var response = await _testPermissionController.GetPermission();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<Permission>>();
        }

        [TestMethod]
        public async Task ValidGetPermissionReturnsOkResponse()
        {
            var response = await _testPermissionController.GetPermission(_testPermissions[0].UserId);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetPermissionReturnCorrectType()
        {
            var response = await _testPermissionController.GetPermission(_testPermissions[0].UserId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Permission>();
        }

        [TestMethod]
        public async Task NullGetPermissionReturnsNotFoundResponse()
        {
            _fakePermissionService.Setup(s => s.GetPermissionByUserId(It.IsAny<int>())).ReturnsAsync((Permission)null);
            
            var response = await _testPermissionController.GetPermission(-1);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPostPermissionReturnsCreatedAtActionResponse()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();

            var response = await _testPermissionController.PostPermission(newPermission);

            response.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostPermissionReturnsCorrectType()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();

            var response = await _testPermissionController.PostPermission(newPermission);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<Permission>();
        }

        [TestMethod]
        public async Task PermissionRoleIsInvalidExceptionPostPermissionReturnsBadRequestResponse()
        {
            _fakePermissionService.Setup(s => s.AddPermission(It.IsAny<Permission>())).ThrowsAsync(new PermissionRoleIsInvalidException());

            var newPermission = ModelFakes.PermissionFake.Generate();

            var response = await _testPermissionController.PostPermission(newPermission);

            response.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task UserDoesExistExceptionExceptionPostPermissionReturnsBadRequestResponse()
        {
            _fakePermissionService.Setup(s => s.AddPermission(It.IsAny<Permission>())).ThrowsAsync(new UserDoesNotExistException());

            var newPermission = ModelFakes.PermissionFake.Generate();

            var response = await _testPermissionController.PostPermission(newPermission);

            response.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task PermissionAlreadyExistsExceptionPostPermissionReturnsConflictResponse()
        {
            _fakePermissionService.Setup(s => s.AddPermission(It.IsAny<Permission>())).ThrowsAsync(new PermissionAlreadyExistsException());

            var newPermission = ModelFakes.PermissionFake.Generate();

            var response = await _testPermissionController.PostPermission(newPermission);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionPostPermissionThrowsException()
        {
            _fakePermissionService.Setup(s => s.AddPermission(It.IsAny<Permission>())).ThrowsAsync(new DbUpdateException());
            var newPermission = ModelFakes.PermissionFake.Generate();

            await _testPermissionController.Invoking(c => c.PostPermission(newPermission)).Should().ThrowAsync<DbUpdateException>();
        }

        [TestMethod]
        public async Task ValidDeletePermissionReturnsOkResponse()
        {
            var response = await _testPermissionController.DeletePermission(_testPermissions[0].UserId);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeletePermissionReturnsCorrectType()
        {
            var response = await _testPermissionController.DeletePermission(_testPermissions[0].UserId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Permission>();
        }

        [TestMethod]
        public async Task NullDeletePermissionReturnsNotFoundResponse()
        {
            _fakePermissionService.Setup(s => s.DeletePermission(It.IsAny<int>())).ReturnsAsync((Permission)null);

            var response = await _testPermissionController.DeletePermission(_testPermissions[0].UserId);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbConcurrencyUpdateExceptionDeletePermissionThrowsError()
        {
            _fakePermissionService.Setup(s => s.DeletePermission(It.IsAny<int>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testPermissionController.Invoking(c => c.DeletePermission(-1)).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }
}
