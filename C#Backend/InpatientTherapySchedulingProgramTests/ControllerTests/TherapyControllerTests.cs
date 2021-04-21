using FluentAssertions;
using InpatientTherapySchedulingProgram.Controllers;
using InpatientTherapySchedulingProgram.Exceptions.TherapyExceptions;
using InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using InpatientTherapySchedulingProgramTests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgramTests.ControllerTests
{
    [TestClass]
    public class TherapyControllerTests
    {
        private static List<TherapyMain> _testTherapyMains;
        private static List<Therapy> _testTherapies;
        private static List<string> _testAdls;
        private static List<string> _testTypes;
        private Mock<ITherapyService> _fakeService;
        private TherapyController _testController;

        [ClassInitialize()]
        public static void Setup(TestContext context)
        {
            _testTherapyMains = new List<TherapyMain>();
            _testTherapies = new List<Therapy>();
            _testAdls = new List<string>();
            _testTypes = new List<string>();

            for(var i = 0; i < 10; i++)
            {
                var newTherapyMain = ModelFakes.TherapyMainFake.Generate();
                _testTherapyMains.Add(newTherapyMain);
                _testTypes.Add(newTherapyMain.Type);

                var therapy = ModelFakes.TherapyFake.Generate();
                therapy.Type = newTherapyMain.Type;
                _testTherapies.Add(therapy);
                _testAdls.Add(therapy.Adl);
            }

            _testTypes = _testTypes.Distinct().ToList();
        }

        [TestInitialize]
        public void Initialize()
        {
            _fakeService = new Mock<ITherapyService>();
            _fakeService.SetupAllProperties();
            _fakeService.Setup(s => s.GetAllTherapies()).ReturnsAsync(_testTherapies);
            _fakeService.Setup(s => s.GetAllAdls()).ReturnsAsync(_testAdls);
            _fakeService.Setup(s => s.GetAllTypes()).ReturnsAsync(_testTypes);
            _fakeService.Setup(s => s.GetTherapyByAbbreviation(It.IsAny<string>())).ReturnsAsync(_testTherapies[0]);
            _fakeService.Setup(s => s.GetTherapyByAdl(It.IsAny<string>())).ReturnsAsync(_testTherapies[0]);
            _fakeService.Setup(s => s.UpdateTherapy(It.IsAny<string>(), It.IsAny<Therapy>())).ReturnsAsync(_testTherapies[0]);
            _fakeService.Setup(s => s.AddTherapy(It.IsAny<Therapy>())).ReturnsAsync(_testTherapies[0]);
            _fakeService.Setup(s => s.DeleteTherapy(It.IsAny<string>())).ReturnsAsync(_testTherapies[0]);

            _testController = new TherapyController(_fakeService.Object);
        }

        [TestMethod]
        public async Task ValidGetAllReturnsOkResponse()
        {
            var response = await _testController.GetTherapy();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllReturnsCorrectType()
        {
            var response = await _testController.GetTherapy();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<Therapy>>();
        }

        [TestMethod]
        public async Task ValidGetTherapyByAdlReturnsOkResponse()
        {
            var response = await _testController.GetTherapy(_testTherapies[0].Adl);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyByAdlReturnsCorrectType()
        {
            var response = await _testController.GetTherapy(_testTherapies[0].Adl);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Therapy>();
        }

        [TestMethod]
        public async Task NullGetTherapyByAdlReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.GetTherapyByAdl(It.IsAny<string>())).ReturnsAsync((Therapy)null);

            var response = await _testController.GetTherapy("-1");

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyByAbbreviationReturnsOkResponse()
        {
            var response = await _testController.GetTherapyByAbbreviation(_testTherapies[0].Abbreviation);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapyByAbbreviationReturnsCorrectType()
        {
            var response = await _testController.GetTherapyByAbbreviation(_testTherapies[0].Abbreviation);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Therapy>();
        }

        [TestMethod]
        public async Task NullGetTherapyByAbbreviationReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.GetTherapyByAbbreviation(It.IsAny<string>())).ReturnsAsync((Therapy)null);

            var response = await _testController.GetTherapyByAbbreviation("-1");

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task GetTherapyTypeReturnsOkResponse()
        {
            var response = await _testController.GetTherapyType();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task GetTherapyTypeReturnsCorrectType()
        {
            var response = await _testController.GetTherapyType();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<string>>();
        }

        [TestMethod]
        public async Task GetTherapyAdlReturnsOkResponse()
        {
            var response = await _testController.GetTherapyAdl();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task GetTherapyAdlReturnsCorrectType()
        {
            var response = await _testController.GetTherapyAdl();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<string>>();
        }

        [TestMethod]
        public async Task ValidPutTherapyReturnsNoContentResponse()
        {
            var response = await _testController.PutTherapy(_testTherapies[0].Adl, _testTherapies[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task TherapyAdlsDoNotMatchExceptionPutTherapyReturnsBadRequestResponse()
        {
            _fakeService.Setup(s => s.UpdateTherapy(It.IsAny<string>(), It.IsAny<Therapy>())).ThrowsAsync(new TherapyAdlsDoNotMatchException());

            var response = await _testController.PutTherapy("-1", _testTherapies[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task TherapyMainDoesNotExistExceptionPutTherapyReturnsBadRequestResponse()
        {
            _fakeService.Setup(s => s.UpdateTherapy(It.IsAny<string>(), It.IsAny<Therapy>())).ThrowsAsync(new TherapyMainDoesNotExistException());

            var fakeTherapyMainType = ModelFakes.TherapyMainFake.Generate().Type;
            _testTherapies[0].Type = fakeTherapyMainType;

            var response = await _testController.PutTherapy(_testTherapies[0].Adl, _testTherapies[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task TherapyDoesNotExistExceptionPutTherapyReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.UpdateTherapy(It.IsAny<string>(), It.IsAny<Therapy>())).ThrowsAsync(new TherapyDoesNotExistException());

            var fakeTherapy = ModelFakes.TherapyFake.Generate();

            var response = await _testController.PutTherapy(fakeTherapy.Adl, fakeTherapy);

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionPutTherapyThrowsError()
        {
            _fakeService.Setup(s => s.UpdateTherapy(It.IsAny<string>(), It.IsAny<Therapy>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testController.Invoking(c => c.PutTherapy(_testTherapies[0].Adl, _testTherapies[0])).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

        [TestMethod]
        public async Task ValidPostTherapyReturnsCreatedAtActionResponse()
        {
            var newTherapy = ModelFakes.TherapyFake.Generate();

            var response = await _testController.PostTherapy(newTherapy);

            response.Result.Should().BeOfType<CreatedAtActionResult>();
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
        public async Task TherapyAdlAlreadyExistExceptionPostTherapyReturnsConflictResponse()
        {
            _fakeService.Setup(s => s.AddTherapy(It.IsAny<Therapy>())).ThrowsAsync(new TherapyAdlAlreadyExistException());

            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Adl = _testTherapies[0].Adl;

            var response = await _testController.PostTherapy(newTherapy);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task TherapyAbbreviationAlreadyExistExceptionPostTherapyReturnsConflictResponse()
        {
            _fakeService.Setup(s => s.AddTherapy(It.IsAny<Therapy>())).ThrowsAsync(new TherapyAbbreviationAlreadyExistException());

            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Abbreviation = _testTherapies[0].Abbreviation;

            var response = await _testController.PostTherapy(newTherapy);

            response.Result.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task TherapyMainDoesNotExistExceptionPostTherapyReturnsBadRequestResponse()
        {
            _fakeService.Setup(s => s.AddTherapy(It.IsAny<Therapy>())).ThrowsAsync(new TherapyMainDoesNotExistException());

            var fakeTherapyMainType = ModelFakes.TherapyMainFake.Generate().Type;

            var newTherapy = ModelFakes.TherapyFake.Generate();
            newTherapy.Type = fakeTherapyMainType;

            var response = await _testController.PostTherapy(newTherapy);

            response.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionPostTherapyThrowsError()
        {
            _fakeService.Setup(s => s.AddTherapy(It.IsAny<Therapy>())).ThrowsAsync(new DbUpdateException());

            var newTherapy = ModelFakes.TherapyFake.Generate();

            await _testController.Invoking(c => c.PostTherapy(newTherapy)).Should().ThrowAsync<DbUpdateException>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapyReturnsOkResponse()
        {
            var response = await _testController.DeleteTherapy(_testTherapies[0].Adl);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapyReturnsCorrectType()
        {
            var response = await _testController.DeleteTherapy(_testTherapies[0].Adl);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Therapy>();
        }

        [TestMethod]
        public async Task NullDeleteTherapyReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.DeleteTherapy(It.IsAny<string>())).ReturnsAsync((Therapy)null);

            var response = await _testController.DeleteTherapy("-1");

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionDeleteTherapyThrowsError()
        {
            _fakeService.Setup(s => s.DeleteTherapy(It.IsAny<string>())).ThrowsAsync(new DbUpdateException());

            await _testController.Invoking(c => c.DeleteTherapy(_testTherapies[0].Adl)).Should().ThrowAsync<DbUpdateException>();
        }
    }
}
