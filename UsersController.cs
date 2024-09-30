using InjectionExt.Models;
using InjectionExt.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InjectionExt;

[Route("/api/users")]
public class UsersController(IUsersService usersService, IUserDetailsService userDetailsService)
    : Controller
{
    [HttpPost("register")]
    public async Task<Guid> Register([FromBody] RegisterUser userRegistration)
    {
        var userId = await usersService.Register(userRegistration.UserName, userRegistration.Password);
        await userDetailsService.Register(
            userRegistration.FirstName,
            userRegistration.LastName,
            userRegistration.SocialSecurityNumber,
            userId);

        return userId;
    }
    
    [HttpGet("getuser/{userId}")]
    public async Task<User> GetUserById(Guid userId)
    {
        return await usersService.GetUserById(
            userId);
    }
}