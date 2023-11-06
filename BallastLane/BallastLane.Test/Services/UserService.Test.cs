using BallastLane.API.Helpers;
using BallastLane.API.Service;
using BallastLane.API.Service.Validators;
using BallastLane.Data.Models;
using BallastLane.Data.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BallastLane.Test.Services
{
    public class UserServiceTest
    {
        [Fact]
        public async Task Must_Return_All_Users()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var mockValidator = new Mock<IUserValidatorService>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            var mockLogger = new Mock<ILogger<UserService>>();

            var users = new List<UserModel>
            {
                new UserModel { Id = "id1", Name = "name1", LastName = "lastname1", Email= "name1@foo.com", Username = "username1", Password = "password1" },
                new UserModel { Id = "id2", Name = "name2", LastName = "lastname2", Email= "name2@foo.com", Username = "username2", Password = "password2" }
            };

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);
            var service = new UserService(mockRepo.Object, mockValidator.Object, mockPasswordHasher.Object, mockLogger.Object);

            // Act
            var (result, returnedUsers) = await service.GetAllUsers();

            // Assert
            Assert.True(result);
            Assert.NotNull(returnedUsers);
            Assert.Equal(users.Count, returnedUsers.Count());

            mockRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Must_Return_A_Given_User_By_Id()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var mockValidator = new Mock<IUserValidatorService>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            var mockLogger = new Mock<ILogger<UserService>>();

            var expectedUser = new UserModel { Id = "id1", Name = "name1", LastName = "lastname1", Email = "name1@foo.com", Username = "username1", Password = "password1" };

            mockRepo.Setup(repo => repo.GetAsync(expectedUser.Id)).ReturnsAsync(expectedUser);
            var service = new UserService(mockRepo.Object, mockValidator.Object, mockPasswordHasher.Object, mockLogger.Object);

            // Act
            var (result, returnedClient) = await service.GetUserById(expectedUser.Id);

            // Assert
            Assert.True(result);
            Assert.NotNull(returnedClient);
            Assert.Equal(expectedUser.Id, returnedClient.Id);

            mockRepo.Verify(repo => repo.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Must_Add_User_When_Valid()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var realValidator = new UserValidatorService();
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            var mockLogger = new Mock<ILogger<UserService>>();

            var user = new UserModel { Id = "id1", Name = "name1", LastName = "lastname1", Email = "name1@foo.com", Username = "username1", Password = "MySt#ongPa33word" };

            var service = new UserService(mockRepo.Object, realValidator, mockPasswordHasher.Object, mockLogger.Object);

            // Act
            var result = await service.CreateUser(user);

            // Assert
            Assert.True(result);
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<UserModel>()), Times.Once);
        }

        [Fact]
        public async Task Must_Fail_Adding_Invalid_Client()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var realValidator = new UserValidatorService();
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            var mockLogger = new Mock<ILogger<UserService>>();

            var user = new UserModel { Id = "id1", Name = "name1", LastName = "lastname1", Email = "name1@foo.com", Username = "username1", Password = "password1" };

            var service = new UserService(mockRepo.Object, realValidator, mockPasswordHasher.Object, mockLogger.Object);

            // Act
            var result = await service.CreateUser(user);

            // Assert
            Assert.False(result);
            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<UserModel>()), Times.Never);
        }
    }
}
