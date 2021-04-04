using FluentAssertions;
using InpatientTherapySchedulingProgram.Exceptions.TherapyExceptions;
using InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions;
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
    public class TherapyServiceTests
    {
        private List<TherapyMain> _testTherapyMains;
        private List<Therapy> _testTherapies;
        private Therapy _nonActiveTherapy;
        private CoreDbContext _testContext;
        private TherapyService _testTherapyService;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TherapyDatabase")
                .Options;
            _testTherapyMains = new List<TherapyMain>();
            _testTherapies = new List<Therapy>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            for(var i = 0; i < 10; i++)
            {
                var newTherapyMain = ModelFakes.TherapyMainFake.Generate();
                _testContext.Add(newTherapyMain);
                _testContext.SaveChanges();
                _testTherapyMains.Add(ObjectExtensions.Copy(newTherapyMain));


                var newTherapy = ModelFakes.TherapyFake.Generate();
                newTherapy.Type = newTherapyMain.Type;
                _testContext.Add(newTherapy);
                _testContext.SaveChanges();
                _testTherapies.Add(ObjectExtensions.Copy(newTherapy));
            }

            _nonActiveTherapy = ModelFakes.TherapyFake.Generate();
            _nonActiveTherapy.Type = _testTherapyMains[0].Type;
            _nonActiveTherapy.Active = false;
            _testContext.Add(_nonActiveTherapy);
            _testContext.SaveChanges();

            _testTherapyService = new TherapyService(_testContext);
        }

        [TestMethod]
        public async Task GetAllTherapiesReturnsCorrectType()
        {
            var allTherapies = await _testTherapyService.GetAllTherapies();

            allTherapies.Should().BeOfType<List<Therapy>>();
        }

        [TestMethod]
        public async Task GetAllTherapiesReturnsCorrectNumberOfTherapists()
        {
            var allTherapies = await _testTherapyService.GetAllTherapies();
            List<Therapy> listOfTherapies = (List<Therapy>)allTherapies;

            listOfTherapies.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task GetAllTherapiesReturnsCorrectListOfTherapists()
        {
            var allTherapies = await _testTherapyService.GetAllTherapies();
            List<Therapy> listOfTherapies = (List<Therapy>)allTherapies;

            for(var i = 0; i < listOfTherapies.Count; i++)
            {
                _testTherapies.Contains(listOfTherapies[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetAllAdlsReturnsCorrectType()
        {
            var allAdls = await _testTherapyService.GetAllAdls();

            allAdls.Should().BeOfType<List<string>>();
        }

        [TestMethod]
        public async Task GetAllAdlsReturnCorrectListOfAdls()
        {
            List<string> _testListOfAdls = new List<string>();

            for(var i = 0; i < _testTherapies.Count; i++)
            {
                _testListOfAdls.Add(_testTherapies[i].Adl);
            }

            var allAdls = await _testTherapyService.GetAllAdls();
            List<string> listOfAdls = (List<string>)allAdls;

            for(var i = 0; i < _testListOfAdls.Count; i++)
            {
                _testListOfAdls.Contains(listOfAdls[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetAllTypesReturnsCorrectType()
        {
            var allTypes = await _testTherapyService.GetAllTypes();

            allTypes.Should().BeOfType<List<string>>();
        }

        [TestMethod]
        public async Task GetAllTypesReturnsCorrectListOfTypes()
        {
            List<string> _testListOfTypes = new List<string>();

            for(var i = 0; i < _testTherapies.Count; i++)
            {
                _testListOfTypes.Add(_testTherapies[i].Type);
            }

            var allTypes = await _testTherapyService.GetAllTypes();
            List<string> listOfTypes = (List<string>)allTypes;

            for(var i = 0; i < listOfTypes.Count; i++)
            {
                _testListOfTypes.Contains(listOfTypes[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetTherapyByAdlReturnsCorrectType()
        {
            var therapy = await _testTherapyService.GetTherapyByAdl(_testTherapies[0].Adl);

            therapy.Should().BeOfType<Therapy>();
        }

        [TestMethod]
        public async Task GetTherapyByAdlReturnsCorrectTherapy()
        {
            var therapy = await _testTherapyService.GetTherapyByAdl(_testTherapies[0].Adl);

            therapy.Should().Be(_testTherapies[0]);
        }

        [TestMethod]
        public async Task GetTherapyByAdlReturnsNullIfTherapyDoesNotExist()
        {
            var therapy = await _testTherapyService.GetTherapyByAdl("-1");

            therapy.Should().BeNull();
        }

        [TestMethod]
        public async Task GetTherapyByAdlReturnsNullIfTherapyIsNotActive()
        {
            var therapy = await _testTherapyService.GetTherapyByAdl(_nonActiveTherapy.Adl);

            therapy.Should().BeNull();
        }

        [TestMethod]
        public async Task GetTherapyByAbbreviationReturnsCorrectType()
        {
            var therapy = await _testTherapyService.GetTherapyByAbbreviation(_testTherapies[0].Abbreviation);

            therapy.Should().BeOfType<Therapy>();
        }

        [TestMethod]
        public async Task GetTherapyByAbbreviationReturnsCorrectTherapy()
        {
            var therapy = await _testTherapyService.GetTherapyByAbbreviation(_testTherapies[0].Abbreviation);

            therapy.Should().Be(_testTherapies[0]);
        }

        [TestMethod]
        public async Task GetTherapyByAbbreviationReturnsNullIfTherapyDoesNotExist()
        {
            var therapy = await _testTherapyService.GetTherapyByAbbreviation("-1");

            therapy.Should().BeNull();
        }

        [TestMethod]
        public async Task GetTherapyByAbbreviationReturnsNullIfTherapyIsNotActive()
        {
            var therapy = await _testTherapyService.GetTherapyByAbbreviation(_nonActiveTherapy.Abbreviation);

            therapy.Should().BeNull();
        }

        [TestMethod]
        public async Task AddTherapyReturnsCorrectType()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Type = _testTherapyMains[0].Type;

            var returnTherapy = await _testTherapyService.AddTherapy(newTherapy);

            returnTherapy.Should().BeOfType<Therapy>();
        }

        [TestMethod]
        public async Task AddTherapyIncreasesCountOfTherapies()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Type = _testTherapyMains[0].Type;

            await _testTherapyService.AddTherapy(newTherapy);

            var allTherapies = await _testTherapyService.GetAllTherapies();
            List<Therapy> listOfTherapies = (List<Therapy>)allTherapies;

            listOfTherapies.Count.Should().Be(11);
        }

        [TestMethod]
        public async Task AddTherapyCorrectlyAddsTherapyToDatabase()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Type = _testTherapyMains[0].Type;

            await _testTherapyService.AddTherapy(newTherapy);

            var allTherapies = await _testTherapyService.GetAllTherapies();
            List<Therapy> listOfTherapies = (List<Therapy>)allTherapies;

            listOfTherapies.Contains(newTherapy).Should().BeTrue();
        }

        [TestMethod]
        public async Task AddTherapyWithExistingAdlThrowsTherapyAdlAlreadyExistException()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Adl = _testTherapies[0].Adl;

            await _testTherapyService.Invoking(s => s.AddTherapy(newTherapy)).Should().ThrowAsync<TherapyAdlAlreadyExistException>();
        }

        [TestMethod]
        public async Task AddTherapyWithExistingAbbreviationThrowsTherapyAbbreviationAlreadyExistException()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Abbreviation = _testTherapies[0].Abbreviation;

            await _testTherapyService.Invoking(s => s.AddTherapy(newTherapy)).Should().ThrowAsync<TherapyAbbreviationAlreadyExistException>();
        }

        [TestMethod]
        public async Task AddTherapyWithNonExistingMainTherapyThrowsTherapyMainDoesNotExistException()
        {
            var fakeTherapyMainType = ModelFakes.TherapyMainFake.Generate().Type;
            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Type = fakeTherapyMainType;

            await _testTherapyService.Invoking(s => s.AddTherapy(newTherapy)).Should().ThrowAsync<TherapyMainDoesNotExistException>();
        }

        [TestMethod]
        public async Task DeleteTherapyReturnsCorrectType()
        {
            var therapy = await _testTherapyService.DeleteTherapy(_testTherapies[0].Adl);

            therapy.Should().BeOfType<Therapy>();
        }

        [TestMethod]
        public async Task DeleteTherapyDecrementsCount()
        {
            await _testTherapyService.DeleteTherapy(_testTherapies[0].Adl);

            var allTherapies = await _testTherapyService.GetAllTherapies();
            List<Therapy> listOfTherapies = (List<Therapy>)allTherapies;

            listOfTherapies.Count.Should().Be(9);
        }

        [TestMethod]
        public async Task DeleteTherapyCorrectlyRemovesTherapyFromDatabase()
        {
            await _testTherapyService.DeleteTherapy(_testTherapies[0].Adl);

            var allTherapies = await _testTherapyService.GetAllTherapies();
            List<Therapy> listOfTherapies = (List<Therapy>)allTherapies;

            listOfTherapies.Contains(_testTherapies[0]).Should().BeFalse();
        }

        [TestMethod]
        public async Task DeleteTherapyThatDoesExistReturnsNull()
        {
            var therapy = await _testTherapyService.DeleteTherapy("-1");

            therapy.Should().BeNull();
        }

        [TestMethod]
        public async Task DeleteTherapyThatIsNotActiveReturnsNull()
        {
            var therapy = await _testTherapyService.DeleteTherapy(_nonActiveTherapy.Type);

            therapy.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateTherapyReturnsCorrectType()
        {
            var therapy = await _testTherapyService.UpdateTherapy(_testTherapies[0].Adl, _testTherapies[0]);

            therapy.Should().BeOfType<Therapy>();
        }

        [TestMethod]
        public async Task UpdateTherapyReturnsCorrectTherapy()
        {
            var therapy = await _testTherapyService.UpdateTherapy(_testTherapies[0].Adl, _testTherapies[0]);

            therapy.Should().Be(_testTherapies[0]);
        }

        [TestMethod]
        public async Task UpdateTherapyWithAlteredDataCorrectlyUpdatesTherapyInDatabase()
        {
            var newAbbreviation = ModelFakes.TherapyFake.Generate().Abbreviation;
            _testTherapies[0].Abbreviation = newAbbreviation;

            await _testTherapyService.UpdateTherapy(_testTherapies[0].Adl, _testTherapies[0]);

            var therapy = await _testTherapyService.GetTherapyByAbbreviation(newAbbreviation);

            therapy.Should().Be(_testTherapies[0]);
        }

        [TestMethod]
        public async Task UpdateTherapyWithNonMatchingAdlsThrowsTherapyAdlsDoNotMatchException()
        {
            await _testTherapyService.Invoking(s => s.UpdateTherapy("-1", _testTherapies[0])).Should().ThrowAsync<TherapyAdlsDoNotMatchException>();
        }

        [TestMethod]
        public async Task UpdateTherapyWithNonExistingTherapyThrowsTherapyDoesNotExistException()
        {
            var fakeTherapy = ModelFakes.TherapyFake.Generate();

            await _testTherapyService.Invoking(s => s.UpdateTherapy(fakeTherapy.Adl, fakeTherapy)).Should().ThrowAsync<TherapyDoesNotExistException>();
        }

        [TestMethod]
        public async Task UpdateTherapyWithNonExistingTherapyMainThrowsTherapyMainDoesNotExistException()
        {
            var fakeTherapyMainType = ModelFakes.TherapyMainFake.Generate().Type;
            _testTherapies[0].Type = fakeTherapyMainType;

            await _testTherapyService.Invoking(s => s.UpdateTherapy(_testTherapies[0].Adl, _testTherapies[0])).Should().ThrowAsync<TherapyMainDoesNotExistException>();
        }
    }
}
