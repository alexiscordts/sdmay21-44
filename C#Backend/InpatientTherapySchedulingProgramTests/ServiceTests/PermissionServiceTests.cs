using FluentAssertions;
using InpatientTherapySchedulingProgram.Exceptions.PermissionExceptions;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services;
using InpatientTherapySchedulingProgramTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgramTests.ServiceTests
{
    [TestClass]
    public class PermissionServiceTests
    {
        private List<User> _testUsers;
        private User _userWithNoPermissions;
        private User _nonActiveUser;
        private List<Permission> _testPermissions;
        private CoreDbContext _testContext;
        private PermissionService _testPermissionService;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "PermissionDatabase")
                .Options;
            _testUsers = new List<User>();
            _testPermissions = new List<Permission>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            _userWithNoPermissions = ModelFakes.UserFake.Generate();
            _testUsers.Add(ObjectExtensions.Copy(_userWithNoPermissions));
            _testContext.Add(_userWithNoPermissions);
            _testContext.SaveChanges();

            for (var i = 0; i < 10; i++)
            {
                var newUser = ModelFakes.UserFake.Generate();
                _testContext.Add(newUser);
                _testContext.SaveChanges();
                _testUsers.Add(ObjectExtensions.Copy(newUser));

                var newPermission = ModelFakes.PermissionFake.Generate();
                newPermission.UserId = newUser.UserId;
                _testContext.Add(newPermission);
                _testContext.SaveChanges();
                _testPermissions.Add(ObjectExtensions.Copy(newPermission));
            }

            _nonActiveUser = ModelFakes.UserFake.Generate();
            _nonActiveUser.Active = false;
            _testContext.Add(_nonActiveUser);
            _testContext.SaveChanges();
            _testUsers.Add(ObjectExtensions.Copy(_nonActiveUser));

            _testPermissionService = new PermissionService(_testContext);
        }

        [TestMethod]
        public async Task GetAllPermissionsReturnsCorrectType()
        {
            var allPermissions = await _testPermissionService.GetAllPermissions();

            allPermissions.Should().BeOfType<List<Permission>>();
        }

        [TestMethod]
        public async Task GetAllPermissionsReturnsCorrectNumberOfPermissions()
        {
            var allPermissions = await _testPermissionService.GetAllPermissions();
            List<Permission> listOfPermissions = (List<Permission>)allPermissions;

            listOfPermissions.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task GetAllPermissionsReturnsCorrectListOfPermissions()
        {
            var allPermissions = await _testPermissionService.GetAllPermissions();
            List<Permission> listOfPermissions = (List<Permission>)allPermissions;

            for (var i = 0; i < listOfPermissions.Count; i++)
            {
                _testPermissions.Contains(listOfPermissions[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetPermissionByUserIdReturnsCorrectType()
        {
            var permission = await _testPermissionService.GetPermissionByUserId(_testPermissions[0].UserId);

            permission.Should().BeOfType<Permission>();
        }

        [TestMethod]
        public async Task GetPermissionByUserIdReturnsCorrectPermission()
        {
            var permission = await _testPermissionService.GetPermissionByUserId(_testPermissions[0].UserId);

            permission.Should().Be(_testPermissions[0]);
        }

        [TestMethod]
        public async Task GetPermissionByUserIdReturnsNullIfUserDoesNotExist()
        {
            var permission = await _testPermissionService.GetPermissionByUserId(-1);

            permission.Should().BeNull();
        }

        [TestMethod]
        public async Task AddPermissionReturnsCorrectType()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = _userWithNoPermissions.UserId;

            var returnPermission = await _testPermissionService.AddPermission(newPermission);

            returnPermission.Should().BeOfType<Permission>();
        }

        [TestMethod]
        public async Task AddPermissionReturnsCorrectPermission()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = _userWithNoPermissions.UserId;

            var returnPermission = await _testPermissionService.AddPermission(newPermission);

            returnPermission.Should().Be(newPermission);
        }

        [TestMethod]
        public async Task AddPermissionIncreasesCountOfPermissions()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = _userWithNoPermissions.UserId;

            await _testPermissionService.AddPermission(newPermission);

            var allPermissions = await _testPermissionService.GetAllPermissions();
            List<Permission> listOfPermissions = (List<Permission>)allPermissions;

            listOfPermissions.Count.Should().Be(11);
        }

        [TestMethod]
        public async Task AddPermissionCorrectlyAddsPermissionsToDatabase()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = _userWithNoPermissions.UserId;

            await _testPermissionService.AddPermission(newPermission);

            var allPermissions = await _testPermissionService.GetAllPermissions();
            List<Permission> listOfPermissions = (List<Permission>)allPermissions;

            listOfPermissions.Contains(newPermission).Should().BeTrue();
        }

        [TestMethod]
        public async Task AddPermissionWithInvalidRoleThrowsPermissionRoleIsInvalidException()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.Role = "-1";

            await _testPermissionService.Invoking(s => s.AddPermission(newPermission)).Should().ThrowAsync<PermissionRoleIsInvalidException>();
        }

        [TestMethod]
        public async Task AddPermissionWithNonExistingUserThrowsUserDoesNotExistException()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = -1;

            await _testPermissionService.Invoking(s => s.AddPermission(newPermission)).Should().ThrowAsync<UserDoesNotExistException>();
        }

        [TestMethod]
        public async Task AddPermissionWithNonActiveUserThrowsUserDoesNotExistException()
        {
            var newPermission = ModelFakes.PermissionFake.Generate();
            newPermission.UserId = _nonActiveUser.UserId;

            await _testPermissionService.Invoking(s => s.AddPermission(newPermission)).Should().ThrowAsync<UserDoesNotExistException>();
        }

        [TestMethod]
        public async Task AddPermissionWithExistingPermissionThrowsPermissionAlreadyExistsException()
        {
            await _testPermissionService.Invoking(s => s.AddPermission(_testPermissions[0])).Should().ThrowAsync<PermissionAlreadyExistsException>();
        }

        [TestMethod]
        public async Task DeletePermissionReturnsCorrectType()
        {
            var permission = await _testPermissionService.DeletePermission(_testPermissions[0].UserId);

            permission.Should().BeOfType<Permission>();
        }

        [TestMethod]
        public async Task DeletePermissionReturnsCorrectPermission()
        {
            var permission = await _testPermissionService.DeletePermission(_testPermissions[0].UserId);

            permission.Should().Be(_testPermissions[0]);
        }

        [TestMethod]
        public async Task DeletePermissionWithNonExistingPermissionReturnsNull()
        {
            var fakePermission = ModelFakes.PermissionFake.Generate();
            fakePermission.UserId = -1;

            var returnPermission = await _testPermissionService.DeletePermission(fakePermission.UserId);

            returnPermission.Should().BeNull();
        }
    }
}
