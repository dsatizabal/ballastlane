using BallastLane.Data.Models;

namespace BallastLane.API.Service.Validators
{
    public interface IUserValidatorService
    {
        bool IsValid(UserModel user);
    }
}