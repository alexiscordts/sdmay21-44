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
        private Patient _nonActivePatient;
        private CoreDbContext _testContext;
        private PatientService _testPatientService;
        private PatientController _testPatientController;

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
                _testContext.Add(newPatient);
                _testContext.SaveChanges();
                _testPatients.Add(ObjectExtensions.Copy(newPatient));
            }

            _nonActivePatient = ModelFakes.PatientFake.Generate();
            _nonActivePatient.Active = false;
            _testContext.Add(_nonActivePatient);
            _testContext.SaveChanges();
            _testPatients.Add(ObjectExtensions.Copy(_nonActivePatient));

            _testPatientService = new PatientService(_testContext);
            _testPatientController = new PatientController(_testPatientService);
        }

        [TestMethod]
        public async Task ValidGetPatientReturnsOkResponse()
        {
            var response = await _testPatientController.GetPatient();
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetPatientReturnsCorrectType()
        {
            var response = await _testPatientController.GetPatient();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<Patient>>();
        }

        [TestMethod]
        public async Task ValidGetPatientReturnsCorrectCountOfPatients()
        {
            var response = await _testPatientController.GetPatient();
            var responseResult = response.Result as OkObjectResult;
            var listOfPatients = (List<Patient>)responseResult.Value;

            listOfPatients.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task ValidGetPatientReturnsCorrectPatients()
        {
            var response = await _testPatientController.GetPatient();
            var responseResult = response.Result as OkObjectResult;
            var listOfPatients = (List<Patient>)responseResult.Value;

            for(var i = 0; i < listOfPatients.Count; i++)
            {
                _testPatients.Contains(listOfPatients[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetPatientByIdReturnsOkResponse()
        {
            var response = await _testPatientController.GetPatient(_testPatients[0].PatientId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetPatientByIdReturnsCorrectType()
        {
            var response = await _testPatientController.GetPatient(_testPatients[0].PatientId);
            var responseResult = response.Result as OkObjectResult;
            var patient = responseResult.Value;

            patient.Should().BeOfType<Patient>();
        }

        [TestMethod]
        public async Task ValidGetPatientByIdReturnsCorrectPatient()
        {
            var response = await _testPatientController.GetPatient(_testPatients[0].PatientId);
            var responseResult = response.Result as OkObjectResult;
            var patient = responseResult.Value;

            patient.Should().Be(_testPatients[0]);
        }

        [TestMethod]
        public async Task NonExistingPatientGetPatientByPatientIdReturnsNotFoundResponse()
        {
            var response = await _testPatientController.GetPatient(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task NonActivePatientGetPatientByPatientIdReturnsNotFoundResponse()
        {
            var response = await _testPatientController.GetPatient(_nonActivePatient.PatientId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutPatientReturnsNoContentResponse()
        {
            var response = await _testPatientController.PutPatient(_testPatients[0].PatientId, _testPatients[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task ValidPutPatientWithAlteredDataCorrectlyUpdatesInDatabase()
        {
            _testPatients[0].FirstName = ModelFakes.PatientFake.Generate().FirstName;

            await _testPatientController.PutPatient(_testPatients[0].PatientId, _testPatients[0]);

            var response = await _testPatientController.GetPatient(_testPatients[0].PatientId);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testPatients[0]);
        }


        [TestMethod]
        public async Task NonMatchingPutPatientIdsShouldReturnBadRequestResponse()
        {
            var response = await _testPatientController.PutPatient(-1, _testPatients[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingPutPatientShouldReturnBadRequestResponse()
        {
            var response = await _testPatientController.PutPatient(-1, new Patient());

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonActivePatientPutPatientShouldReturnBadRequestResponse()
        {
            var response = await _testPatientController.PutPatient(_nonActivePatient.PatientId, _nonActivePatient);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task ValidPostPatientReturnsCreatedAtActionResponse()
        {
            var newPatient = ModelFakes.PatientFake.Generate();
            var response = await _testPatientController.PostPatient(newPatient);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostPatientCorrectlyAddsPatientToDatabase()
        {
            var newPatient = ModelFakes.PatientFake.Generate();
            await _testPatientController.PostPatient(newPatient);

            var response = await _testPatientController.GetPatient(newPatient.PatientId);
            var responseResult = response.Result as OkObjectResult;
            var patient = responseResult.Value;

            patient.Should().Be(newPatient);
        }

        [TestMethod]
        public async Task ExistingPatientIdPostPatientReturnsConflictResponse()
        {
            var newPatient = ModelFakes.PatientFake.Generate();
            newPatient.PatientId = _testPatients[0].PatientId;

            var response = await _testPatientController.PostPatient(newPatient);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task ExistingPatientIdPostPatientDoesNotAddPatient()
        {
            var newPatient = ModelFakes.PatientFake.Generate();
            newPatient.PatientId = _testPatients[0].PatientId;
            
            await _testPatientController.PostPatient(newPatient);

            var getResponse = await _testPatientController.GetPatient(newPatient.PatientId);
            var getResponseResult = getResponse.Result as OkObjectResult;
            var getPatient = getResponseResult.Value;

            getPatient.Should().NotBe(newPatient);
        }


        [TestMethod]
        public async Task ValidDeletePatientReturnsOkResponse()
        {
            var response = await _testPatientController.DeletePatient(_testPatients[0].PatientId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeletePatientReturnsCorrectType()
        {
            var response = await _testPatientController.DeletePatient(_testPatients[0].PatientId);
            var responseResult = response.Result as OkObjectResult;
            var patient = responseResult.Value; 

            patient.Should().BeOfType<Patient>();
        }

        [TestMethod]
        public async Task ValidDeletePatientReturnsCorrectPatient()
        {
            var response = await _testPatientController.DeletePatient(_testPatients[0].PatientId);
            var responseResult = response.Result as OkObjectResult;
            var patient = responseResult.Value;

            patient.Should().Be(_testPatients[0]);
        }

        [TestMethod]
        public async Task ValidDeletePatientCorrectlyRemovesPatientFromDatabase()
        {
            await _testPatientController.DeletePatient(_testPatients[0].PatientId);

            var response = await _testPatientController.GetPatient(_testPatients[0].PatientId);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task NonExistingDeletePatientReturnsNotFoundResponse()
        {
            var response = await _testPatientController.DeletePatient(-1);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }
    }
}
