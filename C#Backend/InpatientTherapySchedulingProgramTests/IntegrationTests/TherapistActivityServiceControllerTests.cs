using System;
using System.Collections.Generic;
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
    public class TherapistActivityServiceControllerTests
    {
        private List<TherapistActivity> _testTherapistActivities;
        private CoreDbContext _testContext;
        private TherapistActivityService _testService;
        private TherapistActivityController _testController;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TherapistActivityDatabase")
                .Options;
            _testTherapistActivities = new List<TherapistActivity>();
            _testContext = new CoreDbContext(options);
            _testContext.Database.EnsureDeleted();

            for(var i = 0; i < 10; i++)
            {
                var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();
                _testTherapistActivities.Add(ObjectExtensions.Copy(newTherapistActivity));
                _testContext.Add(newTherapistActivity);
                _testContext.SaveChanges();
            }

            _testService = new TherapistActivityService(_testContext);
            _testController = new TherapistActivityController(_testService);
        }

        [TestMethod]
        public async Task ValidGetAllTherapistActivitiesReturnsOkResponse()
        {
            var response = await _testController.GetTherapistActivity();
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetAllTherapistActivitiesReturnsCorrectType()
        {
            var response = await _testController.GetTherapistActivity();
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<List<TherapistActivity>>();
        }

        [TestMethod]
        public async Task ValidGetAllTherapistActivitiesReturnsCorrectCountOfTherapistActivities()
        {
            var response = await _testController.GetTherapistActivity();
            var responseResult = response.Result as OkObjectResult;
            List<TherapistActivity> listOfTherapistActivities = (List<TherapistActivity>)responseResult.Value;

            listOfTherapistActivities.Count.Should().Be(10);
        }

        [TestMethod]
        public async Task ValidGetAllTherapistActivitiesReturnsCorrectTherapistActivities()
        {
            var response = await _testController.GetTherapistActivity();
            var responseResult = response.Result as OkObjectResult;
            List<TherapistActivity> listOfTherapistActivities = (List<TherapistActivity>)responseResult.Value;

            for(var i = 0; i < listOfTherapistActivities.Count; i++)
            {
                _testTherapistActivities.Contains(listOfTherapistActivities[i]).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task ValidGetTherapistActivityReturnsOkResponse()
        {
            var response = await _testController.GetTherapistActivity(_testTherapistActivities[0].ActivityName);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidGetTherapistActivityReturnsCorrectType()
        {
            var response = await _testController.GetTherapistActivity(_testTherapistActivities[0].ActivityName);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task ValidGetTherapistActivityReturnsCorrectTherapistActivity()
        {
            var response = await _testController.GetTherapistActivity(_testTherapistActivities[0].ActivityName);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testTherapistActivities[0]);
        }

        [TestMethod]
        public async Task NonExistingGetTherapistActivityReturnsNotFoundResponse()
        {
            var response = await _testController.GetTherapistActivity("-1");
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPutTherapistActivityReturnsNoContentResponse()
        {
            var response = await _testController.PutTherapistActivity(_testTherapistActivities[0].ActivityName, _testTherapistActivities[0]);

            response.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task ValidPutTherapistActivityCorrectlyUpdatesData()
        {
            var oldIsProductive = _testTherapistActivities[0].IsProductive;
            _testTherapistActivities[0].IsProductive = !oldIsProductive;

            await _testController.PutTherapistActivity(_testTherapistActivities[0].ActivityName, _testTherapistActivities[0]);

            var response = await _testController.GetTherapistActivity(_testTherapistActivities[0].ActivityName);
            var responseResult = response.Result as OkObjectResult;
            TherapistActivity therapistActivity = (TherapistActivity)responseResult.Value;

            therapistActivity.IsProductive.Should().Be(!oldIsProductive);
            therapistActivity.Should().Be(_testTherapistActivities[0]);
        }

        [TestMethod]
        public async Task NonMatchingTherapistActivityNamesPutTherapistActivityReturnsBadRequestResponse()
        {
            var response = await _testController.PutTherapistActivity("-1", _testTherapistActivities[0]);

            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task NonExistingTherapistActivityPutTherapistActivityReturnsNotFoundResponse()
        {
            var therapistActivity = ModelFakes.TherapistActivityFake.Generate();

            var response = await _testController.PutTherapistActivity(therapistActivity.ActivityName, therapistActivity);

            response.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task ValidPostTherapistActivityReturnsCreatedAtActionResponse()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            var response = await _testController.PostTherapistActivity(newTherapistActivity);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task ValidPostTherapistActivityReturnsCorrectType()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            var response = await _testController.PostTherapistActivity(newTherapistActivity);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task ValidPostTherapistActivityReturnsCorrectTherapistActivity()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            var response = await _testController.PostTherapistActivity(newTherapistActivity);
            var responseResult = response.Result as CreatedAtActionResult;

            responseResult.Value.Should().Be(newTherapistActivity);
        }

        [TestMethod]
        public async Task ValidPostTherapistActivityCorrectlyAddsTherapistActivity()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();

            await _testController.PostTherapistActivity(newTherapistActivity);

            var response = await _testController.GetTherapistActivity(newTherapistActivity.ActivityName);
            var responseResult = response.Result as OkObjectResult;
            var therapistActivity = responseResult.Value;

            therapistActivity.Should().Be(newTherapistActivity);
        }

        [TestMethod]
        public async Task ExistingTherapistActivityNamePostTherapistActivityReturnsConflictResponse()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();
            newTherapistActivity.ActivityName = _testTherapistActivities[0].ActivityName;

            var response = await _testController.PostTherapistActivity(newTherapistActivity);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<ConflictObjectResult>();
        }

        [TestMethod]
        public async Task ExistingTherapistActivityNamePostTherapistActivityDoesNotAddTherapistActivity()
        {
            var newTherapistActivity = ModelFakes.TherapistActivityFake.Generate();
            newTherapistActivity.IsProductive = !_testTherapistActivities[0].IsProductive;
            newTherapistActivity.ActivityName = _testTherapistActivities[0].ActivityName;

            await _testController.PostTherapistActivity(newTherapistActivity);

            var getResponse = await _testController.GetTherapistActivity(newTherapistActivity.ActivityName);
            var getResponseResult = getResponse.Result as OkObjectResult;
            var getTherapistActivity = getResponseResult.Value;

            getTherapistActivity.Should().NotBe(newTherapistActivity);
        }

        [TestMethod]
        public async Task ValidDeleteTherapistActivityReturnsOkResponse()
        {
            var response = await _testController.DeleteTherapistActivity(_testTherapistActivities[0].ActivityName);
            var responseResult = response.Result;

            responseResult.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapistActivityReturnsCorrectType()
        {
            var response = await _testController.DeleteTherapistActivity(_testTherapistActivities[0].ActivityName);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().BeOfType<TherapistActivity>();
        }

        [TestMethod]
        public async Task ValidDeleteTherapistActivityReturnsCorrectThearpistActivity()
        {
            var response = await _testController.DeleteTherapistActivity(_testTherapistActivities[0].ActivityName);
            var responseResult = response.Result as OkObjectResult;

            responseResult.Value.Should().Be(_testTherapistActivities[0]);
        }

        [TestMethod]
        public async Task NonExistingTherapistActivityDeleteTherapistActivityReturnsNotFoundResponse()
        {
            var response = await _testController.DeleteTherapistActivity("-1");
            var responseResult = response.Result;

            responseResult.Should().BeOfType<NotFoundResult>();
        }
    }
}
