using BallastLane.Data.Models;

namespace BallastLane.Data.Repository
{
    public interface IUserRepository : IRepository<UserModel>
    {
        Task<UserModel> GetByUsernameAsync(string username);
    }
}