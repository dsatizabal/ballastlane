using BallastLane.API.Helpers;

namespace BallastLane.API.Service
{
    public class AuthService : IAuthService
    {
        private string _errorMessage;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserService userService, IPasswordHasher passwordHasher, ILogger<AuthService> logger)
        {
            _errorMessage = string.Empty;
            _userService = userService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public string GetErrorMessage() => _errorMessage;

        public async Task<bool> ValidateCredentials(string username, string password)
        {
            try
            {
                _logger.LogInformation($"Validating credentials for user: {username}");

                var (result, user) = await _userService.GetUserByUsername(username);

                if (!result)
                {
                    _errorMessage = _userService.GetErrorMessage();
                    return false;
                }

                if (user == null)
                {
                    _errorMessage = $"Invalid login";
                    _logger.LogError($"AuthService:ValidateCredentials: {_errorMessage}");
                    return false;
                }

                if (!_passwordHasher.VerifyHashedPassword(user.Password, password))
                {
                    _errorMessage = $"Invalid login";
                    _logger.LogError($"AuthService:ValidateCredentials: {_errorMessage}");
                    return false;
                }

                _logger.LogError($"AuthService:ValidateCredentials: Succesfull Login");
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"AuthService:ValidateCredentials: {_errorMessage}");
                return false;
            }
        }
    }
}
