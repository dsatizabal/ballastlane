using BallastLane.API.Service;
using BallastLane.API.Service.Validators;
using BallastLane.Data.Models;
using BallastLane.Data.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BallastLane.Test.Services
{
    public class ClientServiceTest
    {
        [Fact]
        public async Task Must_Return_All_Clients()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var mockValidator = new Mock<IClientValidatorService>();
            var mockLogger = new Mock<ILogger<ClientService>>();

            var clients = new List<ClientModel>
            {
                new ClientModel { Id = "id1", Name = "name1", Address = "address1", Phone = "phone1", WebSite = "website1", FiscalNumber = "J-87654321", Status = "active" },
                new ClientModel { Id = "id2", Name = "name2", Address = "address2", Phone = "phone2", WebSite = "website2", FiscalNumber = "J-87654322", Status = "active" }
            };

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(clients);
            var service = new ClientService(mockRepo.Object, mockValidator.Object, mockLogger.Object);

            // Act
            var (result, returnedClients) = await service.GetAllClients();

            // Assert
            Assert.True(result);
            Assert.NotNull(returnedClients);
            Assert.Equal(clients.Count, returnedClients.Count());

            mockRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Must_Return_A_Given_Client_By_Id()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var mockValidator = new Mock<IClientValidatorService>();
            var mockLogger = new Mock<ILogger<ClientService>>();

            var expectedClient = new ClientModel { Id = "id1", Name = "name1", Address = "address1", Phone = "phone1", WebSite = "website1", FiscalNumber = "J-87654321", Status = "active" };

            mockRepo.Setup(repo => repo.GetAsync(expectedClient.Id)).ReturnsAsync(expectedClient);
            var service = new ClientService(mockRepo.Object, mockValidator.Object, mockLogger.Object);

            // Act
            var (result, returnedClient) = await service.GetClientById(expectedClient.Id);

            // Assert
            Assert.True(result);
            Assert.NotNull(returnedClient);
            Assert.Equal(expectedClient.Id, returnedClient.Id);

            mockRepo.Verify(repo => repo.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Must_Add_Client_When_Valid()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var realValidator = new ClientValidatorService();
            var mockLogger = new Mock<ILogger<ClientService>>();

            var client = new ClientModel { Id = "id1", Name = "name1", Address = "address1", Phone = "phone1", WebSite = "website1", FiscalNumber = "J-87654321", Status = "active" };

            var service = new ClientService(mockRepo.Object, realValidator, mockLogger.Object);

            // Act
            var result = await service.CreateClient(client);

            // Assert
            Assert.True(result);
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<ClientModel>()), Times.Once);
        }

        [Fact]
        public async Task Must_Fail_Adding_Invalid_Client()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var realValidator = new ClientValidatorService();
            var mockLogger = new Mock<ILogger<ClientService>>();

            var client = new ClientModel { Id = "id1", Name = "name1", Address = "address1", Phone = "phone1", WebSite = "website1", FiscalNumber = "M-87654321", Status = "active" };

            var service = new ClientService(mockRepo.Object, realValidator, mockLogger.Object);

            // Act
            var result = await service.CreateClient(client);

            // Assert
            Assert.False(result);
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<ClientModel>()), Times.Never);
        }
    }
}
