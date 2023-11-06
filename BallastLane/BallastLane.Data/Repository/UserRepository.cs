using BallastLane.Data.Models;
using MongoDB.Driver;

namespace BallastLane.Data.Repository
{
    public class UserRepository : BaseMongoRepository<UserModel>, IUserRepository
    {
        public UserRepository(DatabaseSettingsModel settings)
            : base(settings, "users") { }

        public async Task<UserModel> GetByUsernameAsync(string username)
        {
            return await _collection
                .Find(u => u.Username == username)
                .FirstOrDefaultAsync();
        }
    }
}
