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
    public class RoomServiceControllerTests
    {
        private List<Room> _testRooms;
        private Room _nonActiveRooms;
        private CoreDbContext _testContext;
        private RoomService _testRoomService;
        private RoomController _testRoomController;

        [TestInitialize]
        public void Initialize() {
            var options = new DbContextOptionsBuilder<CoreDbContext>().UseInMemoryDatabase(databaseName: "RoomDatabase").Options;
            _testRooms = new List<Room>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            for (var i = 0; i < 10; i++) {
                var newRoom = ModelFakes.RoomFake.Generate();
                _testContext.Add(newRoom);
                _testContext.SaveChanges();
                _testRooms.Add(ObjectExtensions.Copy(newRoom));
            }

            _nonActiveRooms = ModelFakes.RoomFake.Generate();
            _nonActiveRooms.Active = false;
            _testContext.Add(_nonActiveRooms);
            _testContext.SaveChanges();
            _testRooms.Add(ObjectExtensions.Copy(_nonActiveRooms));

            _testRoomService = new RoomService(_testContext);
            _testRoomController = new RoomController(_testRoomService);
        }

        [TestMethod]
        public async Task ValidGetAllRoomsReturnOkResponse() {
            var response = await _testRoomController.GetRooms();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllRoomsReturnsCorrectType()
        {
            var response = await _testRoomController.GetRooms();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<Room>>();
        }

        [TestMethod]
        public async Task ValidGetAllRoomsReturnsCorrectCountOfRooms() {
            var response = await _testRoomController.GetRooms();
            var responseResult = response.Result as OkObjectResult;
            List<Room> listOfRooms = (List<Room>)responseResult.Value;

            listOfRooms.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task ValidGetAllRoomsReturnsCorrectLocation() {
            var response = await _testRoomController.GetRooms();
            var responseResult = response.Result as OkObjectResult;
            List<Room> listOfRooms = (List<Room>)responseResult.Value;

            for (var i = 0; i < listOfRooms.Count; i++) {
                _testRooms.Contains(listOfRooms[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetRoomsByLocationidReturnsOkResponse() {
            var response = await _testRoomController.GetRoomsByLocationId(_testRooms[0].LocationId);
            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetRoomsByLocationidReturnsCorrectType() {
            var response = await _testRoomController.GetRoomsByLocationId(_testRooms[0].LocationId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<Room>>();
        
        }

        [TestMethod]
        public async Task NonExistingRoomGetRoomsByLocationifReturnsNotFoundResponse() {
            var response = await _testRoomController.GetRoomsByLocationId(-1);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutRoomReturnsNoContentResponse() {
            var response = await _testRoomController.PutRoom(_testRooms[0].Number, _testRooms[0]);

            response.Should().BeOfType<NoContentResult>();
        }

    }
}
