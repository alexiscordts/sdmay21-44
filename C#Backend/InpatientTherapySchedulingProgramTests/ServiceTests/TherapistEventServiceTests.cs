using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services;
using InpatientTherapySchedulingProgramTests.Fakes;
using System.Threading.Tasks;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;
using System;
using InpatientTherapySchedulingProgram.Exceptions.TherapistEventExceptions;
using InpatientTherapySchedulingProgram.Exceptions.TherapistActivityExceptions;

namespace InpatientTherapySchedulingProgramTests.ServiceTests
{
    [TestClass]
    public class TherapistEventServiceTests
    {
        private List<User> _testUsers;
        private List<TherapistEvent> _testTherapistEvents;
        private DateTime _testTargetStartDateTime;
        private DateTime _testTargetEndDateTime;
        private TherapistEvent _testTherapistEvent;
        private User _testNonTherapistUser;
        private CoreDbContext _testContext;
        private TherapistEventService _testTherapistEventService;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TherapistEventDatabase")
                .Options;
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();
            _testUsers = new List<User>();
            _testTherapistEvents = new List<TherapistEvent>();
            _testTargetStartDateTime = new DateTime(2010, 2, 8);
            _testTargetEndDateTime = new DateTime(2010, 2, 12);

            for (var i = 0; i < 8; i++)
            {
                var newUser = ModelFakes.UserFake.Generate();
                _testUsers.Add(ObjectExtensions.Copy(newUser));
                _testContext.Add(newUser);
                _testContext.SaveChanges();

                Permission newPermission = new Permission();
                newPermission.UserId = newUser.UserId;
                newPermission.Role = "therapist";
                _testContext.Add(newPermission);
                _testContext.SaveChanges();

                var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
                newTherapistEvent.TherapistId = newUser.UserId;
                
                if (i <= 2)
                {
                    newTherapistEvent.StartTime = _testTargetStartDateTime.AddDays(i);
                    newTherapistEvent.EndTime = _testTargetEndDateTime.AddDays(-1 * i);
                }

                _testTherapistEvents.Add(ObjectExtensions.Copy(newTherapistEvent));
                _testContext.Add(newTherapistEvent);
                _testContext.SaveChanges();
            }

            _testNonTherapistUser = ModelFakes.UserFake.Generate();
            _testUsers.Add(ObjectExtensions.Copy(_testNonTherapistUser));
            _testContext.Add(_testNonTherapistUser);
            _testContext.SaveChanges();

            var newEdgeTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newEdgeTherapistEvent.TherapistId = _testUsers[0].UserId;
            newEdgeTherapistEvent.StartTime = _testTargetStartDateTime.AddDays(-1);
            _testTherapistEvents.Add(ObjectExtensions.Copy(newEdgeTherapistEvent));
            _testContext.Add(newEdgeTherapistEvent);
            _testContext.SaveChanges();

            newEdgeTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newEdgeTherapistEvent.TherapistId = _testUsers[0].UserId;
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
        }

        [TestMethod]
        public async Task GetAllTherapistEventsReturnsCorrectType()
        {
            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEvents(_testTherapistEvent);

            allTherapistEvents.Should().BeOfType<List<TherapistEvent>>();
        }

        [TestMethod]
        public async Task GetAllTherapistEventsReturnsCorrectNumberOfTherapistEvents()
        {
            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEvents(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            listOfTherapistEvents.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task GetAllTherapistEventsReturnsCorrectListOfTherapistEvents()
        {
            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEvents(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            for (var i = 0; i < listOfTherapistEvents.Count; i++)
            {
                _testTherapistEvents.Contains(listOfTherapistEvents[i]).Should().BeTrue();
            }
        }
        
        [TestMethod]
        public async Task GetAllTherapistEventsInRangeReturnsCorrectNumberOfTherapistEvents()
        {
            _testTherapistEvent.StartTime = _testTargetStartDateTime;
            _testTherapistEvent.EndTime = _testTargetEndDateTime;
            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEvents(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            listOfTherapistEvents.Count.Should().Be(3);
        }

        [TestMethod]
        public async Task GetAllTherapistEventsInRangeReturnsCorrectTherapistEvents()
        {
            _testTherapistEvent.StartTime = _testTargetStartDateTime;
            _testTherapistEvent.EndTime = _testTargetEndDateTime;
            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEvents(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            for (var i = 0; i < listOfTherapistEvents.Count; i++)
            {
                _testTherapistEvents.Contains(listOfTherapistEvents[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetAllTherapistEventsByTherapistIdReturnsCorrectType()
        {
            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEventsByTherapistId(_testTherapistEvent);

            allTherapistEvents.Should().BeOfType<List<TherapistEvent>>();
        }

        [TestMethod]
        public async Task GetAllTherapistEventsByTherapistIdReturnsCorrectNumberOfTherapistEvents()
        {
            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEventsByTherapistId(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            listOfTherapistEvents.Count.Should().Be(3);
        }

        [TestMethod]
        public async Task GetAllTherapistEventsByTherapistIdReturnsCorrectTherapistEvents()
        {
            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEventsByTherapistId(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            for (var i = 0; i < listOfTherapistEvents.Count; i++)
            {
                _testTherapistEvents.Contains(listOfTherapistEvents[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetAllTherapistEventsInRangeByTherapistIdReturnsCorrectNumberOfTherapistEvents()
        {
            _testTherapistEvent.StartTime = _testTargetStartDateTime;
            _testTherapistEvent.EndTime = _testTargetEndDateTime;
            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEventsByTherapistId(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            listOfTherapistEvents.Count.Should().Be(1);
        }

        [TestMethod]
        public async Task GetAllTherapistEventsInRangeByTherapistIdReturnsCorrectTherapistEvents()
        {
            _testTherapistEvent.StartTime = _testTargetStartDateTime;
            _testTherapistEvent.EndTime = _testTargetEndDateTime;
            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEventsByTherapistId(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            for (var i = 0; i < listOfTherapistEvents.Count; i++)
            {
                _testTherapistEvents.Contains(listOfTherapistEvents[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task AddTherapistEventReturnsCorrectType()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = _testUsers[0].UserId;

            var returnTherapistEvent = await _testTherapistEventService.AddTherapistEvent(newTherapistEvent);

            returnTherapistEvent.Should().BeOfType<TherapistEvent>();
        }

        [TestMethod]
        public async Task AddTherapistEventReturnsCorrectTherapistEvent()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = _testUsers[0].UserId;
           // newTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;

            var returnTherapistEvent = await _testTherapistEventService.AddTherapistEvent(newTherapistEvent);

            returnTherapistEvent.Should().Be(newTherapistEvent);
        }

        [TestMethod]
        public async Task AddTherapistEventIncreasesCountOfTherapistEvents()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = _testUsers[0].UserId;
            //newTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;

            await _testTherapistEventService.AddTherapistEvent(newTherapistEvent);
            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEvents(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            listOfTherapistEvents.Count.Should().Be(11);
        }

        [TestMethod]
        public async Task AddTherapistEventCorrectlyAddsTherapistEventToDatabase()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = _testUsers[0].UserId;
            //newTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;

            await _testTherapistEventService.AddTherapistEvent(newTherapistEvent);
            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEvents(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            listOfTherapistEvents.Contains(newTherapistEvent).Should().BeTrue();
        }

        [TestMethod]
        public async Task AddTherapistEventWithExistingEventIdThrowsTherapistEventEventIdAlreadyExistsException()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = _testUsers[0].UserId;
            //newTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;
            newTherapistEvent.EventId = _testTherapistEvents[0].EventId;

            await _testTherapistEventService.Invoking(s => s.AddTherapistEvent(newTherapistEvent)).Should().ThrowAsync<TherapistEventEventIdAlreadyExistsException>();
        }

        [TestMethod]
        public async Task AddTherapistEventWithNonExistingActivityNameThrowsTherapistActivityDoesNotExistException()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = _testUsers[0].UserId;
            newTherapistEvent.ActivityName = "-1";

            await _testTherapistEventService.Invoking(s => s.AddTherapistEvent(newTherapistEvent)).Should().ThrowAsync<TherapistActivityDoesNotExistException>();
        }

        [TestMethod]
        public async Task AddTherapistEventWithNonTherapistEventIdThrowsUserDoesNotExistException()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = -1;
            //newTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;

            await _testTherapistEventService.Invoking(s => s.AddTherapistEvent(newTherapistEvent)).Should().ThrowAsync<UserDoesNotExistException>();
        }

        [TestMethod]
        public async Task AddTherapistEventWithNonTherapistIdUserThrowsUserIsNotATherapistException()
        {
            var newTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            newTherapistEvent.TherapistId = _testNonTherapistUser.UserId;
            //newTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;
            
            await _testTherapistEventService.Invoking(s => s.AddTherapistEvent(newTherapistEvent)).Should().ThrowAsync<UserIsNotATherapistException>();
        }

        [TestMethod]
        public async Task DeleteTherapistEventReturnsCorrectType()
        {
            var returnTherapistEvent = await _testTherapistEventService.DeleteTherapistEvent(_testTherapistEvents[0].EventId);

            returnTherapistEvent.Should().BeOfType<TherapistEvent>();
        }

        [TestMethod]
        public async Task DeleteTherapistEventReturnsCorrectTherapistEvent()
        {
            var returnTherapistEvent = await _testTherapistEventService.DeleteTherapistEvent(_testTherapistEvents[0].EventId);

            returnTherapistEvent.Should().Be(returnTherapistEvent);
        }

        [TestMethod]
        public async Task DeleteTherapistEventDecrementsCountOfTherapistEvents()
        {
            await _testTherapistEventService.DeleteTherapistEvent(_testTherapistEvents[0].EventId);

            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEvents(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            listOfTherapistEvents.Count.Should().Be(9);
        }

        [TestMethod]
        public async Task DeleteTherapistEventRemovesTherapistEventFromDatabase()
        {
            await _testTherapistEventService.DeleteTherapistEvent(_testTherapistEvents[0].EventId);

            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEvents(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            listOfTherapistEvents.Contains(_testTherapistEvents[0]).Should().BeFalse();
        }

        [TestMethod]
        public async Task DeleteTherapistEventThatDoesNotExistReturnsNull()
        {
            var returnTherapistEvent = await _testTherapistEventService.DeleteTherapistEvent(-1);

            returnTherapistEvent.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateTherapistEventReturnsCorrectType()
        {
            var returnTherapistEvent = await _testTherapistEventService.UpdateTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0]);

            returnTherapistEvent.Should().BeOfType<TherapistEvent>();
        }

        [TestMethod]
        public async Task UpdateTherapistEventReturnsCorrectTherapistEvent()
        {
            var returnTherapistEvent = await _testTherapistEventService.UpdateTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0]);

            returnTherapistEvent.Should().Be(_testTherapistEvents[0]);
        }

        [TestMethod]
        public async Task UpdateTherapistEventWithAlteredDataUpdatesCorrectlyInDatabase()
        {
            var newEndDateTime = ModelFakes.TherapistEventFake.Generate().EndTime;
            _testTherapistEvents[0].EndTime = newEndDateTime;

            await _testTherapistEventService.UpdateTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0]);

            var allTherapistEvents = await _testTherapistEventService.GetAllTherapistEvents(_testTherapistEvent);
            List<TherapistEvent> listOfTherapistEvents = (List<TherapistEvent>)allTherapistEvents;

            listOfTherapistEvents.Contains(_testTherapistEvents[0]).Should().BeTrue();
        }

        [TestMethod]
        public async Task UpdateTherapistEventWithNonMatchingEventIdsThrowsTherapistEventEventIdsDoNotMatchException()
        {
            await _testTherapistEventService.Invoking(s => s.UpdateTherapistEvent(-1, _testTherapistEvents[0])).Should().ThrowAsync<TherapistEventEventIdsDoNotMatchException>();
        }

        [TestMethod]
        public async Task UpdateTherapistEventWithNonExistingActivityNameThrowsTherapistActivityDoesNotExistException()
        {
            _testTherapistEvents[0].ActivityName = "-1";

            await _testTherapistEventService.Invoking(s => s.UpdateTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0])).Should().ThrowAsync<TherapistActivityDoesNotExistException>();
        }

        [TestMethod]
        public async Task UpdateTherapistEventWithNonExistingTherapistIdThrowsUserDoesNotExistException()
        {
            _testTherapistEvents[0].TherapistId = -1;

            await _testTherapistEventService.Invoking(s => s.UpdateTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0])).Should().ThrowAsync<UserDoesNotExistException>();
        }

        [TestMethod]
        public async Task UpdateTherapistEventWithNonTherapistIdUserThrowsUserIsNotATherapistException()
        {
            _testTherapistEvents[0].TherapistId = _testNonTherapistUser.UserId;

            await _testTherapistEventService.Invoking(s => s.UpdateTherapistEvent(_testTherapistEvents[0].EventId, _testTherapistEvents[0])).Should().ThrowAsync<UserIsNotATherapistException>();
        }

        [TestMethod]
        public async Task UpdateTherapistEventThatDoesNotExistThrowsTherapistEventDoesNotExistException()
        {
            var fakeTherapistEvent = ModelFakes.TherapistEventFake.Generate();
            //fakeTherapistEvent.ActivityName = _testTherapistActivities[0].ActivityName;
            fakeTherapistEvent.TherapistId = _testUsers[0].UserId;

            await _testTherapistEventService.Invoking(s => s.UpdateTherapistEvent(fakeTherapistEvent.EventId, fakeTherapistEvent)).Should().ThrowAsync<TherapistEventDoesNotExistException>();
        }
    }
}
