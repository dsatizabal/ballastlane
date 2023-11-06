using BallastLane.Data.Models;

namespace BallastLane.API.Service
{
    public interface IUserService
    {
        Task<(bool, IEnumerable<UserModel>)> GetAllUsers();
        Task<(bool, UserModel)> GetUserById(string id);
        Task<(bool, UserModel)> GetUserByUsername(string id);
        Task<bool> CreateUser(UserModel client);
        Task<bool> UpdateUser(UserModel client);
        Task<bool> UpdateUserPassword(UpdateUserPasswordModel updateUserPassword);
        Task<bool> DeleteUser(string id);
        string GetErrorMessage();
    }
}
