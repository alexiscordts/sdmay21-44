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
using InpatientTherapySchedulingProgram.Exceptions.HoursWorkedExceptions;

namespace InpatientTherapySchedulingProgramTests.ControllerTests
{
    [TestClass]
    class HoursWorkedControllerTests
    {
        private static List<HoursWorked> _testHoursWorked;
        private Mock<IHoursWorkedService> _fakeService;
        private HoursWorkedController _testController;

        [ClassInitialize()]
        public static void ClassSetup(TestContext context)
        {
            _testHoursWorked = new List<HoursWorked>();

            for (var i = 0; i < 10; i++)
            {
                var hours = ModelFakes.HoursWorkedFake.Generate();
                _testHoursWorked.Add(hours);
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            _fakeService = new Mock<IHoursWorkedService>();
            _fakeService.SetupAllProperties();
            _fakeService.Setup(s => s.GetHoursWorkedById(It.IsAny<int>())).ReturnsAsync(_testHoursWorked[0]);
            _fakeService.Setup(s => s.GetHoursWorkedByUserId(It.IsAny<int?>())).ReturnsAsync(_testHoursWorked[0]);
            _fakeService.Setup(s => s.UpdateHoursWorked(It.IsAny<int>(), It.IsAny<HoursWorked>())).ReturnsAsync(_testHoursWorked[0]);
            _fakeService.Setup(s => s.AddHoursWorked(It.IsAny<HoursWorked>())).ReturnsAsync(_testHoursWorked[0]);
            _fakeService.Setup(s => s.DeleteHoursWorked(It.IsAny<int>())).ReturnsAsync(_testHoursWorked[0]);

            _testController = new HoursWorkedController(_fakeService.Object);
        }

      
        [TestMethod]
        public async Task ValidGetHoursWorkedByUserIdReturnsOkResponse()
        {
            var response = await _testController.GetHoursWorkedByUserId(_testHoursWorked[0].UserId);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGethoursWorkedByUserIdReturnsCorrectType()
        {
            var response = await _testController.GetHoursWorkedByUserId(_testHoursWorked[0].UserId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<HoursWorked>();
        }

        [TestMethod]
        public async Task GetNonExistingHoursWorkedByUserIdReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.GetHoursWorkedByUserId(It.IsAny<int?>())).ReturnsAsync((HoursWorked)null);

            var response = await _testController.GetHoursWorkedByUserId(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidGetHoursWorkedByIdReturnsOkResponse()
        {
            var response = await _testController.GetHoursWorked(_testHoursWorked[0].HoursWorkedId);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetHoursWorkedByIdReturnsCorrectType()
        {
            var response = await _testController.GetHoursWorked(_testHoursWorked[0].HoursWorkedId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<HoursWorked>();
        }

        [TestMethod]
        public async Task GetNonExistingHoursWorkedByIdReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.GetHoursWorkedById(It.IsAny<int>())).ReturnsAsync((HoursWorked)null);

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
        public async Task NonMatchingHoursWorkedIdPutHoursWorkedReturnsBadRequest()
        {
            _fakeService.Setup(s => s.UpdateHoursWorked(It.IsAny<int>(), It.IsAny<HoursWorked>())).ThrowsAsync(new HoursWorkedIdsDoNotMatchException());

            var response = await _testController.PutHoursWorked(-1, _testHoursWorked[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingHoursWorkedPostHoursWorkedReturnsNotFound()
        {
            _fakeService.Setup(s => s.UpdateHoursWorked(It.IsAny<int>(), It.IsAny<HoursWorked>())).ThrowsAsync(new HoursWorkedDoesNotExistException());

            var response = await _testController.PutHoursWorked(_testHoursWorked[0].HoursWorkedId, new HoursWorked());

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionPutHoursWorkedShouldThrowDbUpdateConcurrencyException()
        {
            _fakeService.Setup(s => s.UpdateHoursWorked(It.IsAny<int>(), It.IsAny<HoursWorked>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testController.Invoking(c => c.PutHoursWorked(_testHoursWorked[0].HoursWorkedId, _testHoursWorked[0])).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

        [TestMethod]
        public async Task ValidPostHoursWorkedReturnsCreatedAtActionResponse()
        {
            var response = await _testController.PostHoursWorked(_testHoursWorked[0]);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostHoursWorkedReturnsCorrectType()
        {
            var response = await _testController.PostHoursWorked(_testHoursWorked[0]);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<HoursWorked>();
        }

        [TestMethod]
        public async Task ExistingHoursWorkedIdPostHoursWorkedReturnsConflictResponse()
        {
            _fakeService.Setup(s => s.AddHoursWorked(It.IsAny<HoursWorked>())).ThrowsAsync(new HoursWorkedIdAlreadyExistsException());

            var response = await _testController.PostHoursWorked(new HoursWorked());
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionPostHoursWorkedThrowsDbUpdateException()
        {
            _fakeService.Setup(s => s.AddHoursWorked(It.IsAny<HoursWorked>())).ThrowsAsync(new DbUpdateException());

            await _testController.Invoking(c => c.PostHoursWorked(new HoursWorked())).Should().ThrowAsync<DbUpdateException>();
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

            responseResult.Value.Should().BeOfType<HoursWorked>();
        }

        [TestMethod]
        public async Task NonExistingHoursWorkedDeleteHoursWorkedReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.DeleteHoursWorked(It.IsAny<int>())).ReturnsAsync((HoursWorked)null);

            var response = await _testController.DeleteHoursWorked(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionDeleteHoursWorkedThrowsDbUpdateConcurrencyException()
        {
            _fakeService.Setup(s => s.DeleteHoursWorked(It.IsAny<int>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testController.Invoking(c => c.DeleteHoursWorked(-1)).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }
}
