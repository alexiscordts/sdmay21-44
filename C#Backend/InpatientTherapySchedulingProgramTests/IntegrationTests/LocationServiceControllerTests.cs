using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using InpatientTherapySchedulingProgram.Services;
using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgramTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InpatientTherapySchedulingProgramTests.IntegrationTests
{
    [TestClass]
    public class LocationServiceControllerTests
    {
        private List<Location> _testLocations;
        private Location _nonActiveLocation;
        private List<string> _testLocationNames;
        private CoreDbContext _testContext;
        private LocationService _testLocationService;
        private LocationController _testLocationController;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "LocationDatabase")
                .Options;
            _testLocations = new List<Location>();
            _testLocationNames = new List<string>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            for(var i = 0; i < 10; i++)
            {
                var newLocation = ModelFakes.LocationFake.Generate();
                _testLocationNames.Add(newLocation.Name);
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
            _testLocationController = new LocationController(_testLocationService);
        }

        [TestMethod]
        public async Task ValidGetAllLocationsReturnsOkResponse()
        {
            var response = await _testLocationController.GetLocation();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllLocationsReturnsCorrectType()
        {
            var response = await _testLocationController.GetLocation();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<Location>>();
        }

        [TestMethod]
        public async Task ValidGetAllLocationsReturnsCorrectCountOfLocations()
        {
            var response = await _testLocationController.GetLocation();
            var responseResult = response.Result as OkObjectResult;
            List<Location> listOfLocations = (List<Location>)responseResult.Value;

            listOfLocations.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task ValidGetAllLocationsReturnsCorrectLocations()
        {
            var response = await _testLocationController.GetLocation();
            var responseResult = response.Result as OkObjectResult;
            List<Location> listOfLocations = (List<Location>)responseResult.Value;

            for(var i = 0; i < listOfLocations.Count; i++)
            {
                _testLocations.Contains(listOfLocations[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetAllLocationNamesReturnsOkResponse()
        {
            var response = await _testLocationController.GetLocationNames();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllLocationNamesReturnsCorrectType()
        {
            var response = await _testLocationController.GetLocationNames();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<string>>();
        }

        [TestMethod]
        public async Task ValidGetAllLocationNamesReturnsCorrectCountOfLocationNames()
        {
            var response = await _testLocationController.GetLocationNames();
            var responseResult = response.Result as OkObjectResult;
            List<string> listOfLocationNames = (List<string>)responseResult.Value;

            listOfLocationNames.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task ValidGetAllLocationNamesReturnsCorrectLocationNames()
        {
            var response = await _testLocationController.GetLocationNames();
            var responseResult = response.Result as OkObjectResult;
            List<string> listOfLocationNames = (List<string>)responseResult.Value;

            for(var i = 0; i < listOfLocationNames.Count; i++)
            {
                _testLocationNames.Contains(listOfLocationNames[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetLocationByLocationidReturnsOkResponse()
        {
            var response = await _testLocationController.GetLocation(_testLocations[0].LocationId);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetLocationByLocationidReturnsCorrectType()
        {
            var response = await _testLocationController.GetLocation(_testLocations[0].LocationId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task ValidGetLocationByLocationidReturnsCorrectLocation()
        {
            var response = await _testLocationController.GetLocation(_testLocations[0].LocationId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testLocations[0]);
        }

        [TestMethod]
        public async Task NonExistingLocationGetLocationByLocationidReturnsNotFoundResponse()
        {
            var response = await _testLocationController.GetLocation(-1);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task NonActiveLocationGetLocationByLocationidReturnsNotFoundResponse()
        {
            var response = await _testLocationController.GetLocation(_nonActiveLocation.LocationId);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidGetLocationByNameReturnsOkResponse()
        {
            var response = await _testLocationController.GetLocation(_testLocations[0].Name);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetLocationByNameReturnsCorrectType()
        {
            var response = await _testLocationController.GetLocation(_testLocations[0].Name);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task ValidGetLocationByNameReturnsCorrectLocation()
        {
            var response = await _testLocationController.GetLocation(_testLocations[0].Name);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testLocations[0]);
        }

        [TestMethod]
        public async Task NonExistingLocationGetLocationByNameReturnsNotFoundResponse()
        {
            var response = await _testLocationController.GetLocation("-1");

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task NonActiveLocationGetLocationByNameReturnsNotFoundResponse()
        {
            var response = await _testLocationController.GetLocation(_nonActiveLocation.Name);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutLocationReturnsNoContentResponse()
        {
            var response = await _testLocationController.PutLocation(_testLocations[0].LocationId, _testLocations[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task ValidPutLocationCorrectlyUpdatesDataInDatabase()
        {
            string oldLocationName = _testLocations[0].Name;
            var newLocationName = ModelFakes.LocationFake.Generate().Name;
            _testLocations[0].Name = newLocationName;

            await _testLocationController.PutLocation(_testLocations[0].LocationId, _testLocations[0]);

            var response = await _testLocationController.GetLocation(_testLocations[0].LocationId);
            var responseResult = response.Result as OkObjectResult;
            Location location = (Location)responseResult.Value;

            location.Name.Should().NotBe(oldLocationName);
            location.Name.Should().Be(newLocationName);
            location.Should().Be(_testLocations[0]);
        }

        [TestMethod]
        public async Task NonMatchingLocationIdsPutLocationReturnsBadRequestResponse()
        {
            var response = await _testLocationController.PutLocation(-1, _testLocations[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingLocationPutLocationShouldReturnNotFoundResponse()
        {
            var fakeLocation = ModelFakes.LocationFake.Generate();

            var response = await _testLocationController.PutLocation(fakeLocation.LocationId, fakeLocation);

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task NonActiveLocationPutLocationShouldReturnNotFoundResponse()
        {
            var response = await _testLocationController.PutLocation(_nonActiveLocation.LocationId, _nonActiveLocation);

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPostLocationReturnsCreatedAtActionResponse()
        {
            var newLocation = ModelFakes.LocationFake.Generate();

            var response = await _testLocationController.PostLocation(newLocation);

            response.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostLocationReturnsCorrectType()
        {
            var newLocation = ModelFakes.LocationFake.Generate();

            var response = await _testLocationController.PostLocation(newLocation);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task ValidPostLocationReturnsCorrectLocation()
        {
            var newLocation = ModelFakes.LocationFake.Generate();

            var response = await _testLocationController.PostLocation(newLocation);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().Be(newLocation);
        }

        [TestMethod]
        public async Task ValidPostLocationCorrectlyAddsLocationToDatabase()
        {
            var newLocation = ModelFakes.LocationFake.Generate();

            await _testLocationController.PostLocation(newLocation);

            var response = await _testLocationController.GetLocation(newLocation.LocationId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(newLocation);
        }

        [TestMethod]
        public async Task ExistingLocationIdPostLocationReturnsConflictResponse()
        {
            var newLocation = ModelFakes.LocationFake.Generate();
            newLocation.LocationId = _testLocations[0].LocationId;

            var response = await _testLocationController.PostLocation(newLocation);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task ExistingLocationIdPostLocationDoesNotAddLocationToDatabase()
        {
            var newLocation = ModelFakes.LocationFake.Generate();
            newLocation.LocationId = _testLocations[0].LocationId;

            await _testLocationController.PostLocation(newLocation);

            var response = await _testLocationController.GetLocation(newLocation.LocationId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().NotBe(newLocation);
        }

        [TestMethod]
        public async Task ExistingNamePostLocationReturnsConflictResponse()
        {
            var newLocation = ModelFakes.LocationFake.Generate();
            newLocation.Name = _testLocations[0].Name;

            var response = await _testLocationController.PostLocation(newLocation);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task ExistingNamePostLocationDoesNotAddLocationToDatabase()
        {
            var newLocation = ModelFakes.LocationFake.Generate();
            newLocation.Name = _testLocations[0].Name;

            await _testLocationController.PostLocation(newLocation);

            var response = await _testLocationController.GetLocation(newLocation.LocationId);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidDeleteLocationReturnsOkResponse()
        {
            var response = await _testLocationController.DeleteLocation(_testLocations[0].LocationId);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteLocationReturnsCorrectType()
        {
            var response = await _testLocationController.DeleteLocation(_testLocations[0].LocationId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task ValidDeleteLocationReturnsLocation()
        {
            var response = await _testLocationController.DeleteLocation(_testLocations[0].LocationId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testLocations[0]);
        }

        [TestMethod]
        public async Task ValidDeleteLocationRemovesLocationFromDatabase()
        {
            await _testLocationController.DeleteLocation(_testLocations[0].LocationId);

            var response = await _testLocationController.GetLocation(_testLocations[0].LocationId);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task NonExistingLocationDeleteLocationReturnsNotFoundResponse()
        {
            var fakeLocation = ModelFakes.LocationFake.Generate();

            var response = await _testLocationController.DeleteLocation(fakeLocation.LocationId);

            response.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}
