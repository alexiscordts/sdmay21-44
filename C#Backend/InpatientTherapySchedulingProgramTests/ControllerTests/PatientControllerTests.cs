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
using InpatientTherapySchedulingProgram.Exceptions.PatientExceptions;


namespace InpatientTherapySchedulingProgramTests
{
    [TestClass]
    public class PatientControllerTests
    {
        private static List<Patient> _testPatients;
        private Mock<IPatientService> _fakeService;
        private PatientController _testController;

        [ClassInitialize()]
        public static void ClassSetup(TestContext context)
        {
            _testPatients = new List<Patient>();

            for (var i = 0; i < 10; i++)
            {
                var Patient = ModelFakes.PatientFake.Generate();
                _testPatients.Add(Patient);
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            _fakeService = new Mock<IPatientService>();
            _fakeService.SetupAllProperties();
            _fakeService.Setup(s => s.GetAllPatients()).ReturnsAsync(_testPatients);
            _fakeService.Setup(s => s.GetPatientByPid(It.IsAny<int>())).ReturnsAsync(_testPatients[0]);
            _fakeService.Setup(s => s.UpdatePatient(It.IsAny<int>(), It.IsAny<Patient>())).ReturnsAsync(_testPatients[0]);
            _fakeService.Setup(s => s.AddPatient(It.IsAny<Patient>())).ReturnsAsync(_testPatients[0]);
            _fakeService.Setup(s => s.DeletePatient(It.IsAny<int>())).ReturnsAsync(_testPatients[0]);

            _testController = new PatientController(_fakeService.Object);
        }

        [TestMethod]
        public async Task ValidGetAllReturnsOkResponse()
        {
            var response = await _testController.GetPatient();

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllReturnsCorrectType()
        {
            var response = await _testController.GetPatient();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<Patient>>();
        }

        [TestMethod]
        public async Task ValidGetPatientByPatientIdReturnsOkResponse()
        {
            var response = await _testController.GetPatient(_testPatients[0].Pid);

            response.Result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetPatientByPatientIdReturnsCorrectType()
        {
            var response = await _testController.GetPatient(_testPatients[0].Pid);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Patient>();
        }

        [TestMethod]
        public async Task GetNonExistingPatientByPatientIdReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.GetPatientByPid(It.IsAny<int>())).ReturnsAsync((Patient)null);

            var response = await _testController.GetPatient(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }


        [TestMethod]
        public async Task ValidPutPatientReturnsNoContentResponse()
        {
            var response = await _testController.PutPatient(_testPatients[0].Pid, _testPatients[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task NonMatchingPatientIdPutPatientReturnsBadRequest()
        {
            _fakeService.Setup(s => s.UpdatePatient(It.IsAny<int>(), It.IsAny<Patient>())).ThrowsAsync(new PatientPidsDoNotMatchException());

            var response = await _testController.PutPatient(-1, _testPatients[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingPatientPutPatientReturnsBadRequest()
        {
            _fakeService.Setup(s => s.UpdatePatient(It.IsAny<int>(), It.IsAny<Patient>())).ThrowsAsync(new PatientDoesNotExistException());

            var response = await _testController.PutPatient(_testPatients[0].Pid, new Patient());

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionPutPatientShouldThrowDbUpdateConcurrencyException()
        {
            _fakeService.Setup(s => s.UpdatePatient(It.IsAny<int>(), It.IsAny<Patient>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testController.Invoking(c => c.PutPatient(_testPatients[0].Pid, _testPatients[0])).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

        [TestMethod]
        public async Task ValidPostPatientReturnsCreatedAtActionResponse()
        {
            var response = await _testController.PostPatient(_testPatients[0]);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostPatientReturnsCorrectType()
        {
            var response = await _testController.PostPatient(_testPatients[0]);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<Patient>();
        }

        [TestMethod]
        public async Task ExistingPatientIdPostPatientReturnsConflictResponse()
        {
            _fakeService.Setup(s => s.AddPatient(It.IsAny<Patient>())).ThrowsAsync(new PidAlreadyExistsException());

            var response = await _testController.PostPatient(new Patient());
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task DbUpdateExceptionPostPatientThrowsDbUpdateException()
        {
            _fakeService.Setup(s => s.AddPatient(It.IsAny<Patient>())).ThrowsAsync(new DbUpdateException());

            await _testController.Invoking(c => c.PostPatient(new Patient())).Should().ThrowAsync<DbUpdateException>();
        }

        [TestMethod]
        public async Task ValidDeletePatientReturnsOkResponse()
        {
            var response = await _testController.DeletePatient(_testPatients[0].Pid);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeletePatientReturnsCorrectType()
        {
            var response = await _testController.DeletePatient(_testPatients[0].Pid);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<Patient>();
        }

        [TestMethod]
        public async Task NonExistingPatientDeletePatientReturnsNotFoundResponse()
        {
            _fakeService.Setup(s => s.DeletePatient(It.IsAny<int>())).ReturnsAsync((Patient)null);

            var response = await _testController.DeletePatient(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DbUpdateConcurrencyExceptionDeletePatientThrowsDbUpdateConcurrencyException()
        {
            _fakeService.Setup(s => s.DeletePatient(It.IsAny<int>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await _testController.Invoking(c => c.DeletePatient(-1)).Should().ThrowAsync<DbUpdateConcurrencyException>();
        }

    }
}
