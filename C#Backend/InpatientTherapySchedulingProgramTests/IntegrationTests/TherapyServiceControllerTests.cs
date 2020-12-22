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
    public class TherapyServiceControllerTests
    {
        private List<Therapy> _testTherapies;
        private static List<string> _testAdls;
        private static List<string> _testTypes;
        private CoreDbContext _testContext;
        private TherapyService _testService;
        private TherapyController _testController;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TherapyDatabase")
                .Options;
            _testTherapies = new List<Therapy>();
            _testAdls = new List<string>();
            _testTypes = new List<string>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            for(var i = 0; i < 10; i++)
            {
                var newTherapy = ModelFakes.TherapyFake.Generate();
                _testTherapies.Add(ObjectExtensions.Copy(newTherapy));
                _testAdls.Add(newTherapy.Adl);
                _testTypes.Add(newTherapy.Type);
                _testContext.Add(newTherapy);
                _testContext.SaveChanges();
            }

            _testService = new TherapyService(_testContext);
            _testController = new TherapyController(_testService);
        }

        [TestMethod]
        public async Task ValidGetAllTherapiesReturnsOkResponse()
        {
            var response = await _testController.GetTherapy();
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllTherapiesReturnsCorrectType()
        {
            var response = await _testController.GetTherapy();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<Therapy>>();
        }

        [TestMethod]
        public async Task ValidGetAllTherapiesReturnsCorrectCountOfTherapies()
        {
            var response = await _testController.GetTherapy();
            var responseResult = response.Result as OkObjectResult;
            List<Therapy> listOfTherapies = (List<Therapy>)responseResult.Value;

            listOfTherapies.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task ValidGetAllTherapiesReturnsCorrectTherapies()
        {
            var response = await _testController.GetTherapy();
            var responseResult = response.Result as OkObjectResult;
            List<Therapy> listOfTherapies = (List<Therapy>)responseResult.Value;

            for(var i = 0; i < listOfTherapies.Count; i++)
            {
                _testTherapies.Contains(listOfTherapies[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetTherapyByAdlReturnsOkResponse()
        {
            var response = await _testController.GetTherapy(_testTherapies[0].Adl);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyByAdlReturnsCorrectType()
        {
            var response = await _testController.GetTherapy(_testTherapies[0].Adl);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Therapy>();
        }

        [TestMethod]
        public async Task ValidGetTherapyByAdlReturnsCorrectTherapy()
        {
            var response = await _testController.GetTherapy(_testTherapies[0].Adl);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testTherapies[0]);
        }

        [TestMethod]
        public async Task NonExistingGetTherapyByAdlReturnsNotFoundResponse()
        {
            var response = await _testController.GetTherapy("-1");
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyByAbbreviationReturnsOkResponse()
        {
            var response = await _testController.GetTherapyByAbbreviation(_testTherapies[0].Abbreviation);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyByAbbreviationReturnsCorrectType()
        {
            var response = await _testController.GetTherapyByAbbreviation(_testTherapies[0].Abbreviation);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Therapy>();
        }

        [TestMethod]
        public async Task ValidGetTherapyByAbbreviationReturnsCorrectTherapy()
        {
            var response = await _testController.GetTherapyByAbbreviation(_testTherapies[0].Abbreviation);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testTherapies[0]);
        }

        [TestMethod]
        public async Task NonExistingGetTherapyByAbbreviationReturnsNotFoundResponse()
        {
            var response = await _testController.GetTherapyByAbbreviation("-1");
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyAdlReturnsOkResponse()
        {
            var response = await _testController.GetTherapyAdl();
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyAdlReturnsCorrectType()
        {
            var response = await _testController.GetTherapyAdl();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<string>>();
        }

        [TestMethod]
        public async Task ValidGetTherapyAdlReturnsCorrectCount()
        {
            var response = await _testController.GetTherapyAdl();
            var responseResult = response.Result as OkObjectResult;
            List<string> listOfAdls = (List<string>)responseResult.Value;

            listOfAdls.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task ValidGetTherapyAdlReturnsCorrectAdls()
        {
            var response = await _testController.GetTherapyAdl();
            var responseResult = response.Result as OkObjectResult;
            List<string> listOfAdls = (List<string>)responseResult.Value;

            for(var i = 0; i < listOfAdls.Count; i++)
            {
                _testAdls.Contains(listOfAdls[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetTherapyTypeReturnsOkResponse()
        {
            var response = await _testController.GetTherapyType();
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyTypeReturnsCorrectType()
        {
            var response = await _testController.GetTherapyType();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<string>>();
        }

        [TestMethod]
        public async Task ValidGetTherapyTypeReturnsCorrectCount()
        {
            var response = await _testController.GetTherapyType();
            var responseResult = response.Result as OkObjectResult;
            List<string> listOfTypes = (List<string>)responseResult.Value;

            listOfTypes.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task ValidGetTherapyTypeReturnsCorrectTypes()
        {
            var response = await _testController.GetTherapyType();
            var responseResult = response.Result as OkObjectResult;
            List<string> listOfTypes = (List<string>)responseResult.Value;

            for(var i = 0; i < listOfTypes.Count; i++)
            {
                _testTypes.Contains(listOfTypes[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidPutTherapyReturnsNoContentResponse()
        {
            var response = await _testController.PutTherapy(_testTherapies[0].Adl, _testTherapies[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task ValidPutTherapyCorrectUpdatesData()
        {
            var oldAbbreviation = _testTherapies[0].Abbreviation;
            var newAbbreviation = ModelFakes.TherapyFake.Generate().Abbreviation;
            _testTherapies[0].Abbreviation = newAbbreviation;

            await _testController.PutTherapy(_testTherapies[0].Adl, _testTherapies[0]);

            var response = await _testController.GetTherapy(_testTherapies[0].Adl);
            var responseResult = response.Result as OkObjectResult;
            Therapy therapy = (Therapy)responseResult.Value;

            therapy.Abbreviation.Should().NotBe(oldAbbreviation);
            therapy.Abbreviation.Should().Be(newAbbreviation);
            therapy.Should().Be(_testTherapies[0]);
        }

        [TestMethod]
        public async Task NonMatchingAdlsPutTherapyShouldReturnBadRequestResponse()
        {
            var response = await _testController.PutTherapy("-1", _testTherapies[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingTherapyPutTherapyReturnsNotFoundResponse()
        {
            var therapy = ModelFakes.TherapyFake.Generate();

            var response = await _testController.PutTherapy(therapy.Adl, therapy);

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPostTherapyReturnsCreatedAtActionResponse()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();

            var response = await _testController.PostTherapy(newTherapy);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostTherapyReturnsCorrectType()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();

            var response = await _testController.PostTherapy(newTherapy);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<Therapy>();
        }

        [TestMethod]
        public async Task ValidPostTherapyReturnsCorrectTherapy()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();

            var response = await _testController.PostTherapy(newTherapy);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().Be(newTherapy);
        }

        [TestMethod]
        public async Task ValidPostTherapyCorrectlyAddsTherapy()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();

            await _testController.PostTherapy(newTherapy);

            var response = await _testController.GetTherapy(newTherapy.Adl);
            var responseResult = response.Result as OkObjectResult;
            var therapy = responseResult.Value;

            therapy.Should().Be(newTherapy);
        }

        [TestMethod]
        public async Task ExistingAdlPostTherapyReturnsConflictResponse()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Adl = _testTherapies[0].Adl;

            var response = await _testController.PostTherapy(newTherapy);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task ExistingAdlPostTherapyDoesNotAddTherapy()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Adl = _testTherapies[0].Adl;

            await _testController.PostTherapy(newTherapy);

            var getResponse = await _testController.GetTherapy(newTherapy.Adl);
            var getResponseResult = getResponse.Result as OkObjectResult;
            var getTherapy = getResponseResult.Value;

            getTherapy.Should().NotBe(newTherapy);
        }

        [TestMethod]
        public async Task ExistingAbbreviationPostTherapyReturnsConflictResponse()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Abbreviation = _testTherapies[0].Abbreviation;

            var response = await _testController.PostTherapy(newTherapy);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task ExistingAbbreviationPostTherapyDoesNotAddTherapy()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Abbreviation = _testTherapies[0].Abbreviation;

            await _testController.PostTherapy(newTherapy);

            var getResponse = await _testController.GetTherapyByAbbreviation(newTherapy.Abbreviation);
            var getResponseResult = getResponse.Result as OkObjectResult;
            var getTherapy = getResponseResult.Value;

            getTherapy.Should().NotBe(newTherapy);
        }

        [TestMethod]
        public async Task ValidDeleteTherapyReturnsOkResponse()
        {
            var response = await _testController.DeleteTherapy(_testTherapies[0].Adl);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapyReturnsCorrectType()
        {
            var response = await _testController.DeleteTherapy(_testTherapies[0].Adl);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Therapy>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapyReturnsCorrectTherapy()
        {
            var response = await _testController.DeleteTherapy(_testTherapies[0].Adl);
            var responseResult = response.Result as OkObjectResult;
            var therapy = responseResult.Value;

            therapy.Should().Be(_testTherapies[0]);
        }

        [TestMethod]
        public async Task NonExistingDeleteTherapyReturnsNotFoundResponse()
        {
            var response = await _testController.DeleteTherapy("-1");
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }
    }
}
