namespace TaskMAPI.Services;

public interface IAuthenticationService
{
    Task<bool> LoginAsync(string email, string password);
    Task LogoutAsync(string accessToken);
}
