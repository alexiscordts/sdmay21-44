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

namespace InpatientTherapySchedulingProgramTests.IntegrationTests
{
    [TestClass]
    public class PatientServiceControllerTests
    {
        private List<Patient> _testPatients;
        private CoreDbContext _testContext;
        private PatientService _testService;
        private PatientController _testController;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "PatientDatabase")
                .Options;
            _testPatients = new List<Patient>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            for (var i = 0; i < 10; i++)
            {
                var newPatient = ModelFakes.PatientFake.Generate();
                _testPatients.Add(ObjectExtensions.Copy(newPatient));
                _testContext.Add(newPatient);
                _testContext.SaveChanges();
            }

            _testService = new PatientService(_testContext);
            _testController = new PatientController(_testService);
        }

        [TestMethod]
        public async Task ValidGetPatientReturnsOkResponse()
        {
            var response = await _testController.GetPatient();
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetPatientReturnsCorrectType()
        {
            var response = await _testController.GetPatient();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<Patient>>();
        }

        [TestMethod]
        public async Task ValidGetPatientReturnsCorrectCountOfPatients()
        {
            var response = await _testController.GetPatient();
            var responseResult = response.Result as OkObjectResult;
            var listOfPatients = (List<Patient>)responseResult.Value;

            listOfPatients.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task ValidGetPatientReturnsCorrectPatients()
        {
            var response = await _testController.GetPatient();
            var responseResult = response.Result as OkObjectResult;
            var listOfPatients = (List<Patient>)responseResult.Value;

            for(var i = 0; i < 10; i++)
            {
                _testPatients.Contains(listOfPatients[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetPatientByIdReturnsOkResponse()
        {
            var response = await _testController.GetPatient(_testPatients[0].PatientId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetPatientByIdReturnsCorrectType()
        {
            var response = await _testController.GetPatient(_testPatients[0].PatientId);
            var responseResult = response.Result as OkObjectResult;
            var patient = responseResult.Value;

            patient.Should().BeOfType<Patient>();
        }

        [TestMethod]
        public async Task ValidGetPatientByIdReturnsCorrectPatient()
        {
            var response = await _testController.GetPatient(_testPatients[0].PatientId);
            var responseResult = response.Result as OkObjectResult;
            var patient = responseResult.Value;

            patient.Should().Be(_testPatients[0]);
        }

        [TestMethod]
        public async Task NonExistingGetPatientByIdReturnsNotFoundResponse()
        {
            var response = await _testController.GetPatient(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutPatientReturnsNoContentResponse()
        {
            var response = await _testController.PutPatient(_testPatients[0].PatientId, _testPatients[0]);

            response.Should().BeOfType<NoContentResult>();
        }


        [TestMethod]
        public async Task NonMatchingPutPatientIdsShouldReturnBadRequest()
        {
            var response = await _testController.PutPatient(-1, _testPatients[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingPutPatientShouldReturnBadRequest()
        {
            var response = await _testController.PutPatient(-1, new Patient());

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task ValidPostPatientReturnsCreatedAtActionResponse()
        {
            var newPatient = ModelFakes.PatientFake.Generate();
            var response = await _testController.PostPatient(newPatient);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostPatientCorrectlyAddsPatient()
        {
            var newPatient = ModelFakes.PatientFake.Generate();
            await _testController.PostPatient(newPatient);

            var response = await _testController.GetPatient(newPatient.PatientId);
            var responseResult = response.Result as OkObjectResult;
            var patient = responseResult.Value;

            patient.Should().Be(newPatient);
        }

        [TestMethod]
        public async Task ExistingPatientIdPostPatientReturnsConflict()
        {
            var newPatient = ModelFakes.PatientFake.Generate();
            newPatient.PatientId = _testPatients[0].PatientId;

            var response = await _testController.PostPatient(newPatient);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task ExistingPatientIdPostPatientDoesNotAddPatient()
        {
            var newPatient = ModelFakes.PatientFake.Generate();
            newPatient.PatientId = _testPatients[0].PatientId;
            
            await _testController.PostPatient(newPatient);

            var getResponse = await _testController.GetPatient(newPatient.PatientId);
            var getResponseResult = getResponse.Result as OkObjectResult;
            var getPatient = getResponseResult.Value;

            getPatient.Should().NotBe(newPatient);
        }


        [TestMethod]
        public async Task ValidDeletePatientReturnsOkResponse()
        {
            var response = await _testController.DeletePatient(_testPatients[0].PatientId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeletePatientReturnsCorrectType()
        {
            var response = await _testController.DeletePatient(_testPatients[0].PatientId);
            var responseResult = response.Result as OkObjectResult;
            var patient = responseResult.Value; 

            patient.Should().BeOfType<Patient>();
        }

        [TestMethod]
        public async Task ValidDeletePatientReturnsCorrectPatient()
        {
            var response = await _testController.DeletePatient(_testPatients[0].PatientId);
            var responseResult = response.Result as OkObjectResult;
            var patient = responseResult.Value;

            patient.Should().Be(_testPatients[0]);
        }

        [TestMethod]
        public async Task ValidDeletePatientCorrectlyRemovesPatient()
        {
            await _testController.DeletePatient(_testPatients[0].PatientId);

            var response = await _testController.GetPatient(_testPatients[0].PatientId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task NonExistingDeletePatientReturnsNotFound()
        {
            var response = await _testController.DeletePatient(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }
    }
}
