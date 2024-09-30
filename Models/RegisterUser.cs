namespace InjectionExt.Models;

public record RegisterUser(
    string FirstName,
    string LastName,
    string SocialSecurityNumber,
    string UserName,
    string Password);
