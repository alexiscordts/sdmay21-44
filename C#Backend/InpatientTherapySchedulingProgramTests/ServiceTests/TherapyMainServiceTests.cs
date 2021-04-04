using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services;
using InpatientTherapySchedulingProgramTests.Fakes;
using System.Threading.Tasks;
using InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions;
using System;
using System.Linq;

namespace InpatientTherapySchedulingProgramTests.ServiceTests
{
    [TestClass]
    public class TherapyMainServiceTests
    {
        private List<TherapyMain> _testTherapyMains;
        private CoreDbContext _testContext;
        private TherapyMainService _testTherapyMainService;
        private TherapyMain _nonActiveTherapyMain;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TherapyMainDatabase")
                .Options;
            _testTherapyMains = new List<TherapyMain>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            for (var i = 0; i < 10; i++)
            {
                var newTherapyMain = ModelFakes.TherapyMainFake.Generate();
                _testContext.Add(newTherapyMain);
                _testContext.SaveChanges();
                _testTherapyMains.Add(ObjectExtensions.Copy(_testContext.TherapyMain.FirstOrDefault(u => u.Type.Equals(newTherapyMain.Type))));
            }

            _nonActiveTherapyMain = ModelFakes.TherapyMainFake.Generate();
            _nonActiveTherapyMain.Active = false;
            _testContext.Add(_nonActiveTherapyMain);
            _testContext.SaveChanges();
            _testTherapyMains.Add(ObjectExtensions.Copy(_testContext.TherapyMain.FirstOrDefault(u => u.Type.Equals(_nonActiveTherapyMain.Type))));

            _testTherapyMainService = new TherapyMainService(_testContext);
        }

        [TestMethod]
        public async Task GetAllTherapyMainsReturnsCorrectType()
        {
            var allTherapyMains = await _testTherapyMainService.GetAllTherapyMains();

            allTherapyMains.Should().BeOfType<List<TherapyMain>>();
        }

        [TestMethod]
        public async Task GetAllTherapyMainsReturnsCorrectNumberOfTherapyMains()
        {
            var allTherapyMains = await _testTherapyMainService.GetAllTherapyMains();
            List<TherapyMain> listOfTherapyMains = (List<TherapyMain>)allTherapyMains;

            listOfTherapyMains.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task GetAllTherapyMainsReturnsCorrectListOfTherapyMains()
        {
            var allTherapyMains = await _testTherapyMainService.GetAllTherapyMains();
            List<TherapyMain> listOfTherapyMains = (List<TherapyMain>)allTherapyMains;

            for (var i = 0; i < listOfTherapyMains.Count; i++)
            {
                _testTherapyMains.Contains(listOfTherapyMains[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetTherapyMainByTypeReturnsCorrectType()
        {
            var returnTherapyMain = await _testTherapyMainService.GetTherapyMainByType(_testTherapyMains[0].Type);

            returnTherapyMain.Should().BeOfType<TherapyMain>();
        }

        [TestMethod]
        public async Task GetTherapyMainByTypeReturnsCorrectTherapyMain()
        {
            var returnTherapyMain = await _testTherapyMainService.GetTherapyMainByType(_testTherapyMains[0].Type);

            returnTherapyMain.Should().Be(_testTherapyMains[0]);
        }

        [TestMethod]
        public async Task GetTherapyMainByTypeReturnsNullIfTherapyMainDoesNotExist()
        {
            var fakeTherapyMain = ModelFakes.TherapyMainFake.Generate();

            var returnTherapyMain = await _testTherapyMainService.GetTherapyMainByType(fakeTherapyMain.Type);

            returnTherapyMain.Should().BeNull();
        }

        [TestMethod]
        public async Task GetTherapyMainByAbbreviationReturnsCorrectType()
        {
            var returnTherapyMain = await _testTherapyMainService.GetTherapyMainByAbbreviation(_testTherapyMains[0].Abbreviation);

            returnTherapyMain.Should().BeOfType<TherapyMain>();
        }

        [TestMethod]
        public async Task GetTherapyMainByAbbreviationReturnsCorrectTherapyMain()
        {
            var returnTherapyMain = await _testTherapyMainService.GetTherapyMainByAbbreviation(_testTherapyMains[0].Abbreviation);

            returnTherapyMain.Should().Be(_testTherapyMains[0]);
        }

        [TestMethod]
        public async Task GetTherapyMainByAbbreviationReturnsNullIfTherapyMainDoesNotExist()
        {
            var fakeTherapyMain = ModelFakes.TherapyMainFake.Generate();

            var returnTherapyMain = await _testTherapyMainService.GetTherapyMainByAbbreviation(fakeTherapyMain.Abbreviation);

            returnTherapyMain.Should().BeNull();
        }

        [TestMethod]
        public async Task AddTherapyMainReturnsCorrectType()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();

            var returnTherapyMain = await _testTherapyMainService.AddTherapyMain(newTherapyMain);

            returnTherapyMain.Should().BeOfType<TherapyMain>();
        }

        [TestMethod]
        public async Task AddTherapyMainIncreasesCountOfTherapyMain()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();

            await _testTherapyMainService.AddTherapyMain(newTherapyMain);

            var allTherapyMains = await _testTherapyMainService.GetAllTherapyMains();
            List<TherapyMain> listOfTherapyMains = (List<TherapyMain>)allTherapyMains;

            listOfTherapyMains.Count.Should().Be(11);
        }

        [TestMethod]
        public async Task AddTherapyMainCorrectlyAddsTherapyMainToDatabase()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();

            await _testTherapyMainService.AddTherapyMain(newTherapyMain);

            var returnTherapyMain = await _testTherapyMainService.GetTherapyMainByType(newTherapyMain.Type);

            returnTherapyMain.Should().Be(newTherapyMain);
        }

        [TestMethod]
        public async Task AddTherapyWithExistingTypeThrowsTherapyMainTypeAlreadyExistsException()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();
            newTherapyMain.Type = _testTherapyMains[0].Type;

            await _testTherapyMainService.Invoking(s => s.AddTherapyMain(newTherapyMain)).Should().ThrowAsync<TherapyMainTypeAlreadyExistsException>();
        }

        [TestMethod]
        public async Task AddTherapyWithExistingAbbreviationThrowsTherapyMainAbbreviationAlreadyExistsException()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();
            newTherapyMain.Abbreviation = _testTherapyMains[0].Abbreviation;

            await _testTherapyMainService.Invoking(s => s.AddTherapyMain(newTherapyMain)).Should().ThrowAsync<TherapyMainTypeAbbreviationAlreadyExistsException>();
        }

        [TestMethod]
        public async Task DeleteTherapyMainReturnsCorrectType()
        {
            var returnTherapyMain = await _testTherapyMainService.DeleteTherapyMain(_testTherapyMains[0].Type);

            returnTherapyMain.Should().BeOfType<TherapyMain>();
        }

        [TestMethod]
        public async Task DeleteTherapyMainReturnsCorrectTherapyMain()
        {
            var returnTherapyMain = await _testTherapyMainService.DeleteTherapyMain(_testTherapyMains[0].Type);

            returnTherapyMain.Should().Be(_testTherapyMains[0]);
        }

        [TestMethod]
        public async Task DeleteTherapyMainDecrementsCount()
        {
            await _testTherapyMainService.DeleteTherapyMain(_testTherapyMains[0].Type);

            var allTherapyMains = await _testTherapyMainService.GetAllTherapyMains();
            List<TherapyMain> listOfTherapyMains = (List<TherapyMain>)allTherapyMains;

            listOfTherapyMains.Count.Should().Be(9);
        }

        [TestMethod]
        public async Task DeleteTherapyMainCorrectlyRemovesTherapyMainFromDatabase()
        {
            await _testTherapyMainService.DeleteTherapyMain(_testTherapyMains[0].Type);

            var allTherapyMains = await _testTherapyMainService.GetAllTherapyMains();
            List<TherapyMain> listOfTherapyMains = (List<TherapyMain>)allTherapyMains;

            listOfTherapyMains.Contains(_testTherapyMains[0]).Should().BeFalse();
        }

        [TestMethod]
        public async Task DeleteTherapyMainThatDoesNotExistReturnsNull()
        {
            var fakeTherapyMain = ModelFakes.TherapyMainFake.Generate();

            var returnTherapyMain = await _testTherapyMainService.DeleteTherapyMain(fakeTherapyMain.Type);

            returnTherapyMain.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateTherapyMainReturnsCorrectType()
        {
            var returnTherapyMain = await _testTherapyMainService.UpdateTherapyMain(_testTherapyMains[0].Type, _testTherapyMains[0]);

            returnTherapyMain.Should().BeOfType<TherapyMain>();
        }

        [TestMethod]
        public async Task UpdateTherapyMainReturnsCorrectTherapyMain()
        {
            var returnTherapyMain = await _testTherapyMainService.UpdateTherapyMain(_testTherapyMains[0].Type, _testTherapyMains[0]);

            returnTherapyMain.Should().Be(_testTherapyMains[0]);
        }

        [TestMethod]
        public async Task UpdateTherapyMainWithAlteredDataCorrectlyUpdatesInDatabase()
        {
            var newAbbreviation = ModelFakes.TherapyMainFake.Generate().Abbreviation;
            _testTherapyMains[0].Abbreviation = newAbbreviation;

            await _testTherapyMainService.UpdateTherapyMain(_testTherapyMains[0].Type, _testTherapyMains[0]);

            var returnTherapyMain = await _testTherapyMainService.GetTherapyMainByType(_testTherapyMains[0].Type);

            returnTherapyMain.Should().Be(_testTherapyMains[0]);
        }

        [TestMethod]
        public async Task UpdateTherapyMainWithNonMatchingTherapyTypesThrowsTherapyMainTypesDoNotMatchException()
        {
            var newAbbreviation = ModelFakes.TherapyMainFake.Generate().Abbreviation;
            _testTherapyMains[0].Abbreviation = newAbbreviation;

            await _testTherapyMainService.Invoking(s => s.UpdateTherapyMain("-1", _testTherapyMains[0])).Should().ThrowAsync<TherapyMainTypesDoNotMatchException>();
        }

        [TestMethod]
        public async Task UpdateTherapyMainWithNonExistingTherapyMainThrowsTherapyMainDoesNotExistsException()
        {
            var fakeTherapyMain = ModelFakes.TherapyMainFake.Generate();

            await _testTherapyMainService.Invoking(s => s.UpdateTherapyMain(fakeTherapyMain.Type, fakeTherapyMain)).Should().ThrowAsync<TherapyMainDoesNotExistException>();
        }
    }
}
