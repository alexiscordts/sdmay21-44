using FluentAssertions;
using InpatientTherapySchedulingProgram.Exceptions.TherapistActivityExceptions;
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
    public class TherapistActivityServiceTests
    {
        private List<TherapistActivity> _testTherapistActivities;
        private CoreDbContext _testContext;
        private TherapistActivityService _testTherapistActivityService;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TherapistActivityDatabase")
                .Options;
            _testTherapistActivities = new List<TherapistActivity>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            for(var i = 0; i < 10; i++)
            {
                var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();
                _testTherapistActivities.Add(ObjectExtensions.Copy(newTherapistActivity));
                _testContext.Add(newTherapistActivity);
                _testContext.SaveChanges();
            }

            _testTherapistActivityService = new TherapistActivityService(_testContext);
        }

        [TestMethod]
        public async Task GetAllTherapistActivitesReturnsCorrectType()
        {
            var allTherapistActivities = await _testTherapistActivityService.GetAllTherapistActivities();

            allTherapistActivities.Should().BeOfType<List<TherapistActivity>>();
        }

        [TestMethod]
        public async Task GetAllTherapistActivitesReturnsCorrectNumberOfTherapists()
        {
            var allTherapistActivities = await _testTherapistActivityService.GetAllTherapistActivities();
            List<TherapistActivity> listOfTherapistActivites = (List<TherapistActivity>)allTherapistActivities;

            listOfTherapistActivites.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task GetAllTherapistActivitesReturnsCorrectListOfTherapistActivity()
        {
            var allTherapistActivites = await _testTherapistActivityService.GetAllTherapistActivities();
            List<TherapistActivity> listOfTherapistActivities = (List<TherapistActivity>)allTherapistActivites;

            for(var i = 0; i < listOfTherapistActivities.Count; i++)
            {
                _testTherapistActivities.Contains(listOfTherapistActivities[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetTherapistActivityByNameReturnsCorrectType()
        {
            var therapistActivity = await _testTherapistActivityService.GetTherapistActivityByName(_testTherapistActivities[0].ActivityName);

            therapistActivity.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task GetTherapistActivityByNameReturnsCorrectTherapistActivity()
        {
            var therapistActivity = await _testTherapistActivityService.GetTherapistActivityByName(_testTherapistActivities[0].ActivityName);

            therapistActivity.Should().Be(_testTherapistActivities[0]);
        }

        [TestMethod]
        public async Task GetTherapistActivityReturnsNullIfTherapistActivityDoesNotExist()
        {
            var therapistActivity = await _testTherapistActivityService.GetTherapistActivityByName("-1");

            therapistActivity.Should().BeNull();
        }

        [TestMethod]
        public async Task AddTherapistActivityReturnsCorrectType()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            var returnTherapistActivity = await _testTherapistActivityService.AddTherapistActivity(newTherapistActivity);

            returnTherapistActivity.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task AddTherapistActivityIncreasesCountOfTherapistActivites()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            await _testTherapistActivityService.AddTherapistActivity(newTherapistActivity);

            var allTherapistActivities = await _testTherapistActivityService.GetAllTherapistActivities();
            List<TherapistActivity> listOfTherapistActivities = (List<TherapistActivity>)allTherapistActivities;

            listOfTherapistActivities.Count.Should().Be(11);
        }

        [TestMethod]
        public async Task AddTherapistActivityCorrectlyAddsTherapistActivityToDatabase()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            await _testTherapistActivityService.AddTherapistActivity(newTherapistActivity);

            var allTherapistActivities = await _testTherapistActivityService.GetAllTherapistActivities();
            List<TherapistActivity> listOfTherapistActivities = (List<TherapistActivity>)allTherapistActivities;

            listOfTherapistActivities.Contains(newTherapistActivity).Should().BeTrue();
        }

        [TestMethod]
        public async Task AddTherapistActivityWithExistingNameThrowsError()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();
            newTherapistActivity.ActivityName = _testTherapistActivities[0].ActivityName;

            await _testTherapistActivityService.Invoking(s => s.AddTherapistActivity(newTherapistActivity)).Should().ThrowAsync<TherapistActivityAlreadyExistsException>();
        }

        [TestMethod]
        public async Task DeleteTherapistActivityReturnsCorrectType()
        {
            var therapistActivity = await _testTherapistActivityService.DeleteTherapistActivity(_testTherapistActivities[0].ActivityName);

            therapistActivity.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task DeleteTherapistActivityDecrementsCount()
        {
            await _testTherapistActivityService.DeleteTherapistActivity(_testTherapistActivities[0].ActivityName);

            var allTherapistActivities = await _testTherapistActivityService.GetAllTherapistActivities();
            List<TherapistActivity> listOfTherapistActivities = (List<TherapistActivity>)allTherapistActivities;

            listOfTherapistActivities.Count.Should().Be(9);
        }

        [TestMethod]
        public async Task DeleteTherapistActivityRemovesTherapistActivityFromDatabase()
        {
            await _testTherapistActivityService.DeleteTherapistActivity(_testTherapistActivities[0].ActivityName);

            var allTherapistActivites = await _testTherapistActivityService.GetAllTherapistActivities();
            List<TherapistActivity> listOfTherapistActivities = (List<TherapistActivity>)allTherapistActivites;

            listOfTherapistActivities.Contains(_testTherapistActivities[0]).Should().BeFalse();
        }

        [TestMethod]
        public async Task DeleteTherapistActivityThatDoesNotExistReturnsNull()
        {
            var therapistActivity = await _testTherapistActivityService.DeleteTherapistActivity("-1");

            therapistActivity.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateTherapistActivityReturnsCorrectType()
        {
            var therapistActivity = await _testTherapistActivityService.UpdateTherapistActivity(_testTherapistActivities[0].ActivityName, _testTherapistActivities[0]);

            therapistActivity.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task UpdateTherapistActivityWithAlteredDataCorrectlyUpdatesInDatabase()
        {
            bool oldIsProductive = (bool)_testTherapistActivities[0].IsProductive;
            _testTherapistActivities[0].IsProductive = !_testTherapistActivities[0].IsProductive;

            await _testTherapistActivityService.UpdateTherapistActivity(_testTherapistActivities[0].ActivityName, _testTherapistActivities[0]);

            var therapistActivity = await _testTherapistActivityService.GetTherapistActivityByName(_testTherapistActivities[0].ActivityName);

            therapistActivity.IsProductive.Should().Be(!oldIsProductive);
        }

        [TestMethod]
        public async Task UpdateTherapistActivityWithNonMatchingNamesThrowsError()
        {
            await _testTherapistActivityService.Invoking(s => s.UpdateTherapistActivity("-1", _testTherapistActivities[0])).Should().ThrowAsync<TherapistActivityNamesDoNotMatchException>();
        }

        [TestMethod]
        public async Task UpdateTherapistActivityWithNonExistingTherapistActivityThrowsError()
        {
            var fakeTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            await _testTherapistActivityService.Invoking(s => s.UpdateTherapistActivity(fakeTherapistActivity.ActivityName, fakeTherapistActivity)).Should().ThrowAsync<TherapistActivityDoesNotExistException>();
        }
    }
}
