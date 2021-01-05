using FluentAssertions;
using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Exceptions.TherapistActivityExceptions;
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
    public class TherapistActivityControllerTests
    {
        private static List<TherapistActivity> _testTherapistActivities;
        private Mock<ITherapistActivityService> _fakeService;
        private TherapistActivityController _testController;

        [ClassInitialize()]
        public static void Setup(TestContext context)
        {
            _testTherapistActivities = new List<TherapistActivity>();

            for(var i = 0; i < 10; i++)
            {
                var therapistActivity = ModelFakes.TherapistActivityFake.Generate();
                _testTherapistActivities.Add(therapistActivity);
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            _fakeService = new Mock<ITherapistActivityService>();
            _fakeService.SetupAllProperties();
            _fakeService.Setup(s => s.GetAllTherapistActivities()).ReturnsAsync(_testTherapistActivities);
            _fakeService.Setup(s => s.GetTherapistActivityByName(It.IsAny<string>())).ReturnsAsync(_testTherapistActivities[0]);
            _fakeService.Setup(s => s.UpdateTherapistActivity(It.IsAny<string>(), It.IsAny<TherapistActivity>())).ReturnsAsync(_testTherapistActivities[0]);
            _fakeService.Setup(s => s.AddTherapistActivity(It.IsAny<TherapistActivity>())).ReturnsAsync(_testTherapistActivities[0]);
            _fakeService.Setup(s => s.DeleteTherapistActivity(It.IsAny<string>())).ReturnsAsync(_testTherapistActivities[0]);

            _testController = new TherapistActivityController(_fakeService.Object);
        }

        [TestMethod]
        public async Task ValidGetAllTherapistActivitiesReturnsOkResponse()
        {
            var response = await _testController.GetTherapistActivity();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllTherapistActivitiesReturnsCorrectType()
        {
            var response = await _testController.GetTherapistActivity();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<TherapistActivity>>();
        }

        [TestMethod]
        public async Task ValidGetTherapistActivityByNameReturnsOkResponse()
        {
            var response = await _testController.GetTherapistActivity(_testTherapistActivities[0].Name);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapistActivityByNameReturnsCorrectType()
        {
            var response = await _testController.GetTherapistActivity(_testTherapistActivities[0].Name);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task NullResultGetTherapistActivityByNameReturnsNotFound()
        {
            _fakeService.Setup(s => s.GetTherapistActivityByName(It.IsAny<string>())).ReturnsAsync((TherapistActivity)null);

            var response = await _testController.GetTherapistActivity("-1");

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutTherapistActivityReturnsNoContentResponse()
        {
            var response = await _testController.PutTherapistActivity(_testTherapistActivities[0].Name, _testTherapistActivities[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task TherapistActivityDoNotMatchExceptionPutTherapistActivityReturnsBadRequestResponse()
        {
            _fakeService.Setup(s => s.UpdateTherapistActivity(It.IsAny<string>(), It.IsAny<TherapistActivity>())).ThrowsAsync(new TherapistActivityNamesDoNotMatchException());

            var response = await _testController.PutTherapistActivity("-1", _testTherapistActivities[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task TherapistActivityDoesNotExistExceptionPutTherapistActivityReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.UpdateTherapistActivity(It.IsAny<string>(), It.IsAny<TherapistActivity>())).ThrowsAsync(new TherapistActivityDoesNotExistException());

            var response = await _testController.PutTherapistActivity("-1", new TherapistActivity());

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionPutTherapistActivityThrowsError()
        {
            _fakeService.Setup(s => s.UpdateTherapistActivity(It.IsAny<string>(), It.IsAny<TherapistActivity>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testController.Invoking(c => c.PutTherapistActivity(_testTherapistActivities[0].Name, _testTherapistActivities[0])).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

        [TestMethod]
        public async Task ValidPostTherapistActivityReturnsCreatedAtActionResponse()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            var response = await _testController.PostTherapistActivity(newTherapistActivity);

            response.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostTherapistActivityReturnsCorrectType()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            var response = await _testController.PostTherapistActivity(newTherapistActivity);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task TherapistActivityAlreadyExistsExceptionPostTherapistReturnsConflictResponse()
        {
            _fakeService.Setup(s => s.AddTherapistActivity(It.IsAny<TherapistActivity>())).ThrowsAsync(new TherapistActivityAlreadyExistsException());

            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();
            newTherapistActivity.Name = _testTherapistActivities[0].Name;

            var response = await _testController.PostTherapistActivity(newTherapistActivity);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionPostTherapistActivityThrowsError()
        {
            _fakeService.Setup(s => s.AddTherapistActivity(It.IsAny<TherapistActivity>())).ThrowsAsync(new DbUpdateException());

            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            await _testController.Invoking(c => c.PostTherapistActivity(newTherapistActivity)).Should().ThrowAsync<DbUpdateException>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapistActivityReturnsOkResponse()
        {
            var response = await _testController.DeleteTherapistActivity(_testTherapistActivities[0].Name);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapistActivityReturnsCorrectType()
        {
            var response = await _testController.DeleteTherapistActivity(_testTherapistActivities[0].Name);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task NullResultDeleteTherapistActivityReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.DeleteTherapistActivity(It.IsAny<string>())).ReturnsAsync((TherapistActivity)null);

            var response = await _testController.DeleteTherapistActivity("-1");

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionDeleteTherapistActivityThrowsError()
        {
            _fakeService.Setup(s => s.DeleteTherapistActivity(It.IsAny<string>())).ThrowsAsync(new DbUpdateException());

            await _testController.Invoking(c => c.DeleteTherapistActivity(_testTherapistActivities[0].Name)).Should().ThrowAsync<DbUpdateException>();
        }
    }
}
