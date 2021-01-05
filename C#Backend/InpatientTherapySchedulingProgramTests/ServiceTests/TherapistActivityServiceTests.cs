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
        private TherapistActivityService _testService;

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

            _testService = new TherapistActivityService(_testContext);
        }

        [TestMethod]
        public async Task GetAllTherapistActivitesReturnsCorrectType()
        {
            var allTherapistActivities = await _testService.GetAllTherapistActivities();

            allTherapistActivities.Should().BeOfType<List<TherapistActivity>>();
        }

        [TestMethod]
        public async Task GetAllTherapistActivitesReturnsCorrectNumberOfTherapists()
        {
            var allTherapistActivities = await _testService.GetAllTherapistActivities();
            List<TherapistActivity> listOfTherapistActivites = (List<TherapistActivity>)allTherapistActivities;

            listOfTherapistActivites.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task GetAllTherapistActivitesReturnsCorrectListOfTherapistActivity()
        {
            var allTherapistActivites = await _testService.GetAllTherapistActivities();
            List<TherapistActivity> listOfTherapistActivities = (List<TherapistActivity>)allTherapistActivites;

            for(var i = 0; i < listOfTherapistActivities.Count; i++)
            {
                _testTherapistActivities.Contains(listOfTherapistActivities[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetTherapistActivityByNameReturnsCorrectType()
        {
            var therapistActivity = await _testService.GetTherapistActivityByName(_testTherapistActivities[0].Name);

            therapistActivity.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task GetTherapistActivityByNameReturnsCorrectTherapistActivity()
        {
            var therapistActivity = await _testService.GetTherapistActivityByName(_testTherapistActivities[0].Name);

            therapistActivity.Should().Be(_testTherapistActivities[0]);
        }

        [TestMethod]
        public async Task GetTherapistActivityReturnsNullIfTherapistActivityDoesNotExist()
        {
            var therapistActivity = await _testService.GetTherapistActivityByName("-1");

            therapistActivity.Should().BeNull();
        }

        [TestMethod]
        public async Task AddTherapistActivityReturnsCorrectType()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            var returnTherapistActivity = await _testService.AddTherapistActivity(newTherapistActivity);

            returnTherapistActivity.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task AddTherapistActivityIncreasesCountOfTherapistActivites()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            await _testService.AddTherapistActivity(newTherapistActivity);

            var allTherapistActivities = await _testService.GetAllTherapistActivities();
            List<TherapistActivity> listOfTherapistActivities = (List<TherapistActivity>)allTherapistActivities;

            listOfTherapistActivities.Count.Should().Be(11);
        }

        [TestMethod]
        public async Task AddTherapistActivityCorrectlyAddsTherapistActivityToDatabase()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            await _testService.AddTherapistActivity(newTherapistActivity);

            var allTherapistActivities = await _testService.GetAllTherapistActivities();
            List<TherapistActivity> listOfTherapistActivities = (List<TherapistActivity>)allTherapistActivities;

            listOfTherapistActivities.Contains(newTherapistActivity).Should().BeTrue();
        }

        [TestMethod]
        public async Task AddTherapistActivityWithExistingNameThrowsError()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();
            newTherapistActivity.Name = _testTherapistActivities[0].Name;

            await _testService.Invoking(s => s.AddTherapistActivity(newTherapistActivity)).Should().ThrowAsync<TherapistActivityAlreadyExistsException>();
        }

        [TestMethod]
        public async Task DeleteTherapistActivityReturnsCorrectType()
        {
            var therapistActivity = await _testService.DeleteTherapistActivity(_testTherapistActivities[0].Name);

            therapistActivity.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task DeleteTherapistActivityDecrementsCount()
        {
            await _testService.DeleteTherapistActivity(_testTherapistActivities[0].Name);

            var allTherapistActivities = await _testService.GetAllTherapistActivities();
            List<TherapistActivity> listOfTherapistActivities = (List<TherapistActivity>)allTherapistActivities;

            listOfTherapistActivities.Count.Should().Be(9);
        }

        [TestMethod]
        public async Task DeleteTherapistActivityRemovesTherapistActivityFromDatabase()
        {
            await _testService.DeleteTherapistActivity(_testTherapistActivities[0].Name);

            var allTherapistActivites = await _testService.GetAllTherapistActivities();
            List<TherapistActivity> listOfTherapistActivities = (List<TherapistActivity>)allTherapistActivites;

            listOfTherapistActivities.Contains(_testTherapistActivities[0]).Should().BeFalse();
        }

        [TestMethod]
        public async Task DeleteTherapistActivityThatDoesNotExistReturnsNull()
        {
            var therapistActivity = await _testService.DeleteTherapistActivity("-1");

            therapistActivity.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateTherapistActivityReturnsCorrectType()
        {
            var therapistActivity = await _testService.UpdateTherapistActivity(_testTherapistActivities[0].Name, _testTherapistActivities[0]);

            therapistActivity.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task UpdateTherapistActivityWithAlteredDataCorrectlyUpdatesInDatabase()
        {
            bool oldIsProductive = (bool)_testTherapistActivities[0].IsProductive;
            _testTherapistActivities[0].IsProductive = !_testTherapistActivities[0].IsProductive;

            await _testService.UpdateTherapistActivity(_testTherapistActivities[0].Name, _testTherapistActivities[0]);

            var therapistActivity = await _testService.GetTherapistActivityByName(_testTherapistActivities[0].Name);

            therapistActivity.IsProductive.Should().Be(!oldIsProductive);
        }

        [TestMethod]
        public async Task UpdateTherapistActivityWithNonMatchingNamesThrowsError()
        {
            await _testService.Invoking(s => s.UpdateTherapistActivity("-1", _testTherapistActivities[0])).Should().ThrowAsync<TherapistActivityNamesDoNotMatchException>();
        }

        [TestMethod]
        public async Task UpdateTherapistWithNonExistingTherapistActivityThrowsError()
        {
            var therapistActivity = ModelFakes.TherapistActivityFake.Generate();

            await _testService.Invoking(s => s.UpdateTherapistActivity(therapistActivity.Name, therapistActivity)).Should().ThrowAsync<TherapistActivityDoesNotExistException>();
        }
    }
}
