using BallastLane.API.Helpers;
using BallastLane.API.Service.Validators;
using BallastLane.Data.Models;
using BallastLane.Data.Repository;

namespace BallastLane.API.Service
{
    public class UserService : IUserService
    {
        private string _errorMessage;
        private readonly IUserRepository _userRepository;
        private readonly IUserValidatorService _userValidatorService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IUserValidatorService userValidatorService,
            IPasswordHasher passwordHasher,
            ILogger<UserService> logger)
        {
            _errorMessage = string.Empty;
            _userRepository = userRepository;
            _userValidatorService = userValidatorService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public string GetErrorMessage() => _errorMessage;

        public async Task<(bool, IEnumerable<UserModel>)> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Getting all users");
                IEnumerable<UserModel> result = await _userRepository.GetAllAsync();
                return (true, result);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"UserService:GetAllUsers: {_errorMessage}");
                return (false, null);
            }
        }

        public async Task<(bool, UserModel)> GetUserById(string id)
        {
            try
            {
                _logger.LogInformation($"Getting user with Id: {id}");
                UserModel result = await _userRepository.GetAsync(id);
                return (true, result);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"UserService:GetUserById: {_errorMessage}");
                return (false, null);
            }
        }

        public async Task<(bool, UserModel)> GetUserByUsername(string username)
        {
            try
            {
                _logger.LogInformation($"Getting user with username: {username}");
                UserModel result = await _userRepository.GetByUsernameAsync(username);
                return (true, result);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"UserService:GetUserByUsername: {_errorMessage}");
                return (false, null);
            }
        }

        public async Task<bool> CreateUser(UserModel user)
        {
            try
            {
                _logger.LogInformation($"Creating new user");

                UserModel result = await _userRepository.GetByUsernameAsync(user.Username);

                if (result != null)
                {
                    _errorMessage = $"User {user.Username} already exists";
                    _logger.LogError($"UserService:CreateUser: {_errorMessage}");
                    return false;
                }

                if (!_userValidatorService.IsValid(user))
                {
                    _errorMessage = $"Invalid password number, must be at least 8 chars, contain one lowercase or one uppercase letter, one number and at least one of special chars: # $ = *";
                    _logger.LogError($"UserService:CreateUser: {_errorMessage}");
                    return false;
                }

                user.Password = _passwordHasher.HashPassword(user.Password);

                await _userRepository.AddAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"UserService:CreateUser: {_errorMessage}");
                return false;
            }
        }

        public async Task<bool> UpdateUser(UserModel user)
        {
            try
            {
                _logger.LogInformation($"Updating user with Id: {user.Id}");

                UserModel result = await _userRepository.GetAsync(user.Id);

                if (result == null)
                {
                    _errorMessage = $"User with ID {user.Id} was not found";
                    _logger.LogError($"UserService:UpdateUser: {_errorMessage}");
                    return false;
                }

                // Do not change password here, use dedicated method
                user.Password = result.Password;

                await _userRepository.UpdateAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"UserService:UpdateUser: {_errorMessage}");
                return false;
            }
        }

        public async Task<bool> UpdateUserPassword(UpdateUserPasswordModel updateUserPassword)
        {
            try
            {
                _logger.LogInformation($"Updating password for user: {updateUserPassword.Username}");

                UserModel user = await _userRepository.GetByUsernameAsync(updateUserPassword.Username);

                if (user == null)
                {
                    _errorMessage = $"User {updateUserPassword.Username} was not found";
                    _logger.LogError($"UserService:UpdateUserPassword: {_errorMessage}");
                    return false;
                }

                if (!_passwordHasher.VerifyHashedPassword(user.Password, updateUserPassword.OldPassword))
                {
                    _errorMessage = $"Invalid credentials";
                    _logger.LogError($"UserService:UpdateUserPassword: {_errorMessage}");
                    return false;
                }

                user.Password = updateUserPassword.NewPassword;
                if (!_userValidatorService.IsValid(user))
                {
                    _errorMessage = $"Invalid password number, must be at least 8 chars, contain one lowercase or one uppercase letter, one number and at least one of special chars: # $ = *";
                    _logger.LogError($"UserService:UpdateUserPassword: {_errorMessage}");
                    return false;
                }

                user.Password = _passwordHasher.HashPassword(updateUserPassword.NewPassword);

                await _userRepository.UpdateAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"UserService:UpdateUserPassword: {_errorMessage}");
                return false;
            }
        }

        public async Task<bool> DeleteUser(string id)
        {
            try
            {
                _logger.LogInformation($"Deleting user with Id: {id}");

                UserModel result = await _userRepository.GetAsync(id);

                if (result == null)
                {
                    _errorMessage = $"User with ID {id} was not found";
                    return false;
                }

                await _userRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _logger.LogError($"UserService:DeleteUser: {_errorMessage}");
                return false;
            }
        }
    }
}
