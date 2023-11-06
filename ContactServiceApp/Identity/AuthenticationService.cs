using System.Security.Claims;

namespace ContactServiceApp.Identity;

public class AuthenticationService
{
    public event Action<ClaimsPrincipal>? UserChanged;
    private ClaimsPrincipal? _currentUser;

    public ClaimsPrincipal CurrentUser
    {
        get => _currentUser ?? new();
        set
        {
            _currentUser = value;
            UserChanged?.Invoke(_currentUser);
        }
    }
}