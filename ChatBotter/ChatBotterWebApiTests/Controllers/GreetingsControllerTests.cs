using CBLib;
using CBLib.Entities;
using ChatBotterWebApi.Controllers;
using ChatBotterWebApi.Data;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChatBotterWebApiTests.Controllers
{
    public class GreetingsControllerTests
    {
        [Fact]
        public async Task ReturnsAllAppGreetings()
        {
            //Arrange
            var mockGreetingRepo = new Mock<IGreetingsRepository>();
            mockGreetingRepo.Setup(repo => repo.GetAllAppGreetingsAsync())
                .Returns(Task.FromResult(GetFakeGreetings()));
            var controller = new GreetingsController(null, null, mockGreetingRepo.Object);

            //Act
            var res = await controller.GetAllAppGreetings();

            //Assert
            Assert.Equal(3, res.ExecuteResultAsync().

        }

        private IEnumerable<Greeting> GetFakeGreetings()
        {
            var res = new List<Greeting>()
            {
                new Greeting()
                {
                    Id = 1,
                    MainGreeting = "Hiiiiii!",
                    ProjectId = 1
                },
                new Greeting()
                {
                    Id = 2,
                    MainGreeting = "Hiiiiii2!",
                    ProjectId = 1
                },
                new Greeting()
                {
                    Id = 3,
                    MainGreeting = "hello....",
                    ProjectId = 2
                },
            };

            return res;
        }
    }
}
