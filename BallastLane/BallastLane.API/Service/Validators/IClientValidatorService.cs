using BallastLane.Data.Models;

namespace BallastLane.API.Service.Validators
{
    public interface IClientValidatorService
    {
        bool IsValid(ClientModel client);
    }
}