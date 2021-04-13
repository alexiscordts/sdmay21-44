using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services;
using InpatientTherapySchedulingProgramTests.Fakes;
using System.Threading.Tasks;
using InpatientTherapySchedulingProgram.Exceptions.UserExceptions;
using InpatientTherapySchedulingProgram.Exceptions.HoursWorkedExceptions;

namespace InpatientTherapySchedulingProgramTests.ServiceTests
{
    [TestClass]
    class HoursWorkedServiceTests
    {
        private List<HoursWorked> _testHoursWorked;
        private CoreDbContext _testContext;
        private HoursWorkedService _testHoursWorkedService;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "UserDatabase")
                .Options;
            _testHoursWorked = new List<HoursWorked>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            for (var i = 0; i < 10; i++)
            {
                var newHoursWorked = ModelFakes.HoursWorkedFake.Generate();
                _testHoursWorked.Add(ObjectExtensions.Copy(newHoursWorked));
                _testContext.Add(newHoursWorked);
                _testContext.SaveChanges();
            }

            _testHoursWorkedService = new HoursWorkedService(_testContext);
        }

        //get hours id
        //update
        //delete
        //add

        //get user id
        [TestMethod]
        public async Task GetHoursWorkedByUserIdReturnsCorrectType()
        {
            var returnHoursWorked = await _testHoursWorkedService.GetHoursWorkedByUserId(_testHoursWorked[0].UserId);

            returnHoursWorked.Should().BeOfType<HoursWorked>();
        }
        //get user id
        [TestMethod]
        public async Task GetHoursWorkedByUserIdReturnsCorrectUser()
        {
            var returnHoursWorked = await _testHoursWorkedService.GetHoursWorkedByUserId(_testHoursWorked[0].UserId);

            returnHoursWorked.Should().Be(_testHoursWorked[0]);
        }
        //get user id
        [TestMethod]
        public async Task GetHoursWorkedByUserIdReturnsNullIfUserDoesNotExist()
        {
            var returnHoursWorked = await _testHoursWorkedService.GetHoursWorkedByUserId(-1);

            returnHoursWorked.Should().BeNull();
        }

        [TestMethod]
        public async Task GetHoursWorkedByIdReturnsCorrectType()
        {
            var returnHoursWorked = await _testHoursWorkedService.GetHoursWorkedByUserId(_testHoursWorked[0].HoursWorkedId);

            returnHoursWorked.Should().BeOfType<HoursWorked>();
        }

        [TestMethod]
        public async Task GetHoursWorkedByIdReturnsCorrectUser()
        {
            var returnHoursWorked = await _testHoursWorkedService.GetHoursWorkedById(_testHoursWorked[0].HoursWorkedId);

            returnHoursWorked.Should().Be(_testHoursWorked[0]);
        }

        [TestMethod]
        public async Task GetHoursWorkedByIdReturnsNullIfHoursWorkedDoesNotExist()
        {
            var returnHoursWorked = await _testHoursWorkedService.GetHoursWorkedById(-1);

            returnHoursWorked.Should().BeNull();
        }

        public async Task AddHoursWorkedReturnsCorrectType()
        {
            var newHoursWorked = ModelFakes.HoursWorkedFake.Generate();

            var returnHoursWorked = await _testHoursWorkedService.AddHoursWorked(newHoursWorked);

            returnHoursWorked.Should().BeOfType<HoursWorked>();
        }


        [TestMethod]
        public async Task AddHoursWorkedCorrectlyAddsHoursWorkedToDatabase()
        {
            var newHoursWorked = ModelFakes.HoursWorkedFake.Generate();

            await _testHoursWorkedService.AddHoursWorked(newHoursWorked);

            var returnHoursWorked = await _testHoursWorkedService.GetHoursWorkedByUserId(newHoursWorked.UserId);

            returnHoursWorked.Should().Be(newHoursWorked);
        }

        [TestMethod]
        public async Task AddHoursWorkedWithExistingIdThrowsError()
        {
            int existingId = _testHoursWorked[0].HoursWorkedId;
            var newHoursWorked = ModelFakes.HoursWorkedFake.Generate();
            newHoursWorked.HoursWorkedId = existingId;

            await _testHoursWorkedService.Invoking(s => s.AddHoursWorked(newHoursWorked)).Should().ThrowAsync<HoursWorkedIdAlreadyExistsException>();
        }

        [TestMethod]
        public async Task DeleteHoursWorkedReturnsCorrectUser()
        {
            var returnHoursWorked = await _testHoursWorkedService.DeleteHoursWorked(_testHoursWorked[0].HoursWorkedId);

            bool isEqual = returnHoursWorked.Equals(_testHoursWorked[0]);

            returnHoursWorked.Should().Be(_testHoursWorked[0]);
        }

        [TestMethod]
        public async Task DeleteHoursWorkedThatDoesExistReturnsNull()
        {
            var returnHoursWorked = await _testHoursWorkedService.DeleteHoursWorked(-1);

            returnHoursWorked.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateHoursWorkedReturnsCorrectType()
        {
            var returnHoursWorked = await _testHoursWorkedService.UpdateHoursWorked(_testHoursWorked[0].HoursWorkedId, _testHoursWorked[0]);

            returnHoursWorked.Should().BeOfType<HoursWorked>();
        }

        [TestMethod]
        public async Task UpdateHoursWorkedReturnsCorrectUser()
        {
            var returnHoursWorked = await _testHoursWorkedService.UpdateHoursWorked(_testHoursWorked[0].HoursWorkedId, _testHoursWorked[0]);

            returnHoursWorked.Should().Be(_testHoursWorked[0]);
        }

        [TestMethod]
        public async Task UpdateHoursWorkedWithNonMatchingIdsThrowsError()
        {
            await _testHoursWorkedService.Invoking(s => s.UpdateHoursWorked(_testHoursWorked[1].HoursWorkedId, _testHoursWorked[0])).Should().ThrowAsync<HoursWorkedIdsDoNotMatchException>();
        }

        [TestMethod]
        public async Task UpdateHoursWorkedWithNonExistingUserThrowsError()
        {
            var fakeHoursWorked = ModelFakes.HoursWorkedFake.Generate();

            await _testHoursWorkedService.Invoking(s => s.UpdateHoursWorked(fakeHoursWorked.HoursWorkedId, fakeHoursWorked)).Should().ThrowAsync<HoursWorkedDoesNotExistException>();
        }

    }
}
