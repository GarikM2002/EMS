using System.Security.Cryptography;
using System.Text;
using DataAccess.Enities;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using EMS.API.Models;
using EMS.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IEmployerRepository employerRepository,
    JwtTokenService jwtTokenService) : ControllerBase
{
    private readonly IEmployerRepository employerRepository = employerRepository;
    private readonly JwtTokenService jwtTokenService = jwtTokenService;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel loginRequest)
    {
        Employer? employer = await employerRepository.GetEmployerByEmailAsync(loginRequest.Email);
        if (employer == null || !VerifyPasswordHash(loginRequest.Password,
            employer.PasswordHash, employer.PasswordSalt))
        {
            return Unauthorized();
        }

        var token = jwtTokenService.GenerateToken(employer);

        //StoreInHttpOnlyCookie(Response, token, 20);

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterViewModel registerRequest)
    {
        if (await employerRepository.GetEmployerByEmailAsync(registerRequest.Email) != null)
        {
            return BadRequest("Email is already taken.");
        }

        var (hash, salt) = CreatePasswordHash(registerRequest.Password);

        var employer = new Employer
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Email = registerRequest.Email,
            PhoneNumber = registerRequest.PhoneNumber,
            Department = registerRequest.Department,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        var employerId = await employerRepository.CreateEmployerAsync(employer);
        employer.Id = employerId;

        return CreatedAtAction(nameof(Login), new { id = employerId }, employer);
    }

    private static (string hash, string salt) CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        
        var salt = Convert.ToBase64String(hmac.Key);
        var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.ASCII.GetBytes(password)));
        return (hash, salt);
    }

    private static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
    {
        using var hmac = new HMACSHA512(Convert.FromBase64String(storedSalt));
        
        var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.ASCII.GetBytes(password)));
        return computedHash == storedHash;
    }

    private static void StoreInHttpOnlyCookie(HttpResponse response, string token, int expirationTime)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddMinutes(expirationTime), // Adjust expiration time as needed
        };

        response.Cookies.Append("jwt_token", token, cookieOptions);
    }
}
