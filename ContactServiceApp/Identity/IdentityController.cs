using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactServiceApp.Identity;

[Route("/identity/account")]
public class IdentityController : ControllerBase
{
    private SignInManager<ApplicationUser> _signInManager;

    public IdentityController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }


    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        // let's test signing you in

        var user = new ApplicationUser()
        {
            Email = "john.doe@thefancydomain.com",
            UserName = "john.doe@thefancydomain.com",
            Id = "abc:12345",
            EmailConfirmed = true,
            
        };

        await _signInManager.SignInAsync(user, new AuthenticationProperties(), "Custom authenticaiton");

        return Redirect("/");
    }
    
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var isSignedIn = _signInManager.IsSignedIn(HttpContext.User);
        if (!isSignedIn)
        {
            return Ok("Not logged in");
        }
        else
        {
            return Ok($"signed in! Yey! Your name is {HttpContext.User.Identity?.Name ?? "Unknown"}");   
        }
    }
}