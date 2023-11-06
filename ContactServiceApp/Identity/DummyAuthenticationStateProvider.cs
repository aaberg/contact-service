using Microsoft.AspNetCore.Components.Authorization;

namespace ContactServiceApp.Identity;

public class DummyAuthenticationStateProvider : AuthenticationStateProvider
{
    private AuthenticationState _authenticationState;
    
    public DummyAuthenticationStateProvider(AuthenticationService authenticationService)
    {
        _authenticationState = new AuthenticationState(authenticationService.CurrentUser);
        authenticationService.UserChanged += (newUser) =>
        {
            _authenticationState = new AuthenticationState(newUser);
            NotifyAuthenticationStateChanged(Task.FromResult(_authenticationState));
        };
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(_authenticationState);
    }
}