using FluentAssertions;
using InpatientTherapySchedulingProgram.Exceptions.LocationExceptions;
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
    public class LocationServiceTests
    {
        private List<Location> _testLocations;
        private CoreDbContext _testContext;
        private LocationService _testLocationService;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "LocationDatabase")
                .Options;
            _testLocations = new List<Location>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            for(var i = 0; i < 10; i++)
            {
                var newLocation = ModelFakes.LocationFake.Generate();
                _testLocations.Add(ObjectExtensions.Copy(newLocation));
                _testContext.Add(newLocation);
                _testContext.SaveChanges();
            }

            _testLocationService = new LocationService(_testContext);
        }

        [TestMethod]
        public async Task GetAllLocationsReturnsCorrectType()
        {
            var allLocations = await _testLocationService.GetAllLocations();

            allLocations.Should().BeOfType<List<Location>>();
        }

        [TestMethod]
        public async Task GetAllLocationsReturnsNumberOfLocations()
        {
            var allLocations = await _testLocationService.GetAllLocations();
            List<Location> listOfLocations = (List<Location>)allLocations;

            listOfLocations.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task GetAllLocationsReturnsCorrectListOfLocations()
        {
            var allLocations = await _testLocationService.GetAllLocations();
            List<Location> listOfLocations = (List<Location>)allLocations;

            for(int i = 0; i < listOfLocations.Count; i++)
            {
                _testLocations.Contains(listOfLocations[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetAllLocationNamesReturnsCorrectType()
        {
            var allLocationNames = await _testLocationService.GetAllLocationNames();

            allLocationNames.Should().BeOfType<List<string>>();
        }

        [TestMethod]
        public async Task GetAllLocationNamesReturnsCorrectNames()
        {
            List<string> _testListOfLocationNames = new List<string>();

            for(var i = 0; i < _testLocations.Count; i++)
            {
                _testListOfLocationNames.Add(_testLocations[i].Name);
            }

            var allLocationNames = await _testLocationService.GetAllLocationNames();
            List<string> listOfLocationNames = (List<string>)allLocationNames;

            for(var i = 0; i < _testListOfLocationNames.Count; i++)
            {
                _testListOfLocationNames.Contains(listOfLocationNames[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetLocationByLidReturnsCorrectType()
        {
            var location = await _testLocationService.GetLocationByLid(_testLocations[0].Lid);

            location.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task GetLocationByLidReturnsCorrectLocation()
        {
            var location = await _testLocationService.GetLocationByLid(_testLocations[0].Lid);

            location.Should().Be(_testLocations[0]);
        }

        [TestMethod]
        public async Task GetLocationByLidReturnsNullIfLocationDoesNotExist()
        {
            var location = await _testLocationService.GetLocationByLid(-1);

            location.Should().BeNull();
        }

        [TestMethod]
        public async Task GetLocationByNameReturnsCorrectType()
        {
            var location = await _testLocationService.GetLocationByName(_testLocations[0].Name);

            location.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task GetLocationByNameReturnsCorrectLocation()
        {
            var location = await _testLocationService.GetLocationByName(_testLocations[0].Name);

            location.Should().Be(_testLocations[0]);
        }

        [TestMethod]
        public async Task GetLocationByNameReturnsNullIfLocationDoesExist()
        {
            var location = await _testLocationService.GetLocationByName("-1");

            location.Should().BeNull();
        }

        [TestMethod]
        public async Task AddLocationReturnsCorrectType()
        {
            var newLocation = ModelFakes.LocationFake.Generate();

            var returnLocation = await _testLocationService.AddLocation(newLocation);

            returnLocation.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task AddLocationIncreasesCountOfLocations()
        {
            var newLocation = ModelFakes.LocationFake.Generate();

            await _testLocationService.AddLocation(newLocation);

            var allLocations = await _testLocationService.GetAllLocations();
            List<Location> listOfLocations = (List<Location>)allLocations;

            listOfLocations.Count.Should().Be(11);
        }

        [TestMethod]
        public async Task AddLocationCorrectlyAddsLocationToDatabase()
        {
            var newLocation = ModelFakes.LocationFake.Generate();

            await _testLocationService.AddLocation(newLocation);

            var allLocations = await _testLocationService.GetAllLocations();
            List<Location> listOfLocations = (List<Location>)allLocations;

            listOfLocations.Contains(newLocation).Should().BeTrue();
        }

        [TestMethod]
        public async Task AddLocationWithExistingLidThrowsLocationIdAlreadyExistsException()
        {
            var newLocation = ModelFakes.LocationFake.Generate();
            newLocation.Lid = _testLocations[0].Lid;

            await _testLocationService.Invoking(s => s.AddLocation(newLocation)).Should().ThrowAsync<LocationIdAlreadyExistsException>();
        }

        [TestMethod]
        public async Task AddLocationWithExistingNameThrowsLocationNameAlreadyExistsException()
        {
            var newLocation = ModelFakes.LocationFake.Generate();
            newLocation.Name = _testLocations[0].Name;

            await _testLocationService.Invoking(s => s.AddLocation(newLocation)).Should().ThrowAsync<LocationNameAlreadyExistsException>();
        }

        [TestMethod]
        public async Task DeleteLocationReturnsCorrectType()
        {
            var location = await _testLocationService.DeleteLocation(_testLocations[0].Lid);

            location.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task DeleteLocationDecrementsCount()
        {
            await _testLocationService.DeleteLocation(_testLocations[0].Lid);

            var allLocations = await _testLocationService.GetAllLocations();
            List<Location> listOfLocations = (List<Location>)allLocations;

            listOfLocations.Count.Should().Be(9);
        }

        [TestMethod]
        public async Task DeleteLocationCorrectlyRemovesLocationFromDatabase()
        {
            await _testLocationService.DeleteLocation(_testLocations[0].Lid);

            var allLocations = await _testLocationService.GetAllLocations();
            List<Location> listOfLocations = (List<Location>)allLocations;

            listOfLocations.Contains(_testLocations[0]).Should().BeFalse();
        }

        [TestMethod]
        public async Task DeleteLocationThatDoesNotExistReturnsNull()
        {
            var location = await _testLocationService.DeleteLocation(-1);

            location.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateLocationReturnsCorrectType()
        {
            var location = await _testLocationService.UpdateLocation(_testLocations[0].Lid, _testLocations[0]);

            location.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task UpdateLocationReturnsCorrectLocation()
        {
            var location = await _testLocationService.UpdateLocation(_testLocations[0].Lid, _testLocations[0]);

            location.Should().Be(_testLocations[0]);
        }

        [TestMethod]
        public async Task UpdateLocationWithAlteredDataCorrectlyUpdatesLocationInDatabase()
        {
            var newLocationName = ModelFakes.LocationFake.Generate().Name;
            _testLocations[0].Name = newLocationName;

            await _testLocationService.UpdateLocation(_testLocations[0].Lid, _testLocations[0]);

            var location = await _testLocationService.GetLocationByLid(_testLocations[0].Lid);

            location.Name.Should().Be(newLocationName);
        }

        [TestMethod]
        public async Task UpdateLocationWithNonMatchingLocationLidsThrowsLocationIdsDoNotMatchExceptionException()
        {
            await _testLocationService.Invoking(s => s.UpdateLocation(-1, _testLocations[0])).Should().ThrowAsync<LocationIdsDoNotMatchException>();
        }

        [TestMethod]
        public async Task UpdateLocationWithNonExistingLocationLocationLidsThrowsLocationDoesNotExistException()
        {
            var fakeLocation = ModelFakes.LocationFake.Generate();

            await _testLocationService.Invoking(s => s.UpdateLocation(fakeLocation.Lid, fakeLocation)).Should().ThrowAsync<LocationDoesNotExistException>();
        }
    }
}
