using System.Security.Claims;

namespace ContactServiceApp.Identity;

public static class ClaimsPrincipalMapper
{
    public static ApplicationUser ToApplicationUser(this ClaimsPrincipal principal)
    {
        var user = new ApplicationUser();
        
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            throw new ArgumentNullException("user.Id","User Id is null");
        }

        user.Id = userId;
        user.UserName = principal.FindFirstValue(ClaimTypes.Name);
        user.Email = principal.FindFirstValue(ClaimTypes.Email);
        user.EmailConfirmed = true;
        return user;
    }
}