using BE_TravelDestination.DataPart;
using BE_TravelDestinations.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AuthController(JwtTokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginDto user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var storedUser = _userRepository.FindByUsername(user.Username);
        if (storedUser == null)
        {
            return Unauthorized("User does not exist.");
        }

        bool validPassword = BCrypt.Net.BCrypt.Verify(user.Password, storedUser.HashedPassword);
        if (!validPassword)
        {
            return Unauthorized("Invalid credentials.");
        }

        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, storedUser.Username)
        };

        var accessToken = _tokenService.GenerateAccessToken(claims);
        return Ok(new { AccessToken = accessToken });
    }
}
