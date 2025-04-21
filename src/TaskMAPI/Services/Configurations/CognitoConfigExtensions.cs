using Amazon.CognitoIdentityProvider;
using Amazon.Runtime;

namespace TaskMAPI.Services;

public static class CognitoConfigExtensions
{
    public static void CognitoServices(this IServiceCollection services, IConfiguration config)
    {
        var awsOptions = new AmazonCognitoIdentityProviderConfig
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(config["AWS:Region"])
        };

        var credentials = new BasicAWSCredentials(config["AWS:AccessKey"], config["AWS:SecretKey"]);
        services.AddSingleton<IAmazonCognitoIdentityProvider>(new AmazonCognitoIdentityProviderClient(credentials, awsOptions));

        services.AddScoped<IAuthenticationService, CognitoAuthService>();
    }
}
