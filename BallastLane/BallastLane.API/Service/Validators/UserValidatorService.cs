using BallastLane.API.Helpers;
using BallastLane.Data.Models;

namespace BallastLane.API.Service.Validators
{
    public class UserValidatorService : IUserValidatorService
    {
        public bool IsValid(UserModel user)
        {
            return RegexValidator.IsValid(user.Password, RegexValidator.SecurePasswordRegEx);
        }
    }
}
