using BallastLane.API.Controllers;
using BallastLane.API.Service;
using BallastLane.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BallastLane.Test.Controllers
{
    public class ClientControllerTest
    {
        [Fact]
        public async Task Must_Return_All_Clients()
        {
            // Arrange
            var mockService = new Mock<IClientService>();
            var clients = new List<ClientModel>
            {
                new ClientModel { Id = "id1", Name = "name1", Address = "address1", Phone = "phone1", WebSite = "website1", FiscalNumber = "J-87654321", Status = "active" },
                new ClientModel { Id = "id2", Name = "name2", Address = "address2", Phone = "phone2", WebSite = "website2", FiscalNumber = "J-87654322", Status = "active" }
            };
            mockService.Setup(service => service.GetAllClients())
                       .ReturnsAsync((true, clients.AsEnumerable()));

            var controller = new ClientsController(mockService.Object);

            // Act
            var actionResult = await controller.Get();

            // Assert
            var result = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedClients = Assert.IsAssignableFrom<IEnumerable<ClientModel>>(result.Value);
            Assert.Equal(clients.Count, returnedClients.Count());
        }

        [Fact]
        public async Task Must_Return_Single_Client_By_Id()
        {
            // Arrange
            var mockService = new Mock<IClientService>();
            var client = new ClientModel { Id = "id1", Name = "name1", Address = "address1", Phone = "phone1", WebSite = "website1", FiscalNumber = "J-87654321", Status = "active" };

            mockService.Setup(service => service.GetClientById("id1"))
                       .ReturnsAsync((true, client));

            var controller = new ClientsController(mockService.Object);

            // Act
            var actionResult = await controller.Get("id1");

            // Assert
            var result = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedClient = Assert.IsType<ClientModel>(result.Value);
            Assert.Equal("id1", returnedClient.Id);
        }

        [Fact]
        public async Task Must_Return_Not_Found_If_Client_Does_Not_Exists()
        {
            // Arrange
            var mockService = new Mock<IClientService>();

            mockService.Setup(service => service.GetClientById("id1"))
                       .ReturnsAsync((true, null));

            var controller = new ClientsController(mockService.Object);

            // Act
            var actionResult = await controller.Get("id1");

            // Assert
            var result = Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task Must_Create_A_New_Client()
        {
            // Arrange
            var mockService = new Mock<IClientService>();
            var newClient = new ClientModel { Id = "id1", Name = "name1", Address = "address1", Phone = "phone1", WebSite = "website1", FiscalNumber = "J-87654321", Status = "active" };

            mockService.Setup(service => service.CreateClient(It.IsAny<ClientModel>()))
                       .ReturnsAsync(true);

            var controller = new ClientsController(mockService.Object);

            // Act
            var actionResult = await controller.Post(newClient);

            // Assert
            var result = Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task Must_Update_Client()
        {
            // Arrange
            var mockService = new Mock<IClientService>();
            var existingClient = new ClientModel { Id = "id1", Name = "Existing Client" };
            mockService.Setup(service => service.UpdateClient(It.IsAny<ClientModel>()))
                       .ReturnsAsync(true);

            var controller = new ClientsController(mockService.Object);

            // Act
            var actionResult = await controller.Put(existingClient);

            // Assert
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task Must_Delete_Client()
        {
            // Arrange
            var mockService = new Mock<IClientService>();
            mockService.Setup(service => service.DeleteClient("id1"))
                       .ReturnsAsync(true);

            var controller = new ClientsController(mockService.Object);

            // Act
            var actionResult = await controller.Delete("id1");

            // Assert
            Assert.IsType<OkResult>(actionResult);
        }
    }
}
