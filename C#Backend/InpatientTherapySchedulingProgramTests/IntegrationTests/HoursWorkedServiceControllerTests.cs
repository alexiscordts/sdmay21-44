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
    public class HoursWorkedServiceControllerTests
    {
        private List<HoursWorked> _testHoursWorked;
        private CoreDbContext _testHoursWorkedContext;
        private HoursWorkedService _testHoursWorkedService;
        private HoursWorkedController _testHoursWorkedController;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "HoursWorkedDatabase")
                .Options;
            _testHoursWorked = new List<HoursWorked>();
            _testHoursWorkedContext = new CoreDbContext(options);
            _testHoursWorkedContext.Database.EnsureDeleted();

            for (var i = 0; i < 10; i++)
            {
                var newUser = ModelFakes.UserFake.Generate();
                _testHoursWorkedContext.Add(newUser);
                _testHoursWorkedContext.SaveChanges();

                var newHoursWorked = ModelFakes.HoursWorkedFake.Generate();
                newHoursWorked.User = newUser;
                newHoursWorked.UserId = newUser.UserId;
                _testHoursWorkedContext.Add(newHoursWorked);
                _testHoursWorkedContext.SaveChanges();
                //newHoursWorked.User.HoursWorked.Add(newHoursWorked);
                _testHoursWorked.Add(ObjectExtensions.Copy(newHoursWorked));
            }

            _testHoursWorkedService = new HoursWorkedService(_testHoursWorkedContext);
            _testHoursWorkedController = new HoursWorkedController(_testHoursWorkedService);
        }

        [TestMethod]
        public async Task ValidGetByUserIdReturnsOkResponse()
        {
            var response = await _testHoursWorkedController.GetHoursWorkedByUserId(_testHoursWorked[0].UserId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetByUserIdReturnsCorrectType()
        {
            var response = await _testHoursWorkedController.GetHoursWorkedByUserId(_testHoursWorked[0].UserId);
            var responseResult = response.Result as OkObjectResult;
            var hoursWorked = responseResult.Value;

            hoursWorked.Should().BeOfType<HoursWorked>();
        }

        [TestMethod]
        public async Task ValidGetByUserIdReturnsCorrectUsersHours()
        {
            var response = await _testHoursWorkedController.GetHoursWorkedByUserId(_testHoursWorked[0].UserId);
            var responseResult = response.Result as OkObjectResult;
            var hoursWorked = responseResult.Value;

            hoursWorked.Should().Be(_testHoursWorked[0]);
        }

        [TestMethod]
        public async Task NonExistingGetByUserIdReturnsNotFoundResponse()
        {
            var response = await _testHoursWorkedController.GetHoursWorkedByUserId(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidGetHoursWorkedByIdReturnsOkResponse()
        {
            var response = await _testHoursWorkedController.GetHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetHoursWorkedByIdReturnsCorrectType()
        {
            var response = await _testHoursWorkedController.GetHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result as OkObjectResult;
            var hours = responseResult.Value;

            hours.Should().BeOfType<HoursWorked>();
        }

        [TestMethod]
        public async Task ValidGetHoursWorkedByIdReturnsCorrectHoursWorked()
        {
            var response = await _testHoursWorkedController.GetHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result as OkObjectResult;
            HoursWorked hours = (HoursWorked)responseResult.Value;

            var compare = hours.Equals(_testHoursWorked[0]);
            hours.Should().Be(_testHoursWorked[0]);
        }

        [TestMethod]
        public async Task NonExistingGetHoursWorkedByIdReturnsNotFound()
        {
            var response = await _testHoursWorkedController.GetHoursWorked(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutHoursWorkedReturnsNoContentResponse()
        {
            var response = await _testHoursWorkedController.PutHoursWorked(_testHoursWorked[0].HoursWorkedId, _testHoursWorked[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task NonMatchingPutHoursWorkedIdsShouldReturnBadRequest()
        {
            var response = await _testHoursWorkedController.PutHoursWorked(-1, _testHoursWorked[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingPutHoursWorkedShouldReturnBadRequest()
        {
            var fakeHoursWorked = new HoursWorked();
            fakeHoursWorked.UserId = -1;

            var response = await _testHoursWorkedController.PutHoursWorked(-1, fakeHoursWorked);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task ValidPostUHoursWorkedReturnsCreatedAtActionResponse()
        {
            var newHoursWorked = ModelFakes.HoursWorkedFake.Generate();
            var response = await _testHoursWorkedController.PostHoursWorked(newHoursWorked);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostHoursCorrectlyAddsHours()
        {
            var newHoursWorked = ModelFakes.HoursWorkedFake.Generate();
            await _testHoursWorkedController.PostHoursWorked(newHoursWorked);

            var response = await _testHoursWorkedController.GetHoursWorked(newHoursWorked.HoursWorkedId);
            var responseResult = response.Result as OkObjectResult;
            var hours = responseResult.Value;

            hours.Should().Be(newHoursWorked);
        }

        [TestMethod]
        public async Task ValidDeleteHoursWorkedReturnsOkResponse()
        {
            var response = await _testHoursWorkedController.DeleteHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteHoursWorkedReturnsCorrectType()
        {
            var response = await _testHoursWorkedController.DeleteHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result as OkObjectResult;
            var hours = responseResult.Value;

            hours.Should().BeOfType<HoursWorked>();
        }

        [TestMethod]
        public async Task ValidDeleteHoursWorkedReturnsCorrectHours()
        {
            var response = await _testHoursWorkedController.DeleteHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result as OkObjectResult;
            var hours = responseResult.Value;

            hours.Should().Be(_testHoursWorked[0]);
        }

        [TestMethod]
        public async Task ValidDeleteHoursWorkedCorrectlyRemovesHoursWorked()
        {
            await _testHoursWorkedController.DeleteHoursWorked(_testHoursWorked[0].HoursWorkedId);

            var response = await _testHoursWorkedController.GetHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingDeleteHoursWorkedReturnsNotFound()
        {
            var response = await _testHoursWorkedController.DeleteHoursWorked(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }
    }
}
