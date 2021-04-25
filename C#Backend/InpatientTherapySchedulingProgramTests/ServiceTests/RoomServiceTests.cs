using FluentAssertions;
using InpatientTherapySchedulingProgram.Exceptions.RoomException;
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
    public class RoomServiceTests
    {
        private List<Room> _testRooms;
        private Room _nonActiveRooms;
        private CoreDbContext _testContext;
        private RoomService _testRoomService;

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

        }


        [TestMethod]
        public async Task GetAllRoomsReturnsCorrectType()
        {
            var allRooms = await _testRoomService.GetAllRooms();

            allRooms.Should().BeOfType<List<Room>>();
        }

        [TestMethod]
        public async Task GetAllRoomsReturnsCorrectNumberOfRooms()
        {
            var allRooms = await _testRoomService.GetAllRooms();
            List<Room> listOfRooms = (List<Room>)allRooms;

            listOfRooms.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task GetAllRoomsReturnsCorrectListOfRooms()
        {
            var allRooms = await _testRoomService.GetAllRooms();
            List<Room> listOfRooms = (List<Room>)allRooms;

            for (int i = 0; i < listOfRooms.Count; i++)
            {
                _testRooms.Contains(listOfRooms[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetRoomByLocationIdReturnsCorrectType()
        {
            var rooms = await _testRoomService.GetAllRoomsByLocationId(_testRooms[0].LocationId);

            rooms.Should().BeOfType<List<Room>>();
        }

        [TestMethod]
        public async Task GetRoomByLocationIdReturnsNullIfRoomDoesNotExist()
        {
            var room = await _testRoomService.GetAllRoomsByLocationId(-1);

            room.Should().BeEmpty();
        }

        [TestMethod]
        public async Task GetRoomByLocationIdReturnsNullIfRoomIsNotActive()
        {
            var room = await _testRoomService.GetAllRoomsByLocationId(_nonActiveRooms.LocationId);

            room.Should().BeEmpty();
        }

        [TestMethod]
        public async Task AddRoomReturnsCorrectType()
        {
            var newRoom = ModelFakes.RoomFake.Generate();

            var returnRoom = await _testRoomService.AddRoom(newRoom);

            returnRoom.Should().BeOfType<Room>();
        }

        [TestMethod]
        public async Task AddRoomIncreasesCountOfRooms()
        {
            var newRoom = ModelFakes.RoomFake.Generate();

            await _testRoomService.AddRoom(newRoom);

            var allRooms = await _testRoomService.GetAllRooms();
            List<Room> listOfRooms = (List<Room>)allRooms;

            listOfRooms.Count.Should().Be(11);
        }

        [TestMethod]
        public async Task AddRoomCorrectlyAddsRoomToDatabase()
        {
            var newRoom = ModelFakes.RoomFake.Generate();

            await _testRoomService.AddRoom(newRoom);

            var allRooms = await _testRoomService.GetAllRooms();
            List<Room> listOfRooms = (List<Room>)allRooms;

           listOfRooms.Contains(newRoom).Should().BeTrue();
        }

        [TestMethod]
        public async Task AddRoomThatExistsThrowsRoomAlreadtExistsException()
        {
            var newRoom = ModelFakes.RoomFake.Generate();
            newRoom = _testRooms[0];

            await _testRoomService.Invoking(s => s.AddRoom(newRoom)).Should().ThrowAsync<RoomAlreadyExistsException>();
        }


        [TestMethod]
        public async Task DeleteRoomReturnsCorrectType()
        {
            var room = await _testRoomService.DeleteRoom(_testRooms[0]);

            room.Should().BeOfType<Room>();
        }

        [TestMethod]
        public async Task DeleteRoomDecrementsCount()
        {
            await _testRoomService.DeleteRoom(_testRooms[0]);

            var allRooms = await _testRoomService.GetAllRooms();
            List<Room> listOfRooms = (List<Room>)allRooms;

            listOfRooms.Count.Should().Be(9);
        }

        [TestMethod]
        public async Task DeleteRoomCorrectlyRemovesRoomFromDatabase()
        {
            await _testRoomService.DeleteRoom(_testRooms[0]);

            var allRooms = await _testRoomService.GetAllRooms();
            List<Room> listOfRooms = (List<Room>)allRooms;

            listOfRooms.Contains(_testRooms[0]).Should().BeFalse();
        }

    }
}

