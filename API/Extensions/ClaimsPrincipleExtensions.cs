using System.Security.Authentication;
using System.Security.Claims;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ClaimsPrincipleExtensions
{
    // Gets a user by its email, and this methods extends the UserManager<AppUser> class
    public static async Task<AppUser> GetUserByEmail(
        this UserManager<AppUser> userManager,
        ClaimsPrincipal user
    )
    {
        var userToReturn =
            await userManager.Users.FirstOrDefaultAsync(x => x.Email == user.GetEmail())
            ?? throw new AuthenticationException("User not found");

        return userToReturn;
    }

    // Gets a user by its email and with its address, and this methods extends the UserManager<AppUser> class
    public static async Task<AppUser> GetUserByEmailWithAddress(
        this UserManager<AppUser> userManager,
        ClaimsPrincipal user
    )
    {
        var userToReturn =
            await userManager
                .Users.Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Email == user.GetEmail())
            ?? throw new AuthenticationException("User not found");

        return userToReturn;
    }

    // Gets the email of a user, and this method extends the ClaimsPrincipal class
    public static string GetEmail(this ClaimsPrincipal user)
    {
        var email =
            user.FindFirstValue(ClaimTypes.Email)
            ?? throw new AuthenticationException("Email claim not found");

        return email;
    }
}
