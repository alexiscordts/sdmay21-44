using FluentAssertions;
using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Exceptions.RoomException;
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
    public class RoomControllerTests
    {

        private static List<Room> _testRooms;
        private Mock<IRoomService> _fakeService;
        private RoomController _testController;

        [ClassInitialize()]
        public static void ClassSetup(TestContext context) {
            _testRooms = new List<Room>();

            for (var i = 0; i < 10; i++) {
                var room = ModelFakes.RoomFake.Generate();
                _testRooms.Add(room);
            }
        }

        [TestInitialize]
        public void Initialize() {
            _fakeService = new Mock<IRoomService>();
            _fakeService.SetupAllProperties();
            _fakeService.Setup(s => s.GetAllRooms()).ReturnsAsync(_testRooms);
            _fakeService.Setup(s => s.GetAllRoomsByLocationId(It.IsAny<int>())).ReturnsAsync(_testRooms);
            _fakeService.Setup(s => s.AddRoom(It.IsAny<Room>())).ReturnsAsync(_testRooms[0]);
            _fakeService.Setup(s => s.UpdateRoom(It.IsAny<int>(), It.IsAny<Room>())).ReturnsAsync(_testRooms[0]);
            _fakeService.Setup(s => s.DeleteRoom(It.IsAny<Room>())).ReturnsAsync(_testRooms[0]);

            _testController = new RoomController(_fakeService.Object);
        }

        [TestMethod]
        public async Task ValidGetAllRoomsReturnOkResponse() {
            var response = await _testController.GetRooms();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllRoomsReturnsCorrectType() {
            var response = await _testController.GetRooms();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<Room>>();
        }

        [TestMethod]
        public async Task ValidGetRoomByLocationidReturnsOkResponse() {
            var response = await _testController.GetRoomsByLocationId(_testRooms[0].LocationId);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetRoomByLocationidReturnsCorrectType() {
            var response = await _testController.GetRoomsByLocationId(_testRooms[0].LocationId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<Room>>();
        }

        [TestMethod]
        public async Task NullGetRoomByLocationIdReturnsNotFound() {
            List<Room> listOfRooms = new List<Room>();
            
            _fakeService.Setup(s => s.GetAllRoomsByLocationId(It.IsAny<int>())).ReturnsAsync(listOfRooms);

            var response = await _testController.GetRoomsByLocationId(-1);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutRoomReturnsNoContentResponse() {
            var response = await _testController.PutRoom(_testRooms[0].Number, _testRooms[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task RoomNumbersDoNotMatchExceptionPutRoomReturnsBadRequestResponse() {
            _fakeService.Setup(s => s.UpdateRoom(It.IsAny<int>(), It.IsAny<Room>())).ThrowsAsync(new RoomNumbersDoNotMatchException());

            var response = await _testController.PutRoom(_testRooms[0].Number, _testRooms[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task RoomDoesNotExistExceptionPutRoomReturnsNotFoundResponse() {
            _fakeService.Setup(s => s.UpdateRoom(It.IsAny<int>(), It.IsAny<Room>())).ThrowsAsync(new RoomDoesNotExistException());

            var response = await _testController.PutRoom(_testRooms[0].Number, _testRooms[0]);

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExcpetionPutRoomThrowsError() {
            _fakeService.Setup(s => s.UpdateRoom(It.IsAny<int>(), It.IsAny<Room>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testController.Invoking(c => c.PutRoom(_testRooms[0].Number, _testRooms[0])).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

        [TestMethod]
        public async Task ValidPostRoomReturnsCorrectType() {
            var newRoom = ModelFakes.RoomFake.Generate();

            var response = await _testController.PostRoom(newRoom);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<Room>();
        }

        [TestMethod]
        public async Task RoomAlreadyExistsExceptionPostRoomReturnsConflictResponse() {
            _fakeService.Setup(s => s.AddRoom(It.IsAny<Room>())).ThrowsAsync(new RoomAlreadyExistsException());

            var newRoom = ModelFakes.RoomFake.Generate();
            newRoom.Number = _testRooms[0].Number;

            var response = await _testController.PostRoom(newRoom);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionPostRoomThrowsException() {
            _fakeService.Setup(s => s.AddRoom(It.IsAny<Room>())).ThrowsAsync(new DbUpdateException());

            var newRoom = ModelFakes.RoomFake.Generate();

            await _testController.Invoking(s => s.PostRoom(newRoom)).Should().ThrowAsync<DbUpdateException>();
        }

        [TestMethod]
        public async Task ValidDeleteRoomReturnsCorrectType() {
            var response = await _testController.DeleteRoom(_testRooms[0]);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Room>();
        }

        [TestMethod]
        public async Task NullDeleteRoomReturnsNotFoundResponse() {
            _fakeService.Setup(s => s.DeleteRoom(It.IsAny<Room>())).ReturnsAsync((Room)null);

            var response = await _testController.DeleteRoom(null);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionDeleteRoomThrowsError() {
            _fakeService.Setup(s => s.DeleteRoom(It.IsAny<Room>())).ThrowsAsync(new DbUpdateException());

            await _testController.Invoking(c => c.DeleteRoom(_testRooms[0])).Should().ThrowAsync<DbUpdateException>();
        }
    }
}
