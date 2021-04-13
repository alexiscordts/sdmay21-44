using System;
using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgramTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using InpatientTherapySchedulingProgram.Services;

namespace InpatientTherapySchedulingProgramTests.IntegrationTests
{
    [TestClass]
    class HoursWorkedServiceControllerTests
    {
        private List<HoursWorked> _testHoursWorked;
        private CoreDbContext _testContext;
        private HoursWorkedService _testService;
        private HoursWorkedController _testController;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "UserDatabase")
                .Options;
            _testHoursWorked = new List<HoursWorked>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            for (var i = 0; i < 10; i++)
            {
                var newHoursWorked = ModelFakes.HoursWorkedFake.Generate();
                _testHoursWorked.Add(ObjectExtensions.Copy(newHoursWorked));
                _testContext.Add(newHoursWorked);
                _testContext.SaveChanges();
            }

            _testService = new HoursWorkedService(_testContext);
            _testController = new HoursWorkedController(_testService);
        }

        [TestMethod]
        public async Task ValidGetByUserIdReturnsOkResponse()
        {
            var response = await _testController.GetHoursWorkedByUserId(_testHoursWorked[0].UserId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetByUserIdReturnsCorrectType()
        {
            var response = await _testController.GetHoursWorkedByUserId(_testHoursWorked[0].UserId);
            var responseResult = response.Result as OkObjectResult;
            var hoursWorked = responseResult.Value;

            hoursWorked.Should().BeOfType<HoursWorked>();
        }

        [TestMethod]
        public async Task ValidGetByUserIdReturnsCorrectUsersHours()
        {
            var response = await _testController.GetHoursWorkedByUserId(_testHoursWorked[0].UserId);
            var responseResult = response.Result as OkObjectResult;
            var hoursWorked = responseResult.Value;

            hoursWorked.Should().Be(_testHoursWorked[0]);
        }

        [TestMethod]
        public async Task NonExistingGetByUserIdReturnsNotFoundResponse()
        {
            var response = await _testController.GetHoursWorkedByUserId(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidGetHoursWorkedByIdReturnsOkResponse()
        {
            var response = await _testController.GetHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetHoursWorkedByIdReturnsCorrectType()
        {
            var response = await _testController.GetHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result as OkObjectResult;
            var user = responseResult.Value;

            user.Should().BeOfType<HoursWorked>();
        }

        [TestMethod]
        public async Task ValidGetHoursWorkedByIdReturnsCorrectHoursWorked()
        {
            var response = await _testController.GetHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result as OkObjectResult;
            var user = responseResult.Value;

            user.Should().Be(_testHoursWorked[0]);
        }

        [TestMethod]
        public async Task NonExistingGetHoursWorkedByIdReturnsNotFound()
        {
            var response = await _testController.GetHoursWorked(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutHoursWorkedReturnsNoContentResponse()
        {
            var response = await _testController.PutHoursWorked(_testHoursWorked[0].HoursWorkedId, _testHoursWorked[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task NonMatchingPutHoursWorkedIdsShouldReturnBadRequest()
        {
            var response = await _testController.PutHoursWorked(-1, _testHoursWorked[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingPutHoursWorkedShouldReturnBadRequest()
        {
            var fakeHoursWorked = new HoursWorked();
            fakeHoursWorked.UserId = -1;

            var response = await _testController.PutHoursWorked(-1, fakeHoursWorked);

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPostUHoursWorkedReturnsCreatedAtActionResponse()
        {
            var newHoursWorked = ModelFakes.HoursWorkedFake.Generate();
            var response = await _testController.PostHoursWorked(newHoursWorked);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostHoursCorrectlyAddsHours()
        {
            var newHoursWorked = ModelFakes.HoursWorkedFake.Generate();
            await _testController.PostHoursWorked(newHoursWorked);

            var response = await _testController.GetHoursWorked(newHoursWorked.HoursWorkedId);
            var responseResult = response.Result as OkObjectResult;
            var user = responseResult.Value;

            user.Should().Be(newHoursWorked);
        }

        [TestMethod]
        public async Task ExistingHoursWorkedIdPostUserDoesNotAddHoursWorked()
        {
            var newHoursWorked = ModelFakes.HoursWorkedFake.Generate();
            newHoursWorked.HoursWorkedId = _testHoursWorked[0].HoursWorkedId;

            await _testController.PostHoursWorked(newHoursWorked);

            var getResponse = await _testController.GetHoursWorked(newHoursWorked.HoursWorkedId);
            var getResponseResult = getResponse.Result as OkObjectResult;
            var getUser = getResponseResult.Value;

            getUser.Should().NotBe(newHoursWorked);
        }

        [TestMethod]
        public async Task ValidDeleteHoursWorkedReturnsOkResponse()
        {
            var response = await _testController.DeleteHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteHoursWorkedReturnsCorrectType()
        {
            var response = await _testController.DeleteHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result as OkObjectResult;
            var user = responseResult.Value;

            user.Should().BeOfType<User>();
        }

        [TestMethod]
        public async Task ValidDeleteHoursWorkedReturnsCorrectHours()
        {
            var response = await _testController.DeleteHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result as OkObjectResult;
            var user = responseResult.Value;

            user.Should().Be(_testHoursWorked[0]);
        }

        [TestMethod]
        public async Task ValidDeleteHoursWorkedCorrectlyRemovesHoursWorked()
        {
            await _testController.DeleteHoursWorked(_testHoursWorked[0].HoursWorkedId);

            var response = await _testController.GetHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task NonExistingDeleteHoursWorkedReturnsNotFound()
        {
            var response = await _testController.DeleteHoursWorked(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }
    }
}
