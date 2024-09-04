using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController(SignInManager<AppUser> signInManager) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        // New user object
        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email,
        };

        // New user is created
        var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

        // If the user wasn't created successfully, return a bad request with the errors
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        // Logouts the user and destroys the login cookie
        await signInManager.SignOutAsync();

        return NoContent();
    }

    [HttpGet("user-info")]
    // Gets user info
    public async Task<ActionResult> GetUserInfo()
    {
        // Checks if the user is authenticated
        if (User.Identity?.IsAuthenticated == false)
            return NoContent();

        // Finds the user by email
        var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

        // Else return user data
        return Ok(
            new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                Address = user.Address?.ToDto(),
            }
        );
    }

    [HttpGet("auth-status")]
    public ActionResult GetAuthState()
    {
        return Ok(new { IsAuthenticated = User.Identity?.IsAuthenticated ?? false });
    }

    [Authorize]
    [HttpPost("address")]
    public async Task<ActionResult<Address>> CreateOrUpdateAddress(AddressDto addressDto)
    {
        // Finds the user with address by email
        var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

        // If user doesn't have an address, create one
        if (user.Address == null)
        {
            user.Address = addressDto.ToEntity();
        }
        // Else update it
        else
        {
            user.Address.UpdateFromDto(addressDto);
        }

        // Update the user entity
        var result = await signInManager.UserManager.UpdateAsync(user);

        // Throw error if something bad happends
        if (!result.Succeeded)
            return BadRequest("Problem updating user address");

        // Return address
        return Ok(user.Address.ToDto());
    }
}
