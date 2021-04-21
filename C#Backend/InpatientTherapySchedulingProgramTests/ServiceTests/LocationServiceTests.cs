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
        private Location _nonActiveLocation;
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
                _testContext.Add(newLocation);
                _testContext.SaveChanges();
                _testLocations.Add(ObjectExtensions.Copy(newLocation));
            }

            _nonActiveLocation = ModelFakes.LocationFake.Generate();
            _nonActiveLocation.Active = false;
            _testContext.Add(_nonActiveLocation);
            _testContext.SaveChanges();
            _testLocations.Add(ObjectExtensions.Copy(_nonActiveLocation));

            _testLocationService = new LocationService(_testContext);
        }

        [TestMethod]
        public async Task GetAllLocationsReturnsCorrectType()
        {
            var allLocations = await _testLocationService.GetAllLocations();

            allLocations.Should().BeOfType<List<Location>>();
        }

        [TestMethod]
        public async Task GetAllLocationsReturnsCorrectNumberOfLocations()
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

            for(var i = 0; i < listOfLocationNames.Count; i++)
            {
                _testListOfLocationNames.Contains(listOfLocationNames[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetLocationByLocationIdReturnsCorrectType()
        {
            var location = await _testLocationService.GetLocationByLocationId(_testLocations[0].LocationId);

            location.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task GetLocationByLocationIdReturnsCorrectLocation()
        {
            var location = await _testLocationService.GetLocationByLocationId(_testLocations[0].LocationId);

            location.Should().Be(_testLocations[0]);
        }

        [TestMethod]
        public async Task GetLocationByLocationIdReturnsNullIfLocationDoesNotExist()
        {
            var location = await _testLocationService.GetLocationByLocationId(-1);

            location.Should().BeNull();
        }

        [TestMethod]
        public async Task GetLocationByLocationIdReturnsNullIfLocationIsNotActive()
        {
            var location = await _testLocationService.GetLocationByLocationId(_nonActiveLocation.LocationId);

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
        public async Task GetLocationByNameReturnsNullIfLocationDoesNotExist()
        {
            var location = await _testLocationService.GetLocationByName("-1");

            location.Should().BeNull();
        }

        [TestMethod]
        public async Task GetLocationByNameReturnsNullIfLocationIsNotActive()
        {
            var location = await _testLocationService.GetLocationByName(_nonActiveLocation.Name);

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
        public async Task AddLocationWithExistingLocationIdThrowsLocationIdAlreadyExistsException()
        {
            var newLocation = ModelFakes.LocationFake.Generate();
            newLocation.LocationId = _testLocations[0].LocationId;

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
            var location = await _testLocationService.DeleteLocation(_testLocations[0].LocationId);

            location.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task DeleteLocationDecrementsCount()
        {
            await _testLocationService.DeleteLocation(_testLocations[0].LocationId);

            var allLocations = await _testLocationService.GetAllLocations();
            List<Location> listOfLocations = (List<Location>)allLocations;

            listOfLocations.Count.Should().Be(9);
        }

        [TestMethod]
        public async Task DeleteLocationCorrectlyRemovesLocationFromDatabase()
        {
            await _testLocationService.DeleteLocation(_testLocations[0].LocationId);

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
            var location = await _testLocationService.UpdateLocation(_testLocations[0].LocationId, _testLocations[0]);

            location.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task UpdateLocationReturnsCorrectLocation()
        {
            var location = await _testLocationService.UpdateLocation(_testLocations[0].LocationId, _testLocations[0]);

            location.Should().Be(_testLocations[0]);
        }

        [TestMethod]
        public async Task UpdateLocationWithAlteredDataCorrectlyUpdatesLocationInDatabase()
        {
            var newLocationName = ModelFakes.LocationFake.Generate().Name;
            _testLocations[0].Name = newLocationName;

            await _testLocationService.UpdateLocation(_testLocations[0].LocationId, _testLocations[0]);

            var location = await _testLocationService.GetLocationByLocationId(_testLocations[0].LocationId);

            location.Name.Should().Be(newLocationName);
        }

        [TestMethod]
        public async Task UpdateLocationWithNonMatchingLocationLocationIdsThrowsLocationIdsDoNotMatchExceptionException()
        {
            await _testLocationService.Invoking(s => s.UpdateLocation(-1, _testLocations[0])).Should().ThrowAsync<LocationIdsDoNotMatchException>();
        }

        [TestMethod]
        public async Task UpdateLocationWithNonExistingLocationThrowsLocationDoesNotExistException()
        {
            var fakeLocation = ModelFakes.LocationFake.Generate();

            await _testLocationService.Invoking(s => s.UpdateLocation(fakeLocation.LocationId, fakeLocation)).Should().ThrowAsync<LocationDoesNotExistException>();
        }

        [TestMethod]
        public async Task UpdateLocationWithNonActiveLocationThrowsLocationDoesNotExistException()
        {
            await _testLocationService.Invoking(s => s.UpdateLocation(_nonActiveLocation.LocationId, _nonActiveLocation)).Should().ThrowAsync<LocationDoesNotExistException>();
        }
    }
}
