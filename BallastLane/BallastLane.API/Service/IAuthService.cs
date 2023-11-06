namespace BallastLane.API.Service
{
    public interface IAuthService
    {
        string GetErrorMessage();
        Task<bool> ValidateCredentials(string username, string password);
    }
}