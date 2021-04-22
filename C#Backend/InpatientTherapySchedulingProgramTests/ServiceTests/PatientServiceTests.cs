using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services;
using InpatientTherapySchedulingProgramTests.Fakes;
using System.Threading.Tasks;
using InpatientTherapySchedulingProgram.Exceptions.PatientExceptions;
using System;

namespace InpatientTherapySchedulingProgramTests.ServiceTests
{
    [TestClass]
    public class PatientServiceTests
    {
        private List<Patient> _testPatients;
        private Patient _nonActivePatient;
        private CoreDbContext _testContext;
        private PatientService _testService;


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


            _testService = new PatientService(_testContext);
        }

        [TestMethod]
        public async Task GetAllReturnsCorrectType() {
            var allPatients = await _testService.GetAllPatients();

            allPatients.Should().BeOfType<List<Patient>>();
        }

        [TestMethod]
        public async Task GetAllReturnsCorrectNumberOfPatient() {
            var allPatients = await _testService.GetAllPatients();
            List<Patient> listOfPatients = (List<Patient>)allPatients;

            for (var i = 0; i < 10; i++) {
                _testPatients.Contains(listOfPatients[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetPatientByPatientIdReturnsCorrectType() {
            var returnPatient = await _testService.GetPatientByPatientId(_testPatients[0].PatientId);

            returnPatient.Should().BeOfType<Patient>();
        }

        [TestMethod]
        public async Task GetPatientByPatientIdReturnsCorrectPatient() {
            var returnPatient = await _testService.GetPatientByPatientId(_testPatients[0].PatientId);
            returnPatient.Should().Be(_testPatients[0]);
        }
       
        [TestMethod]
        public async Task GetPatientByPatientIdReturnsNullIfPatientDoesNotExist()
        {
            var returnPatient = await _testService.GetPatientByPatientId(-1);

            returnPatient.Should().BeNull();
        }

        [TestMethod]
        public async Task GetPatientByPatientIdReturnsNullIfPatientIsNotActive()
        {
            var returnPatient = await _testService.GetPatientByPatientId(_nonActivePatient.PatientId);

            returnPatient.Should().BeNull();
        }


        [TestMethod]
        public async Task AddPatientIncreasesCountOfUsers()
        {
            var newPatient = ModelFakes.PatientFake.Generate();
            await _testService.AddPatient(newPatient);

            var allPatients = await _testService.GetAllPatients();
            List<Patient> listOfPatients = (List<Patient>)allPatients;

            listOfPatients.Count.Should().Be(11);
        }

        [TestMethod]
        public async Task AddPatientCorrectlyAddsPatientToDatabase()
        {
            var newPatient = ModelFakes.PatientFake.Generate();
            await _testService.AddPatient(newPatient);

            var returnPatient = await _testService.GetPatientByPatientId(newPatient.PatientId);

            returnPatient.Should().Be(newPatient);
        }

        [TestMethod]
        public async Task AddPatientWithExistingIdThrowsError()
        {
            int existingId = _testPatients[0].PatientId;
            var newPatient = ModelFakes.PatientFake.Generate();
            newPatient.PatientId = existingId;

            await _testService.Invoking(s => s.AddPatient(newPatient)).Should().ThrowAsync<PatientIdAlreadyExistException>();
        }

        [TestMethod]
        public async Task DeletePatientDecrementsCount()
        {
            await _testService.DeletePatient(_testPatients[0].PatientId);

            var allPatients = await _testService.GetAllPatients();
            List<Patient> listOfPatients = (List<Patient>)allPatients;

            listOfPatients.Count.Should().Be(9);
        }

        [TestMethod]
        public async Task DeletePatientRemovesPatientFromDatabase()
        {
            await _testService.DeletePatient(_testPatients[0].PatientId);

            var allPatients = await _testService.GetAllPatients();
            List<Patient> listOfPatients = (List<Patient>)allPatients;

            listOfPatients.Contains(_testPatients[0]).Should().BeFalse();
        }

        [TestMethod]
        public async Task DeletePatientReturnsCorrectPatient()
        {
            var returnPatient = await _testService.DeletePatient(_testPatients[0].PatientId);

            bool isEqual = returnPatient.Equals(_testPatients[0]);

            returnPatient.Should().Be(_testPatients[0]);
        }

        [TestMethod]
        public async Task DeletePatientThatDoesExistReturnsNull()
        {
            var returnPatient = await _testService.DeletePatient(-1);

            returnPatient.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdatePatientReturnsCorrectType()
        {
            var returnPatient = await _testService.UpdatePatient(_testPatients[0].PatientId, _testPatients[0]);

            returnPatient.Should().BeOfType<Patient>();
        }

        [TestMethod]
        public async Task UpdatePatientReturnsCorrectPatient()
        {
            var returnPatient = await _testService.UpdatePatient(_testPatients[0].PatientId, _testPatients[0]);

            returnPatient.Should().Be(_testPatients[0]);
        }

        [TestMethod]
        public async Task UpdateUserWithAlteredDataUpdatesCorrectlyInDatabase()
        {
            var newAddress = ModelFakes.PatientFake.Generate().Address;
            _testPatients[0].Address = newAddress;

            await _testService.UpdatePatient(_testPatients[0].PatientId, _testPatients[0]);

            var returnPatient = await _testService.GetPatientByPatientId(_testPatients[0].PatientId);

            returnPatient.Address.Should().Be(newAddress);
        }

        [TestMethod]
        public async Task UpdatePatientWithNonMatchingIdsThrowsPatientPidsDoNotMatchException()
        {
            await _testService.Invoking(s => s.UpdatePatient(_testPatients[1].PatientId, _testPatients[0])).Should().ThrowAsync<PatientPidsDoNotMatchException>();
        }

        [TestMethod]
        public async Task UpdatePatientWithNonExistingPatientThrowsPatientDoesNotExistException()
        {
            var fakePatient = ModelFakes.PatientFake.Generate();
            fakePatient.PatientId = -1;

            await _testService.Invoking(s => s.UpdatePatient(fakePatient.PatientId, fakePatient)).Should().ThrowAsync<PatientDoesNotExistException>();
        }

        [TestMethod]
        public async Task UpdatePatientWithNonActivePatientThrowsPatientDoesNotExistException()
        {
            await _testService.Invoking(s => s.UpdatePatient(_nonActivePatient.PatientId, _nonActivePatient)).Should().ThrowAsync<PatientDoesNotExistException>();
        }
    }
}
