using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgramTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions;

namespace InpatientTherapySchedulingProgramTests.ControllerTests
{
    [TestClass]
    public class TherapyMainControllerTests
    {
        private static List<TherapyMain> _testTherapyMains;
        private Mock<ITherapyMainService> _fakeTherapyMainService;
        private TherapyMainController _testTherapyMainController;

        [ClassInitialize()]
        public static void ClassSetup(TestContext context)
        {
            _testTherapyMains = new List<TherapyMain>();

            for (var i = 0; i < 10; i++)
            {
                var newTherapyMain = ModelFakes.TherapyMainFake.Generate();
                _testTherapyMains.Add(newTherapyMain);
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            _fakeTherapyMainService = new Mock<ITherapyMainService>();
            _fakeTherapyMainService.SetupAllProperties();
            _fakeTherapyMainService.Setup(s => s.GetAllTherapyMains()).ReturnsAsync(_testTherapyMains);
            _fakeTherapyMainService.Setup(s => s.GetTherapyMainByType(It.IsAny<string>())).ReturnsAsync(_testTherapyMains[0]);
            _fakeTherapyMainService.Setup(s => s.GetTherapyMainByAbbreviation(It.IsAny<string>())).ReturnsAsync(_testTherapyMains[0]);
            _fakeTherapyMainService.Setup(s => s.UpdateTherapyMain(It.IsAny<string>(), It.IsAny<TherapyMain>())).ReturnsAsync(_testTherapyMains[0]);
            _fakeTherapyMainService.Setup(s => s.AddTherapyMain(It.IsAny<TherapyMain>())).ReturnsAsync(_testTherapyMains[0]);
            _fakeTherapyMainService.Setup(s => s.DeleteTherapyMain(It.IsAny<string>())).ReturnsAsync(_testTherapyMains[0]);

            _testTherapyMainController = new TherapyMainController(_fakeTherapyMainService.Object);
        }

        [TestMethod]
        public async Task ValidGetAllTherapyMainReturnsOkResponse()
        {
            var response = await _testTherapyMainController.GetTherapyMain();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllTherapyMainReturnsType()
        {
            var response = await _testTherapyMainController.GetTherapyMain();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<TherapyMain>>();
        }

        [TestMethod]
        public async Task ValidGetTherapyMainByTypeReturnsOkResponse()
        {
            var response = await _testTherapyMainController.GetTherapyMainByType(_testTherapyMains[0].Type);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyMainByTypeReturnsCorrectType()
        {
            var response = await _testTherapyMainController.GetTherapyMainByType(_testTherapyMains[0].Type);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapyMain>();
        }

        [TestMethod]
        public async Task NullGetTherapyMainByTypeReturnsNotFoundResponse()
        {
            _fakeTherapyMainService.Setup(s => s.GetTherapyMainByType(It.IsAny<string>())).ReturnsAsync((TherapyMain)null);

            var response = await _testTherapyMainController.GetTherapyMainByType(_testTherapyMains[0].Type);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyMainByAbbreviationReturnsOkResponse()
        {
            var response = await _testTherapyMainController.GetTherapyMainByAbbreviation(_testTherapyMains[0].Abbreviation);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyMainByAbbreviationReturnsCorrectType()
        {
            var response = await _testTherapyMainController.GetTherapyMainByAbbreviation(_testTherapyMains[0].Abbreviation);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapyMain>();
        }

        [TestMethod]
        public async Task NullGetTherapyMainByAbbreviationReturnsNotFoundResponse()
        {
            _fakeTherapyMainService.Setup(s => s.GetTherapyMainByAbbreviation(It.IsAny<string>())).ReturnsAsync((TherapyMain)null);

            var response = await _testTherapyMainController.GetTherapyMainByAbbreviation("-1");

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutTherapyMainReturnsNoContentResponse()
        {
            var response = await _testTherapyMainController.PutTherapyMain(_testTherapyMains[0].Type, _testTherapyMains[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task TherapyMainTypesDoNotMatchExceptionPutTherapyMainReturnsBadRequestResponse()
        {
            _fakeTherapyMainService.Setup(s => s.UpdateTherapyMain(It.IsAny<string>(), It.IsAny<TherapyMain>())).ThrowsAsync(new TherapyMainTypesDoNotMatchException());

            var response = await _testTherapyMainController.PutTherapyMain("-1", _testTherapyMains[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task TherapyMainDoesNotExistsExceptionPutTherapyMainReturnsNotFoundResponse()
        {
            _fakeTherapyMainService.Setup(s => s.UpdateTherapyMain(It.IsAny<string>(), It.IsAny<TherapyMain>())).ThrowsAsync(new TherapyMainDoesNotExistsException());

            var fakeTherapyMain = ModelFakes.TherapyMainFake.Generate();

            var response = await _testTherapyMainController.PutTherapyMain(fakeTherapyMain.Type, fakeTherapyMain);

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionPutTherapyMainThrowsDbUpdateConcurrencyException()
        {
            _fakeTherapyMainService.Setup(s => s.UpdateTherapyMain(It.IsAny<string>(), It.IsAny<TherapyMain>())).ThrowsAsync(new DbUpdateConcurrencyException());

            var fakeTherapyMain = ModelFakes.TherapyMainFake.Generate();

            await _testTherapyMainController.Invoking(s => s.PutTherapyMain(fakeTherapyMain.Type, fakeTherapyMain)).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

        [TestMethod]
        public async Task ValidPostTherapyMainReturnsCreatedAtActionResponse()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();

            var response = await _testTherapyMainController.PostTherapyMain(newTherapyMain);

            response.Result.Should().BeOfType<CreatedAtActionResult>();
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
        public async Task TherapyMainTypeAlreadyExistsExceptionPostTherapyMainReturnsConflictResponse()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();
            newTherapyMain.Type = _testTherapyMains[0].Type;

            _fakeTherapyMainService.Setup(s => s.AddTherapyMain(It.IsAny<TherapyMain>())).ThrowsAsync(new TherapyMainTypeAlreadyExistsException());

            var response = await _testTherapyMainController.PostTherapyMain(newTherapyMain);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task TherapyMainTypeAbbreviationAlreadyExistsExceptionPostTherapyMainReturnsConflictResponse()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();
            newTherapyMain.Abbreviation = _testTherapyMains[0].Abbreviation;

            _fakeTherapyMainService.Setup(s => s.AddTherapyMain(It.IsAny<TherapyMain>())).ThrowsAsync(new TherapyMainTypeAbbreviationAlreadyExistsException());

            var response = await _testTherapyMainController.PostTherapyMain(newTherapyMain);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionPostTherapyMainThrowsDbUpdateConcurrencyException()
        {
            var newTherapyMain = ModelFakes.TherapyMainFake.Generate();

            _fakeTherapyMainService.Setup(s => s.AddTherapyMain(It.IsAny<TherapyMain>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testTherapyMainController.Invoking(s => s.PostTherapyMain(newTherapyMain)).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapyMainReturnsOkResponse()
        {
            var response = await _testTherapyMainController.DeleteTherapyMain(_testTherapyMains[0].Type);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapyMainReturnsCorrectType()
        {
            var response = await _testTherapyMainController.DeleteTherapyMain(_testTherapyMains[0].Type);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapyMain>();
        }

        [TestMethod]
        public async Task NullDeleteTherapyMainReturnsNotFoundResponse()
        {
            _fakeTherapyMainService.Setup(s => s.DeleteTherapyMain(It.IsAny<string>())).ReturnsAsync((TherapyMain)null);

            var response = await _testTherapyMainController.DeleteTherapyMain(_testTherapyMains[0].Type);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionDeleteTherapyMainThrowsDbUpdateConcurrencyException()
        {
            _fakeTherapyMainService.Setup(s => s.DeleteTherapyMain(It.IsAny<string>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testTherapyMainController.Invoking(s => s.DeleteTherapyMain(_testTherapyMains[0].Type)).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }
}
