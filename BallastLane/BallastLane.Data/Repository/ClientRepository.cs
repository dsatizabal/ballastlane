using BallastLane.Data.Models;

namespace BallastLane.Data.Repository
{
    public class ClientRepository : BaseMongoRepository<ClientModel>, IClientRepository
    {
        public ClientRepository(DatabaseSettingsModel settings)
            : base(settings, "clients") { }
    }
}
