using BallastLane.API.Controllers;
using BallastLane.API.Service;
using BallastLane.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BallastLane.Test.Controllers
{
    public class UserControllerTest
    {
        [Fact]
        public async Task Must_Return_All_users()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            var users = new List<UserModel>
            {
                new UserModel { Id = "id1", Name = "name1", LastName = "lastname1", Email= "name1@foo.com", Username = "username1", Password = "password1" },
                new UserModel { Id = "id2", Name = "name2", LastName = "lastname2", Email= "name2@foo.com", Username = "username2", Password = "password2" }
            };
            mockService.Setup(service => service.GetAllUsers())
                       .ReturnsAsync((true, users.AsEnumerable()));

            var controller = new UsersController(mockService.Object);

            // Act
            var actionResult = await controller.Get();

            // Assert
            var result = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserModel>>(result.Value);
            Assert.Equal(users.Count, returnedUsers.Count());
        }

        [Fact]
        public async Task Must_Return_Single_User_By_Id()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            var client = new UserModel { Id = "id1", Name = "name2", LastName = "lastname2", Email = "name2@foo.com", Username = "username2", Password = "password2" };

            mockService.Setup(service => service.GetUserById("id1"))
                       .ReturnsAsync((true, client));

            var controller = new UsersController(mockService.Object);

            // Act
            var actionResult = await controller.Get("id1");

            // Assert
            var result = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedUser = Assert.IsType<UserModel>(result.Value);
            Assert.Equal("id1", returnedUser.Id);
        }

        [Fact]
        public async Task Must_Return_Not_Found_If_User_Does_Not_Exists()
        {
            // Arrange
            var mockService = new Mock<IUserService>();

            mockService.Setup(service => service.GetUserById("id1"))
                       .ReturnsAsync((true, null));

            var controller = new UsersController(mockService.Object);

            // Act
            var actionResult = await controller.Get("id1");

            // Assert
            var result = Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task Must_Create_A_New_User()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            var newUser = new UserModel { Id = "id2", Name = "name2", LastName = "lastname2", Email = "name2@foo.com", Username = "username2", Password = "password2" };

            mockService.Setup(service => service.CreateUser(It.IsAny<UserModel>()))
                       .ReturnsAsync(true);

            var controller = new UsersController(mockService.Object);

            // Act
            var actionResult = await controller.Post(newUser);

            // Assert
            var result = Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task Must_Update_User()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            var existingUser = new UserModel { Id = "id2", Name = "name2", LastName = "lastname2", Email = "name2@foo.com", Username = "username2", Password = "password2" };

            mockService.Setup(service => service.UpdateUser(It.IsAny<UserModel>()))
                       .ReturnsAsync(true);

            var controller = new UsersController(mockService.Object);

            // Act
            var actionResult = await controller.Put(existingUser);

            // Assert
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task Must_Delete_User()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            mockService.Setup(service => service.DeleteUser("id1"))
                       .ReturnsAsync(true);

            var controller = new UsersController(mockService.Object);

            // Act
            var actionResult = await controller.Delete("id1");

            // Assert
            Assert.IsType<OkResult>(actionResult);
        }
    }
}
