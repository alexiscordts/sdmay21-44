using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgramTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using InpatientTherapySchedulingProgram.Services;
using System;
using System.Linq;


namespace InpatientTherapySchedulingProgramTests.IntegrationTests
{
    [TestClass]
    public class TherapyMainServiceControllerTests
    {
        private List<TherapyMain> _testTherapyMains;
        private CoreDbContext _testContext;
        private TherapyMainService _testTherapyMainService;
        private TherapyMainController _testTherapyMainController;
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
                _testTherapyMains.Add(ObjectExtensions.Copy(_testContext.TherapyMain.FirstOrDefault(t => t.Type.Equals(newTherapyMain.Type))));
            }

            _nonActiveTherapyMain = ModelFakes.TherapyMainFake.Generate();
            _nonActiveTherapyMain.Active = false;
            _testContext.Add(_nonActiveTherapyMain);
            _testContext.SaveChanges();
            _testTherapyMains.Add(ObjectExtensions.Copy(_testContext.TherapyMain.FirstOrDefault(t => t.Type.Equals(_nonActiveTherapyMain.Type))));

            _testTherapyMainService = new TherapyMainService(_testContext);
            _testTherapyMainController = new TherapyMainController(_testTherapyMainService);
        }

        [TestMethod]
        public async Task ValidGetTherapyMainReturnsOkResponse()
        {
            var response = await _testTherapyMainController.GetTherapyMain();
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyMainReturnsCorrectType() 
        {
            var response = await _testTherapyMainController.GetTherapyMain();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<TherapyMain>>();
        }

        [TestMethod]
        public async Task ValidGetTherapyMainReturnsCorrectNumberOfTherapyMains()
        {
            var response = await _testTherapyMainController.GetTherapyMain();
            var responseResult = response.Result as OkObjectResult;
            List<TherapyMain> listOfTherapyMains = (List<TherapyMain>)responseResult.Value;

            listOfTherapyMains.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task ValidGetTherapyMainReturnsCorrectListOfTherapyMains()
        {
            var response = await _testTherapyMainController.GetTherapyMain();
            var responseResult = response.Result as OkObjectResult;
            List<TherapyMain> listOfTherapyMains = (List<TherapyMain>)responseResult.Value;

            for (var i = 0; i < listOfTherapyMains.Count; i++)
            {
                _testTherapyMains.Contains(listOfTherapyMains[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetTherapyMainByTypeReturnsOkResponse()
        {
            var response = await _testTherapyMainController.GetTherapyMainByType(_testTherapyMains[0].Type);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyMainByTypeReturnsCorrectType()
        {
            var response = await _testTherapyMainController.GetTherapyMainByType(_testTherapyMains[0].Type);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapyMain>();
        }

        [TestMethod]
        public async Task ValidGetTherapyMainByTypeReturnsCorrectTherapyMain()
        {
            var response = await _testTherapyMainController.GetTherapyMainByType(_testTherapyMains[0].Type);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testTherapyMains[0]);
        }

        [TestMethod]
        public async Task NonExistingGetTherapyMainByTypeReturnsNotFoundResponse()
        {
            var fakeTherapyMain = ModelFakes.TherapyMainFake.Generate();

            var response = await _testTherapyMainController.GetTherapyMainByType(fakeTherapyMain.Type);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyMainByAbbreviationReturnsOkResponse()
        {
            var response = await _testTherapyMainController.GetTherapyMainByAbbreviation(_testTherapyMains[0].Abbreviation);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyMainByAbbreviationReturnsCorrectType()
        {
            var response = await _testTherapyMainController.GetTherapyMainByAbbreviation(_testTherapyMains[0].Abbreviation);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapyMain>();
        }

        [TestMethod]
        public async Task ValidGetTherapyMainByAbbreviationReturnsCorrectTherapyMain()
        {
            var response = await _testTherapyMainController.GetTherapyMainByAbbreviation(_testTherapyMains[0].Abbreviation);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testTherapyMains[0]);
        }

        [TestMethod]
        public async Task NonExistingGetTherapyMainByAbbreviationReturnsNotFoundResponse()
        {
            var fakeTherapyMain = ModelFakes.TherapyMainFake.Generate();

            var response = await _testTherapyMainController.GetTherapyMainByAbbreviation(fakeTherapyMain.Abbreviation);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutTherapyMainReturnsNoContentResponse()
        {
            var newAbbreviation = ModelFakes.TherapyMainFake.Generate().Abbreviation;
            _testTherapyMains[0].Abbreviation = newAbbreviation;

            var response = await _testTherapyMainController.PutTherapyMain(_testTherapyMains[0].Type, _testTherapyMains[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task ValidPutTherapyMainWithAlteredDataCorrectlyUpdatesInDatabase()
        {
            var newAbbreviation = ModelFakes.TherapyMainFake.Generate().Abbreviation;
            _testTherapyMains[0].Abbreviation = newAbbreviation;

            await _testTherapyMainController.PutTherapyMain(_testTherapyMains[0].Type, _testTherapyMains[0]);

            var response = await _testTherapyMainController.GetTherapyMainByType(_testTherapyMains[0].Type);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testTherapyMains[0]);
        }

        [TestMethod]
        public async Task NonMatchingTherapyMainTypesShouldReturnsBadRequestResponse()
        {
            var newAbbreviation = ModelFakes.TherapyMainFake.Generate().Abbreviation;
            _testTherapyMains[0].Abbreviation = newAbbreviation;

            var response = await _testTherapyMainController.PutTherapyMain("-1", _testTherapyMains[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingTherapyMainTypesShouldReturnsNotFoudnResponse()
        {
            var fakeTherapyMain = ModelFakes.TherapyMainFake.Generate();

            var response = await _testTherapyMainController.PutTherapyMain(fakeTherapyMain.Type, fakeTherapyMain);

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPostTherapyMainReturnsCreatedAtActionResponse()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();

            var response = await _testTherapyMainController.PostTherapyMain(newTherapyMain);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostTherapyMainReturnsCorrectType()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();

            var response = await _testTherapyMainController.PostTherapyMain(newTherapyMain);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<TherapyMain>();
        }

        [TestMethod]
        public async Task ValidPostTherapyMainCorrectlyAddsTherapyMain()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();

            await _testTherapyMainController.PostTherapyMain(newTherapyMain);

            var response = await _testTherapyMainController.GetTherapyMainByType(newTherapyMain.Type);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(newTherapyMain);
        }

        [TestMethod]
        public async Task ExistingTypePostTherapyMainReturnsConflictResponse()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();
            newTherapyMain.Type = _testTherapyMains[0].Type;

            var response = await _testTherapyMainController.PostTherapyMain(newTherapyMain);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task ExistingTypePostTherapyMainDoesNotAddTherapyMain()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();
            newTherapyMain.Type = _testTherapyMains[0].Type;

            await _testTherapyMainController.PostTherapyMain(newTherapyMain);

            var response = _testTherapyMainController.GetTherapyMainByType(newTherapyMain.Type);
            var responseResult = response.Result;

            responseResult.Should().NotBe(newTherapyMain);
        }

        [TestMethod]
        public async Task ExistingAbbreviationPostTherapyMainReturnsConflictResponse()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();
            newTherapyMain.Abbreviation = _testTherapyMains[0].Abbreviation;

            var response = await _testTherapyMainController.PostTherapyMain(newTherapyMain);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task ExistingAbbreviationPostTherapyMainDoesNotAddTherapyMainToDatabase()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();
            newTherapyMain.Abbreviation = _testTherapyMains[0].Abbreviation;

            await _testTherapyMainController.PostTherapyMain(newTherapyMain);

            var response = _testTherapyMainController.GetTherapyMainByType(newTherapyMain.Type);
            var responseResult = response.Result;

            response.Result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapyMainReturnsOkResponse()
        {
            var response = await _testTherapyMainController.DeleteTherapyMain(_testTherapyMains[0].Type);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapyMainReturnsCorrectType()
        {
            var response = await _testTherapyMainController.DeleteTherapyMain(_testTherapyMains[0].Type);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapyMain>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapyMainReturnsCorrectTherapyMain()
        {
            var response = await _testTherapyMainController.DeleteTherapyMain(_testTherapyMains[0].Type);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testTherapyMains[0]);
        }

        [TestMethod]
        public async Task ValidDeleteTherapyMainReturnsCorrectlyRemovesTherapyMainFromDatabase()
        {
            await _testTherapyMainController.DeleteTherapyMain(_testTherapyMains[0].Type);

            var response = await _testTherapyMainController.GetTherapyMainByType(_testTherapyMains[0].Type);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task NonExistingTherapyMainDeleteTherapyMainReturnsNotFoundResponse()
        {
            var fakeTherapyMain = ModelFakes.TherapyMainFake.Generate();

            var response = await _testTherapyMainController.DeleteTherapyMain(fakeTherapyMain.Type);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }
    }
}
