using System;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;

namespace TaskMAPI.Services;

public class CognitoAuthService : IAuthenticationService
{
    private readonly IAmazonCognitoIdentityProvider _cognitoClient;
    private readonly ILogger<CognitoAuthService> _logger;
    private readonly string _clientId;

    public CognitoAuthService(
        IAmazonCognitoIdentityProvider cognitoClient,
        IConfiguration configuration,
        ILogger<CognitoAuthService> logger)
    {
        _cognitoClient = cognitoClient;
        _logger = logger;
        _clientId = configuration["AWS:UserPoolClientId"];
    }
    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var loginRequest = new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                ClientId = _clientId,
                AuthParameters = new Dictionary<string, string>
                {
                    {"USERNAME", email},
                    {"PASSWORD", password}
                }
            };

            var loginResponse = await _cognitoClient.InitiateAuthAsync(loginRequest);

            if (loginResponse.AuthenticationResult == null)
            {
                _logger.LogWarning("Authentication failed: no result returned");
                return false;
            }

            return true;
        }
        catch (NotAuthorizedException)
        {
            _logger.LogWarning("Login failed: Incorrect username or password.");
            return false;
        }
        catch (UserNotConfirmedException)
        {
            _logger.LogWarning("Login failed: User not confirmed. Please verify your email.");
            return false;
        }
        catch (UserNotFoundException)
        {
            _logger.LogWarning("Login failed: User does not exist.");
            return false;
        }
        catch (AmazonCognitoIdentityProviderException ex)
        {
            _logger.LogWarning(ex, $"Login failed: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during login.");
            return false;
        }
    }

    public async Task LogoutAsync(string accessToken)
    {
        try
        {
            var LogoutRequest = new GlobalSignOutRequest
            {
                AccessToken = accessToken
            };

            await _cognitoClient.GlobalSignOutAsync(LogoutRequest);
        }
        catch (NotAuthorizedException ex)
        {
            _logger.LogWarning(ex, "Logout failed: Invalid access token or already signed out.");
        }
        catch (AmazonCognitoIdentityProviderException ex)
        {
            _logger.LogWarning(ex, $"Logout failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            var message = "An unexpected error occurred during logout.";
            _logger.LogWarning(ex, message);
            throw new Exception(message, ex);

        }
    }
}
