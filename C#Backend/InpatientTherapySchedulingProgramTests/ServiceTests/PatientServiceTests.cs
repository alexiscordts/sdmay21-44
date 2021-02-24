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
        private CoreDbContext _testContext;
        private PatientService _testService;


        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "UserDatabase")
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
        }

    }
}
