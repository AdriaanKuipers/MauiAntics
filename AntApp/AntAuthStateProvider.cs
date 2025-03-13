using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using AntCore.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace AntApp;

public class AntAuthStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal currentUser = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() =>
        await Task.FromResult(new AuthenticationState(currentUser));

    public Task LogInAsync()
    {
        var loginTask = LogInAsyncCore();
        NotifyAuthenticationStateChanged(loginTask);

        return loginTask;

        async Task<AuthenticationState> LogInAsyncCore()
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity());

            // Get access token
            var authResult = await WebAuthenticator.Default.AuthenticateAsync(new("https://10.0.2.2:5001/auth/mobilelogin"), new("ant://"));

            // Get user info from auth/me

            // DEBUG - Ignore certificate errors
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert != null && cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };

            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authResult.AccessToken}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var user = await client.GetFromJsonAsync<AntUser>("https://10.0.2.2:5001/auth/me");

            // Build Identity from *me*
            List<Claim> claims = [new(ClaimTypes.Name, user.Name)];
            claims.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x)));
            authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "ant"));

            currentUser = authenticatedUser;

            return new AuthenticationState(currentUser);
        }
    }

    public void LogOut()
    {
        currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        SecureStorage.Default.Remove("token");
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(currentUser)));
    }
}