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
    public class RoomControllerTests
    {

        private static List<Room> _testRooms;
        private Mock<IRoomService> _fakeService;
        private PatientController _testController;

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
            //_fakeService.Setup(s => s.GetAllRoomsByLocationId(It.IsAny<int>())).ReturnAsync(_testRooms);
        }
    }
}
