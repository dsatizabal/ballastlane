using BallastLane.API.Service.Validators;
using BallastLane.Data.Models;
using BallastLane.Data.Repository;

namespace BallastLane.API.Service
{
    public class ClientService : IClientService
    {
        private string _errorMessage;
        private readonly IClientRepository _clientRepository;
        private readonly IClientValidatorService _clientValidatorService;
        private readonly ILogger<ClientService> _logger;

        public string GetErrorMessage() => _errorMessage;


        public ClientService(IClientRepository clientRepository, IClientValidatorService clientValidatorService, ILogger<ClientService> logger)
        {
            _errorMessage = string.Empty;
            _clientRepository = clientRepository;
            _clientValidatorService = clientValidatorService;
            _logger = logger;
        }

        public async Task<(bool, IEnumerable<ClientModel>)> GetAllClients()
        {
            try
            {
                _logger.LogInformation("Getting all clients");

                IEnumerable<ClientModel> result = await _clientRepository.GetAllAsync();
                return (true, result);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"ClientService:GetAllClients: {_errorMessage}");
                return (false, null);
            }
        }

        public async Task<(bool, ClientModel)> GetClientById(string id)
        {
            try
            {
                _logger.LogInformation($"Getting client with Id: {id}");

                ClientModel result = await _clientRepository.GetAsync(id);
                return (true, result);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"ClientService:GetClientById: {_errorMessage}");
                return (false, null);
            }
        }

        public async Task<bool> CreateClient(ClientModel client)
        {
            try
            {
                _logger.LogInformation($"Creating new client");

                if (!_clientValidatorService.IsValid(client))
                {
                    _errorMessage = $"Invalid fiscal number: {client.FiscalNumber}";
                    return false;
                }

                await _clientRepository.AddAsync(client);
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"ClientService:CreateClient: {_errorMessage}");
                return false;
            }
        }

        public async Task<bool> UpdateClient(ClientModel client)
        {
            try
            {
                _logger.LogInformation($"Updating client with Id: {client.Id}");

                ClientModel result = await _clientRepository.GetAsync(client.Id);

                if (result == null)
                {
                    _errorMessage = $"Client with ID {client.Id} was not found";
                    return false;
                }

                if (!_clientValidatorService.IsValid(client))
                {
                    _errorMessage = $"Invalid fiscal number: {client.FiscalNumber}";
                    return false;
                }

                await _clientRepository.UpdateAsync(client);
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"ClientService:UpdateClient: {_errorMessage}");
                return false;
            }
        }

        public async Task<bool> DeleteClient(string id)
        {
            try
            {
                _logger.LogInformation($"Deleting client with Id: {id}");

                ClientModel result = await _clientRepository.GetAsync(id);

                if (result == null)
                {
                    _errorMessage = $"Client with ID {id} was not found";
                    return false;
                }

                await _clientRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"ClientService:DeleteClient: {_errorMessage}");
                return false;
            }
        }
    }
}
