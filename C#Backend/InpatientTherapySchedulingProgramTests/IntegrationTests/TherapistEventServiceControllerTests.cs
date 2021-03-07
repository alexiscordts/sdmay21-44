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
using System;

namespace InpatientTherapySchedulingProgramTests.IntegrationTests
{
    [TestClass]
    public class TherapistEventServiceControllerTests
    {
        private List<User> _testUsers;
        private List<TherapistActivity> _testTherapistActivities;
        private List<TherapistEvent> _testTherapistEvents;
        private DateTime _testTargetStartDateTime;
        private DateTime _testTargetEndDateTime;
        private TherapistEvent _testTherapistEvent;
        private CoreDbContext _testContext;
        private TherapistEventService _testTherapistEventService;
        private TherapistEventController _testTherapistEventController;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TherapistEventDatabase")
                .Options;
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();
            _testUsers = new List<User>();
            _testTherapistActivities = new List<TherapistActivity>();
            _testTherapistEvents = new List<TherapistEvent>();
            _testTargetStartDateTime = new DateTime(2010, 2, 8);
            _testTargetEndDateTime = new DateTime(2010, 2, 12);

            for (var i = 0; i < 8; i++)
            {
                var newUser = ModelFakes.UserFake.Generate();
                _testUsers.Add(ObjectExtensions.Copy(newUser));
                _testContext.Add(newUser);
                _testContext.SaveChanges();

                var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();
                _testTherapistActivities.Add(ObjectExtensions.Copy(newTherapistActivity));
                _testContext.Add(newTherapistActivity);
                _testContext.SaveChanges();

                var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
                newTherapistEvent.TherapistId = newUser.UserId;
                newTherapistEvent.ActivityName = newTherapistActivity.ActivityName;

                if (i <= 2)
                {
                    newTherapistEvent.StartTime = _testTargetStartDateTime.AddDays(i);
                    newTherapistEvent.EndTime = _testTargetEndDateTime.AddDays(-1 * i);
                }

                _testTherapistEvents.Add(ObjectExtensions.Copy(newTherapistEvent));
                _testContext.Add(newTherapistEvent);
                _testContext.SaveChanges();
            }

            var newEdgeTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newEdgeTherapistEvent.TherapistId = _testUsers[0].UserId;
            newEdgeTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;
            newEdgeTherapistEvent.StartTime = _testTargetStartDateTime.AddDays(-1);
            _testTherapistEvents.Add(ObjectExtensions.Copy(newEdgeTherapistEvent));
            _testContext.Add(newEdgeTherapistEvent);
            _testContext.SaveChanges();

            newEdgeTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newEdgeTherapistEvent.TherapistId = _testUsers[0].UserId;
            newEdgeTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;
            newEdgeTherapistEvent.EndTime = _testTargetEndDateTime.AddDays(1);
            _testTherapistEvents.Add(ObjectExtensions.Copy(newEdgeTherapistEvent));
            _testContext.Add(newEdgeTherapistEvent);
            _testContext.SaveChanges();

            _testTherapistEvent = new TherapistEvent
            {
                StartTime = DateTime.MinValue,
                EndTime = DateTime.MaxValue,
                TherapistId = _testUsers[0].UserId
            };

            _testTherapistEventService = new TherapistEventService(_testContext);
            _testTherapistEventController = new TherapistEventController(_testTherapistEventService);
        }

        [TestMethod]
        public async Task ValidGetAllTherapistEventsReturnsOkResponse()
        {
            var response = await _testTherapistEventController.GetTherapistEvent(_testTherapistEvent);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllTherapistEventsReturnsCorrectType()
        {
            var response = await _testTherapistEventController.GetTherapistEvent(_testTherapistEvent);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<TherapistEvent>>();
        }

        [TestMethod]
        public async Task ValidGetAllTherapistEventsReturnsCorrectNumberOfTherapistEvents()
        {
            var response = await _testTherapistEventController.GetTherapistEvent(_testTherapistEvent);
            var responseResult = response.Result as OkObjectResult;
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)responseResult.Value;

            listOfTherapistEvents.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task ValidGetAllTherapistEventsReturnsCorrectTherapistEvents()
        {
            var response = await _testTherapistEventController.GetTherapistEvent(_testTherapistEvent);
            var responseResult = response.Result as OkObjectResult;
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)responseResult.Value;

            for (var i = 0; i < listOfTherapistEvents.Count; i++)
            {
                _testTherapistEvents.Contains(listOfTherapistEvents[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetAllTherapistEventsInRangeReturnsCorrectNumberOfTherapistEvents()
        {
            _testTherapistEvent.StartTime = _testTargetStartDateTime;
            _testTherapistEvent.EndTime = _testTargetEndDateTime;
            var response = await _testTherapistEventController.GetTherapistEvent(_testTherapistEvent);
            var responseResult = response.Result as OkObjectResult;
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)responseResult.Value;

            listOfTherapistEvents.Count.Should().Be(3);
        }

        [TestMethod]
        public async Task ValidGetAllTherapistEventsInRangeReturnsCorrectTherapistEvents()
        {
            _testTherapistEvent.StartTime = _testTargetStartDateTime;
            _testTherapistEvent.EndTime = _testTargetEndDateTime;
            var response = await _testTherapistEventController.GetTherapistEvent(_testTherapistEvent);
            var responseResult = response.Result as OkObjectResult;
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)responseResult.Value;

            for (var i = 0; i < listOfTherapistEvents.Count; i++)
            {
                _testTherapistEvents.Contains(listOfTherapistEvents[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task NullParameterGetAllTherapistEventsReturnsBadRequestResponse()
        {
            var response = await _testTherapistEventController.GetTherapistEvent((TherapistEvent)null);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public async Task ValidGetAllTherapistEventsByTherapistIdReturnsOkResponse()
        {
            var response = await _testTherapistEventController.GetTherapistEventByTherapistId(_testTherapistEvent);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllTherapistEventsByTherapistIdReturnsCorrectType()
        {
            var response = await _testTherapistEventController.GetTherapistEventByTherapistId(_testTherapistEvent);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<TherapistEvent>>();
        }

        [TestMethod]
        public async Task ValidGetAllTherapistEventsByTherapistIdReturnsCorrectNumberOfTherapistEvents()
        {
            var response = await _testTherapistEventController.GetTherapistEventByTherapistId(_testTherapistEvent);
            var responseResult = response.Result as OkObjectResult;
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)responseResult.Value;

            listOfTherapistEvents.Count.Should().Be(3);
        }

        [TestMethod]
        public async Task ValidGetAllTherapistEventsByTherapistIdReturnsCorrectTherapistEvents()
        {
            var response = await _testTherapistEventController.GetTherapistEventByTherapistId(_testTherapistEvent);
            var responseResult = response.Result as OkObjectResult;
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)responseResult.Value;

            for (var i = 0; i < listOfTherapistEvents.Count; i++)
            {
                _testTherapistEvents.Contains(listOfTherapistEvents[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetAllTherapistEventsByTherapistIdInRangeReturnsCorrectNumberOfTherapistEvents()
        {
            _testTherapistEvent.StartTime = _testTargetStartDateTime;
            _testTherapistEvent.EndTime = _testTargetEndDateTime;
            var response = await _testTherapistEventController.GetTherapistEventByTherapistId(_testTherapistEvent);
            var responseResult = response.Result as OkObjectResult;
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)responseResult.Value;

            listOfTherapistEvents.Count.Should().Be(1);
        }

        [TestMethod]
        public async Task ValidGetAllTherapistEventsByTherapistIdInRangeReturnsCorrectTherapistEvents()
        {
            _testTherapistEvent.StartTime = _testTargetStartDateTime;
            _testTherapistEvent.EndTime = _testTargetEndDateTime;
            var response = await _testTherapistEventController.GetTherapistEventByTherapistId(_testTherapistEvent);
            var responseResult = response.Result as OkObjectResult;
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)responseResult.Value;

            for (var i = 0; i < listOfTherapistEvents.Count; i++)
            {
                _testTherapistEvents.Contains(listOfTherapistEvents[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task NullParameterGetAllTherapistEventsByTherapistIdReturnsBadRequestResponse()
        {
            var response = await _testTherapistEventController.GetTherapistEventByTherapistId((TherapistEvent)null);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public async Task ValidPutTherapistEventReturnsNoContentResponse()
        {
            var response = await _testTherapistEventController.PutTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task ValidPutTherapistEventWithAlteredDataCorrectlyUpdatesInDatabase()
        {
            var fakeTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            _testTherapistEvents[0].StartTime = fakeTherapistEvent.StartTime;
            _testTherapistEvents[0].EndTime = fakeTherapistEvent.EndTime;

            await _testTherapistEventController.PutTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0]);

            var response = await _testTherapistEventController.GetTherapistEventByTherapistId(_testTherapistEvents[0]);
            var responseResult = response.Result as OkObjectResult;
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)responseResult.Value;

            listOfTherapistEvents.Contains(_testTherapistEvents[0]).Should().BeTrue();
        }

        [TestMethod]
        public async Task NonMatchingEventIdsPutTherapistEventReturnsBadRequestResponse()
        {
            var response = await _testTherapistEventController.PutTherapistEvent(-1, _testTherapistEvents[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingTherapistEventPutTherapistEventReturnsNotFoundResponse()
        {
            var fakeActivityName = ModelFakes.TherapistActivityFake.Generate().ActivityName;
            _testTherapistEvents[0].ActivityName = fakeActivityName;

            var response = await _testTherapistEventController.PutTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingTherapistActivityPutTherapistEventReturnsBadRequestResponse()
        {
            _testTherapistEvents[0].ActivityName = ModelFakes.TherapistActivityFake.Generate().ActivityName;

            var response = await _testTherapistEventController.PutTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingUserPutTherapistEventReturnsBadRequestResponse()
        {
            _testTherapistEvents[0].TherapistId = -1;

            var response = await _testTherapistEventController.PutTherapistEvent(-1, _testTherapistEvents[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task ValidPostTherapistEventReturnsCreatedAtActionResponse()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = _testUsers[0].UserId;
            newTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;

            var response = await _testTherapistEventController.PostTherapistEvent(newTherapistEvent);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostTherapistEventReturnsCorrectType()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = _testUsers[0].UserId;
            newTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;

            var response = await _testTherapistEventController.PostTherapistEvent(newTherapistEvent);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<TherapistEvent>();
        }

        [TestMethod]
        public async Task ValidPostTherapistEventCorrectlyAddsTherapistEventToDatabase()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = _testUsers[0].UserId;
            newTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;

            await _testTherapistEventController.PostTherapistEvent(newTherapistEvent);

            var response = await _testTherapistEventController.GetTherapistEventByTherapistId(newTherapistEvent);
            var responseResult = response.Result as OkObjectResult;
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)responseResult.Value;

            listOfTherapistEvents.Contains(newTherapistEvent).Should().BeTrue();
        }

        [TestMethod]
        public async Task ExistingEventIdPostTherapistEventReturnsConflictResponse()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.EventId = _testTherapistEvents[0].EventId;
            newTherapistEvent.TherapistId = _testUsers[0].UserId;
            newTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;

            var response = await _testTherapistEventController.PostTherapistEvent(newTherapistEvent);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingTherapistActivityPostTherapistEventReturnsNotFoundResponse()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = _testUsers[0].UserId;
            newTherapistEvent.ActivityName = "-1";

            var response = await _testTherapistEventController.PostTherapistEvent(newTherapistEvent);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingTherapistIdPostTherapistEventReturnsNotFoundResponse()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = -1;
            newTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;

            var response = await _testTherapistEventController.PostTherapistEvent(newTherapistEvent);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapistEventReturnsOkResponse()
        {
            var response = await _testTherapistEventController.DeleteTherapistEvent(_testTherapistEvents[0].EventId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapistEventReturnsCorrectType()
        {
            var response = await _testTherapistEventController.DeleteTherapistEvent(_testTherapistEvents[0].EventId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapistEvent>();
        }

        [TestMethod]
        public async Task NonExistingDeleteTherapistEventReturnsNotFoundResponse()
        {
            var response = await _testTherapistEventController.DeleteTherapistEvent(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }
    }
}
