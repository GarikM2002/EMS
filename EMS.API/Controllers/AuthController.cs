using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Auth;
using Services.Employees;
using Shared.DTOs;

namespace EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthenticationService authenticationService,
	IEmployeeService employeeService) : ControllerBase
{
	private readonly AuthenticationService authenticationService = authenticationService;
	private readonly IEmployeeService employeeService = employeeService;

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

	[HttpGet, Authorize]
	public IActionResult IsAuthenticated()
	{
		return Ok();
	}

	[HttpGet("GetUserData"), Authorize]
	public IActionResult GetUserDataFromJWT()
	{
		var user = new JWTUserData();
		{
			user.Id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
			user.UserName = User.FindFirstValue(ClaimTypes.Name);
			user.Email = User.FindFirstValue(ClaimTypes.Email);
		};

		return Ok(user);
	}
}