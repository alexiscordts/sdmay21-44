using FluentAssertions;
using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Exceptions.LocationExceptions;
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
    public class LocationControllerTests
    {
        private static List<Location> _testLocations;
        private static List<string> _testLocationNames;
        private Mock<ILocationService> _fakeLocationService;
        private LocationController _testLocationController;

        [ClassInitialize()]
        public static void Setup(TestContext context)
        {
            _testLocations = new List<Location>();
            _testLocationNames = new List<string>();

            for(var i = 0; i < 10; i++)
            {
                var newLocation = ModelFakes.LocationFake.Generate();
                _testLocations.Add(newLocation);
                _testLocationNames.Add(newLocation.Name);
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            _fakeLocationService = new Mock<ILocationService>();
            _fakeLocationService.SetupAllProperties();
            _fakeLocationService.Setup(s => s.GetAllLocations()).ReturnsAsync(_testLocations);
            _fakeLocationService.Setup(s => s.GetAllLocationNames()).ReturnsAsync(_testLocationNames);
            _fakeLocationService.Setup(s => s.GetLocationByLocationId(It.IsAny<int>())).ReturnsAsync(_testLocations[0]);
            _fakeLocationService.Setup(s => s.GetLocationByName(It.IsAny<string>())).ReturnsAsync(_testLocations[0]);
            _fakeLocationService.Setup(s => s.UpdateLocation(It.IsAny<int>(), It.IsAny<Location>())).ReturnsAsync(_testLocations[0]);
            _fakeLocationService.Setup(s => s.AddLocation(It.IsAny<Location>())).ReturnsAsync(_testLocations[0]);
            _fakeLocationService.Setup(s => s.DeleteLocation(It.IsAny<int>())).ReturnsAsync(_testLocations[0]);

            _testLocationController = new LocationController(_fakeLocationService.Object);
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
        public async Task ValidGetLocationNamesReturnsOkResponse()
        {
            var response = await _testLocationController.GetLocationNames();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetLocationNamesReturnsCorrectType()
        {
            var response = await _testLocationController.GetLocationNames();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<string>>();
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
        public async Task NullGetLocationByLocationidReturnsNotFoundResponse()
        {
            _fakeLocationService.Setup(s => s.GetLocationByLocationId(It.IsAny<int>())).ReturnsAsync((Location)null);

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
        public async Task NullGetLocationByNameReturnsNotFoundResponse()
        {
            _fakeLocationService.Setup(s => s.GetLocationByName(It.IsAny<string>())).ReturnsAsync((Location)null);

            var response = await _testLocationController.GetLocation("-1");

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutLocationReturnsNoContentResponse()
        {
            var response = await _testLocationController.PutLocation(_testLocations[0].LocationId, _testLocations[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task LocationIdsDoNotMatchExceptionPutTherapyReturnsBadRequestResponse()
        {
            _fakeLocationService.Setup(s => s.UpdateLocation(It.IsAny<int>(), It.IsAny<Location>())).ThrowsAsync(new LocationIdsDoNotMatchException());

            var response = await _testLocationController.PutLocation(_testLocations[0].LocationId, _testLocations[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task LocationDoesNotExistExceptionPutTherapyReturnsNotFoundResponse()
        {
            _fakeLocationService.Setup(s => s.UpdateLocation(It.IsAny<int>(), It.IsAny<Location>())).ThrowsAsync(new LocationDoesNotExistException());

            var response = await _testLocationController.PutLocation(_testLocations[0].LocationId, _testLocations[0]);

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionPutTherapyThrowsError()
        {
            _fakeLocationService.Setup(s => s.UpdateLocation(It.IsAny<int>(), It.IsAny<Location>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testLocationController.Invoking(c => c.PutLocation(_testLocations[0].LocationId, _testLocations[0])).Should().ThrowAsync<DbUpdateConcurrencyException>();
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
        public async Task LocationIdAlreadyExistsExceptionPostLocationReturnsConflictResponse()
        {
            _fakeLocationService.Setup(s => s.AddLocation(It.IsAny<Location>())).ThrowsAsync(new LocationIdAlreadyExistsException());

            var newLocation = ModelFakes.LocationFake.Generate();
            newLocation.LocationId = _testLocations[0].LocationId;

            var response = await _testLocationController.PostLocation(newLocation);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task LocationNameAlreadyExistsExceptionPostLocationReturnsConflictResponse()
        {
            _fakeLocationService.Setup(s => s.AddLocation(It.IsAny<Location>())).ThrowsAsync(new LocationNameAlreadyExistsException());

            var newLocation = ModelFakes.LocationFake.Generate();
            newLocation.Name = _testLocations[0].Name;

            var response = await _testLocationController.PostLocation(newLocation);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionPostLocationReturnsConflictResponse()
        {
            _fakeLocationService.Setup(s => s.AddLocation(It.IsAny<Location>())).ThrowsAsync(new DbUpdateException());

            var newLocation = ModelFakes.LocationFake.Generate();

            await _testLocationController.Invoking(s => s.PostLocation(newLocation)).Should().ThrowAsync<DbUpdateException>();
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
        public async Task NullDeleteLocationReturnsNotFoundResponse()
        {
            _fakeLocationService.Setup(s => s.DeleteLocation(It.IsAny<int>())).ReturnsAsync((Location)null);

            var response = await _testLocationController.DeleteLocation(-1);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionDeleteLocationThrowsError()
        {
            _fakeLocationService.Setup(s => s.DeleteLocation(It.IsAny<int>())).ThrowsAsync(new DbUpdateException());

            await _testLocationController.Invoking(c => c.DeleteLocation(-1)).Should().ThrowAsync<DbUpdateException>();
        }
    }
}
