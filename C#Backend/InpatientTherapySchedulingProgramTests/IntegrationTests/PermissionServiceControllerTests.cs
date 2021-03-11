using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using InpatientTherapySchedulingProgram.Services;
using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgramTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InpatientTherapySchedulingProgramTests.IntegrationTests
{
    [TestClass]
    public class PermissionServiceControllerTests
    {
        private List<Permission> _testPermissions;
        private List<User> _testUsers;
        private User _userWithNoPermissions;
        private CoreDbContext _testContext;
        private PermissionService _testPermissionService;
        private PermissionController _testPermissionController;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "PermissionDatabase")
                .Options;
            _testPermissions = new List<Permission>();
            _testUsers = new List<User>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            _userWithNoPermissions = ModelFakes.UserFake.Generate();
            _testUsers.Add(ObjectExtensions.Copy(_userWithNoPermissions));
            _testContext.Add(_userWithNoPermissions);
            _testContext.SaveChanges();

            for (var i = 0; i < 10; i++)
            {
                var newUser = ModelFakes.UserFake.Generate();
                _testUsers.Add(ObjectExtensions.Copy(newUser));
                _testContext.Add(newUser);
                _testContext.SaveChanges();

                var newPermission = ModelFakes.PermissionFake.Generate();
                _testPermissions.Add(ObjectExtensions.Copy(newPermission));
                _testContext.Add(newPermission);
                _testContext.SaveChanges();
            }

            _testPermissionService = new PermissionService(_testContext);
            _testPermissionController = new PermissionController(_testPermissionService);
        }

        [TestMethod]
        public async Task ValidGetAllPermissionsReturnsOkResponse()
        {
            var response = await _testPermissionController.GetPermission();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task  ValidGetAllPermissionsReturnsCorrectType()
        {
            var response = await _testPermissionController.GetPermission();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<Permission>>();
        }

        [TestMethod]
        public async Task ValidGetAllPermissionsReturnsCorrectCountOfPermissions()
        {
            var response = await _testPermissionController.GetPermission();
            var responseResult = response.Result as OkObjectResult;
            List<Permission> listOfPermissions = (List<Permission>)responseResult.Value;

            listOfPermissions.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task ValidGetAllPermissionsReturnsCorrectPermissions()
        {
            var response = await _testPermissionController.GetPermission();
            var responseResult = response.Result as OkObjectResult;
            List<Permission> listOfPermissions = (List<Permission>)responseResult.Value;

            for (var i = 0; i < 10; i++)
            {
                _testPermissions.Contains(listOfPermissions[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetPermissionByUserIdReturnsOkResponse()
        {
            var response = await _testPermissionController.GetPermission(_testPermissions[0].UserId);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetPermissionByUserIdReturnsCorrectType()
        {
            var response = await _testPermissionController.GetPermission(_testPermissions[0].UserId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Permission>();
        }

        [TestMethod]
        public async Task ValidGetPermissionByUserIdReturnsPermission()
        {
            var response = await _testPermissionController.GetPermission(_testPermissions[0].UserId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testPermissions[0]);
        }

        [TestMethod]
        public async Task NonExistingPermissionGetPermissionByUserIdReturnsNotFoundResponse()
        {
            var response = await _testPermissionController.GetPermission(-1);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPostPermissionReturnsCreatedAtActionResponse()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = _userWithNoPermissions.UserId;

            var response = await _testPermissionController.PostPermission(newPermission);

            response.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostPermissionReturnsType()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = _userWithNoPermissions.UserId;

            var response = await _testPermissionController.PostPermission(newPermission);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<Permission>();
        }

        [TestMethod]
        public async Task ValidPostPermissionReturnsCorrectPermission()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = _userWithNoPermissions.UserId;

            var response = await _testPermissionController.PostPermission(newPermission);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().Be(newPermission);
        }

        [TestMethod]
        public async Task ValidPostPermissionCorrectlyAddsPermissionToDatabase()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = _userWithNoPermissions.UserId;

            await _testPermissionController.PostPermission(newPermission);

            var response = await _testPermissionController.GetPermission(newPermission.UserId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(newPermission);
        }

        [TestMethod]
        public async Task InvalidRolePostPermissionReturnsBadRequestResponse()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = _userWithNoPermissions.UserId;
            newPermission.Role = "-1";

            var response = await _testPermissionController.PostPermission(newPermission);

            response.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task InvalidRolePostPermissionDoesNotAddPermissionToDatabase()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = _userWithNoPermissions.UserId;
            newPermission.Role = "-1";

            await _testPermissionController.PostPermission(newPermission);

            var response = await _testPermissionController.GetPermission(newPermission.UserId);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task NonExistingUserPostPermissionReturnsBadRequestResponse()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = -1;

            var response = await _testPermissionController.PostPermission(newPermission);

            response.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingUserPostPermissionDoesNotAddPermissionToDatabase()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = - 1;

            await _testPermissionController.PostPermission(newPermission);

            var response = await _testPermissionController.GetPermission(newPermission.UserId);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ExistingPermissionPostPermissionReturnsBadRequestResponse()
        {
            var newPermission = _testPermissions[0];

            var response = await _testPermissionController.PostPermission(newPermission);

            response.Result.Should().BeOfType<BadRequestObjectResult>();
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

            responseResult.Value.Should().Be(_testPermissions[0]);
        }

        [TestMethod]
        public async Task ValidDeletePermissionCorrectlyRemovesPermissionFromDatabase()
        {
            await _testPermissionController.DeletePermission(_testPermissions[0].UserId);

            var response = await _testPermissionController.GetPermission(_testPermissions[0].UserId);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task NonExistingPermissionDeletePermissionReturnsNotFoundResponse()
        {
            var response = await _testPermissionController.DeletePermission(-1);

            response.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}
