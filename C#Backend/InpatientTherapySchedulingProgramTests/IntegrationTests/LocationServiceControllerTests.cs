using System;
using System.Collections.Generic;
using System.Text;
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
                _testLocations.Add(ObjectExtensions.Copy(newLocation));
                _testLocationNames.Add(newLocation.Name);
                _testContext.Add(newLocation);
                _testContext.SaveChanges();
            }

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

            for(var i = 0; i < 10; i++)
            {
                _testLocationNames.Contains(listOfLocationNames[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetLocationByLidReturnsOkResponse()
        {
            var response = await _testLocationController.GetLocation(_testLocations[0].Lid);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetLocationByLidReturnsCorrectType()
        {
            var response = await _testLocationController.GetLocation(_testLocations[0].Lid);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task ValidGetLocationByLidReturnsCorrectLocation()
        {
            var response = await _testLocationController.GetLocation(_testLocations[0].Lid);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testLocations[0]);
        }

        [TestMethod]
        public async Task NonExistingLocationGetLocationByLidReturnsNotFoundResponse()
        {
            var response = await _testLocationController.GetLocation(-1);

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
        public async Task ValidPutLocationReturnsNoContentResponse()
        {
            var response = await _testLocationController.PutLocation(_testLocations[0].Lid, _testLocations[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task ValidPutLocationCorrectlyUpdatesDataInDatabase()
        {
            string oldLocationName = _testLocations[0].Name;
            var newLocationName = ModelFakes.LocationFake.Generate().Name;
            _testLocations[0].Name = newLocationName;

            await _testLocationController.PutLocation(_testLocations[0].Lid, _testLocations[0]);

            var response = await _testLocationController.GetLocation(_testLocations[0].Lid);
            var responseResult = response.Result as OkObjectResult;
            Location location = (Location)responseResult.Value;

            location.Name.Should().NotBe(oldLocationName);
            location.Name.Should().Be(newLocationName);
            location.Should().Be(_testLocations[0]);
        }

        [TestMethod]
        public async Task NonMatchingLidsPutLocationReturnsBadRequestResponse()
        {
            var response = await _testLocationController.PutLocation(-1, _testLocations[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingLocationPutLocationShouldReturnNotFoundResponse()
        {
            var fakeLocation = ModelFakes.LocationFake.Generate();

            var response = await _testLocationController.PutLocation(fakeLocation.Lid, fakeLocation);

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

            var response = await _testLocationController.GetLocation(newLocation.Lid);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(newLocation);
        }

        [TestMethod]
        public async Task ExistingLidPostLocationReturnsConflictResponse()
        {
            var newLocation = ModelFakes.LocationFake.Generate();
            newLocation.Lid = _testLocations[0].Lid;

            var response = await _testLocationController.PostLocation(newLocation);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task ExistingLidPostLocationDoesNotAddLocationToDatabase()
        {
            var newLocation = ModelFakes.LocationFake.Generate();
            newLocation.Lid = _testLocations[0].Lid;

            await _testLocationController.PostLocation(newLocation);

            var response = await _testLocationController.GetLocation(newLocation.Lid);
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

            var response = await _testLocationController.GetLocation(newLocation.Lid);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidDeleteLocationReturnsOkResponse()
        {
            var response = await _testLocationController.DeleteLocation(_testLocations[0].Lid);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteLocationReturnsCorrectType()
        {
            var response = await _testLocationController.DeleteLocation(_testLocations[0].Lid);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Location>();
        }

        [TestMethod]
        public async Task ValidDeleteLocationReturnsLocation()
        {
            var response = await _testLocationController.DeleteLocation(_testLocations[0].Lid);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testLocations[0]);
        }

        [TestMethod]
        public async Task ValidDeleteLocationRemovesLocationFromDatabase()
        {
            await _testLocationController.DeleteLocation(_testLocations[0].Lid);

            var response = await _testLocationController.GetLocation(_testLocations[0].Lid);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task NonExistingLocationDeleteLocationReturnsNotFoundResponse()
        {
            var fakeLocation = ModelFakes.LocationFake.Generate();

            var response = await _testLocationController.DeleteLocation(fakeLocation.Lid);

            response.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}
