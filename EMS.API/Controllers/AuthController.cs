using Microsoft.AspNetCore.Mvc;
using Services.Auth;
using Shared.DTOs;

namespace EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthenticationService authenticationService) : ControllerBase
{
    private readonly AuthenticationService authenticationService = authenticationService;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel loginRequest)
    {
        string? token = await authenticationService.TryGenerateToken(loginRequest);

        if (token is null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegistrationViewModel registerRequest)
    {
        if (await authenticationService.IsAlreadyRegistered(registerRequest.Email))
        {
            return BadRequest("Email is already taken.");
        }

        EmployerViewModel employer = await authenticationService.RegistrateEmployer(registerRequest);

        return CreatedAtAction(nameof(Register), employer);
    }
}
