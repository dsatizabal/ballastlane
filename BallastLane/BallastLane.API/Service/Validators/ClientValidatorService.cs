using BallastLane.API.Helpers;
using BallastLane.Data.Models;

namespace BallastLane.API.Service.Validators
{
    public class ClientValidatorService : IClientValidatorService
    {
        public bool IsValid(ClientModel client)
        {
            return RegexValidator.IsValid(client.FiscalNumber, RegexValidator.FiscalNumberRegEx);
        }
    }
}
