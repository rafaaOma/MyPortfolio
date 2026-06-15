using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

//     Provides information about the authentication state of the current user.
public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage; //local storage will gonna be uesed to store jwt

    public AuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

//to get the authentication state of the user, we check if there is a token in local storage. If there is no token, we return an empty ClaimsPrincipal, which means the user is not authenticated. If there is a token, we read it and create a ClaimsPrincipal based on the claims in the token. Finally, we return an AuthenticationState with the ClaimsPrincipal.
public override async Task<AuthenticationState> GetAuthenticationStateAsync()
{
    Console.WriteLine("AuthStateProvider CALLED");//testing purpses

    try
    {
        var token = await _localStorage.GetItemAsync<string>("token");//get token stored in local storage

        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity())
            );
        }

        var handler = new JwtSecurityTokenHandler();//to handel with tokens
        var jwt = handler.ReadJwtToken(token);

        var claims = jwt.Claims.ToList();//extract the claims

        // extract roles
        var roleClaims = claims
            .Where(c => c.Type == "role" || c.Type == "roles")
            .ToList();

        foreach (var role in roleClaims)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Value));
            Console.WriteLine($"Added role claim: {role.Value}");
        }

        var identity = new ClaimsIdentity( //creating idintity 
            claims,
            authenticationType: "jwt",
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role
        );

        Console.WriteLine($"Is Authenticated: {identity.IsAuthenticated}");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");

        return new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity())
        );
    }
}
    //to notify the application that the authentication state has changed, we call the NotifyAuthenticationStateChanged method and pass it the result of GetAuthenticationStateAsync. This will trigger any components that are subscribed to the AuthenticationStateChanged event to re-render and update their UI based on the new authentication state.
    public async Task NotifyUserAuthentication()
{
        var authState = await GetAuthenticationStateAsync();
    NotifyAuthenticationStateChanged(Task.FromResult(authState));
}
    public void NotifyUserLogout()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
}