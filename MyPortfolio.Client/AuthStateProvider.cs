using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

//     Provides information about the authentication state of the current user.
public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public AuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

//to get the authentication state of the user, we check if there is a token in local storage. If there is no token, we return an empty ClaimsPrincipal, which means the user is not authenticated. If there is a token, we read it and create a ClaimsPrincipal based on the claims in the token. Finally, we return an AuthenticationState with the ClaimsPrincipal.
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("token");

        if (string.IsNullOrEmpty(token))
        {
             var anonymous = new ClaimsPrincipal(new ClaimsIdentity()); // بدون type
             return new AuthenticationState(anonymous);
        }

        var handler = new JwtSecurityTokenHandler();//to deal with tokens
        var jwt = handler.ReadJwtToken(token);//read token

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");//read claim from token

        var user = new ClaimsPrincipal(identity);//user information

        return new AuthenticationState(user);
    }

    //to notify the application that the authentication state has changed, we call the NotifyAuthenticationStateChanged method and pass it the result of GetAuthenticationStateAsync. This will trigger any components that are subscribed to the AuthenticationStateChanged event to re-render and update their UI based on the new authentication state.
    public void NotifyUserAuthentication()
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "admin"),
            new Claim(ClaimTypes.Role, "Admin")
        }, "jwt"));

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
    }

    public void NotifyUserLogout()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
}