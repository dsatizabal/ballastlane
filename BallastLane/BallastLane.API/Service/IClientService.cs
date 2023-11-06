using BallastLane.Data.Models;

namespace BallastLane.API.Service
{
    public interface IClientService
    {
        Task<(bool, IEnumerable<ClientModel>)> GetAllClients();
        Task<(bool, ClientModel)> GetClientById(string id);
        Task<bool> CreateClient(ClientModel client);
        Task<bool> UpdateClient(ClientModel client);
        Task<bool> DeleteClient(string id);
        string GetErrorMessage();
    }
}
