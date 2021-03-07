using FluentAssertions;
using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Exceptions.TherapistActivityExceptions;
using InpatientTherapySchedulingProgram.Exceptions.TherapistEventExceptions;
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
    public class TherapistEventControllerTests
    {
        private static List<TherapistEvent> _testTherapistEvents;
        private Mock<ITherapistEventService> _fakeTherapistEventService;
        private TherapistEventController _testTherapistEventController;

        [ClassInitialize()]
        public static void Setup(TestContext context)
        {
            _testTherapistEvents = new List<TherapistEvent>();

            for (var i = 0; i < 10; i++)
            {
                var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
                _testTherapistEvents.Add(newTherapistEvent);
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            _fakeTherapistEventService = new Mock<ITherapistEventService>();
            _fakeTherapistEventService.SetupAllProperties();
            _fakeTherapistEventService.Setup(s => s.GetAllTherapistEvents(It.IsAny<TherapistEvent>())).ReturnsAsync(_testTherapistEvents);
            _fakeTherapistEventService.Setup(s => s.GetAllTherapistEventsByTherapistId(It.IsAny<TherapistEvent>())).ReturnsAsync(_testTherapistEvents);
            _fakeTherapistEventService.Setup(s => s.UpdateTherapistEvent(It.IsAny<int>(), It.IsAny<TherapistEvent>())).ReturnsAsync(_testTherapistEvents[0]);
            _fakeTherapistEventService.Setup(s => s.AddTherapistEvent(It.IsAny<TherapistEvent>())).ReturnsAsync(_testTherapistEvents[0]);
            _fakeTherapistEventService.Setup(s => s.DeleteTherapistEvent(It.IsAny<int>())).ReturnsAsync(_testTherapistEvents[0]);

            _testTherapistEventController = new TherapistEventController(_fakeTherapistEventService.Object);
        }

        public async Task ValidGetAllTherapistEventsReturnsOkResponse()
        {
            var response = await _testTherapistEventController.GetTherapistEvent(_testTherapistEvents[0]);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        public async Task ValidGetAllTherapistEventsReturnsCorrectType()
        {
            var response = await _testTherapistEventController.GetTherapistEvent(_testTherapistEvents[0]);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapistEvent>();
        }

        public async Task NullParameterGetAllTherapistEventsReturnsBadRequestResponse()
        {
            var response = await _testTherapistEventController.GetTherapistEvent((TherapistEvent)null);

            response.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        public async Task ValidGetAllTherapistEventsByTherapistIdReturnsOkResponse()
        {
            var response = await _testTherapistEventController.GetTherapistEventByTherapistId(_testTherapistEvents[0]);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        public async Task ValidGetAllTherapistEventsByTherapistIdReturnsCorrectType()
        {
            var response = await _testTherapistEventController.GetTherapistEventByTherapistId(_testTherapistEvents[0]);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapistEvent>();
        }

        public async Task NullParameterGetAllTherapistEventsByTherapistIdReturnsBadRequestResponse()
        {
            var response = await _testTherapistEventController.GetTherapistEventByTherapistId((TherapistEvent)null);

            response.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        public async Task ValidPutTherapistEventReturnsNoContentResponse()
        {
            var response = await _testTherapistEventController.PutTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        public async Task TherapistEventEventIdsDoNotMatchExceptionPutTherapistReturnsBadRequestResponse()
        {
            _fakeTherapistEventService.Setup(s => s.UpdateTherapistEvent(It.IsAny<int>(), It.IsAny<TherapistEvent>())).ThrowsAsync(new TherapistEventEventIdsDoNotMatchException());

            var response = await _testTherapistEventController.PutTherapistEvent(-1, _testTherapistEvents[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        public async Task TherapistEventDoesNotExistExceptionPutTherapistReturnsNotFoundResponse()
        {
            _fakeTherapistEventService.Setup(s => s.UpdateTherapistEvent(It.IsAny<int>(), It.IsAny<TherapistEvent>())).ThrowsAsync(new TherapistEventDoesNotExistException());

            var fakeTherapistEvent = ModelFakes.TherapistEventFake.Generate();

            var response = await _testTherapistEventController.PutTherapistEvent(fakeTherapistEvent.EventId, fakeTherapistEvent);

            response.Should().BeOfType<NotFoundResult>();
        }

        public async Task TherapistActivityDoesNotExistExceptionPutTherapistReturnsBadRequestResponse()
        {
            _fakeTherapistEventService.Setup(s => s.UpdateTherapistEvent(It.IsAny<int>(), It.IsAny<TherapistEvent>())).ThrowsAsync(new TherapistActivityDoesNotExistException());

            _testTherapistEvents[0].ActivityName = "-1";

            var response = await _testTherapistEventController.PutTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        public async Task UserDoesNotExistExceptionPutTherapistReturnsBadRequestResponse()
        {
            _fakeTherapistEventService.Setup(s => s.UpdateTherapistEvent(It.IsAny<int>(), It.IsAny<TherapistEvent>())).ThrowsAsync(new UserDoesNotExistException());

            _testTherapistEvents[0].TherapistId = -1;

            var response = await _testTherapistEventController.PutTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        public async Task ValidPostTherapistEventReturnsCreatedAtActionResponse()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();

            var response = await _testTherapistEventController.PostTherapistEvent(newTherapistEvent);

            response.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        public async Task TherapistEventEventIdAlreadyExistsExceptionPostTherapistEventReturnsConflictResponse()
        {
            _fakeTherapistEventService.Setup(s => s.AddTherapistEvent(It.IsAny<TherapistEvent>())).ThrowsAsync(new TherapistEventEventIdAlreadyExistsException());

            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent = _testTherapistEvents[0];

            var response = await _testTherapistEventController.PostTherapistEvent(newTherapistEvent);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        public async Task TherapistActivityDoesNotExistExceptionPostTherapistEventReturnsNotFoundResponse()
        {
            _fakeTherapistEventService.Setup(s => s.AddTherapistEvent(It.IsAny<TherapistEvent>())).ThrowsAsync(new TherapistActivityDoesNotExistException());

            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.ActivityName = "-1";

            var response = await _testTherapistEventController.PostTherapistEvent(newTherapistEvent);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        public async Task UserDoesNotExistExceptionPostTherapistEventReturnsNotFoundResponse()
        {
            _fakeTherapistEventService.Setup(s => s.AddTherapistEvent(It.IsAny<TherapistEvent>())).ThrowsAsync(new UserDoesNotExistException());

            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = -1;

            var response = await _testTherapistEventController.PostTherapistEvent(newTherapistEvent);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        public async Task DbUpdateExceptionPostTherapistEventThrowsDbUpdateException()
        {
            _fakeTherapistEventService.Setup(s => s.AddTherapistEvent(It.IsAny<TherapistEvent>())).ThrowsAsync(new DbUpdateException());

            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();

            await _testTherapistEventController.Invoking(c => c.PostTherapistEvent(newTherapistEvent)).Should().ThrowAsync<DbUpdateException>();
        }

        public async Task ValidDeleteTherapistEventReturnsOkResponse()
        {
            var response = await _testTherapistEventController.DeleteTherapistEvent(_testTherapistEvents[0].EventId);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        public async Task NullDeleteTherapistEventReturnsNotFoundResponse()
        {
            _fakeTherapistEventService.Setup(s => s.DeleteTherapistEvent(It.IsAny<int>())).ReturnsAsync((TherapistEvent)null);

            var response = await _testTherapistEventController.DeleteTherapistEvent(-1);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        public async Task DbUpdateConcurrencyExceptionDeleteTherapistEventThrowsDbUpdateConcurrencyException()
        {
            _fakeTherapistEventService.Setup(s => s.DeleteTherapistEvent(It.IsAny<int>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testTherapistEventController.Invoking(c => c.DeleteTherapistEvent(_testTherapistEvents[0].EventId)).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }
}
