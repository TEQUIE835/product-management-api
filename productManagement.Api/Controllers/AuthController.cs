using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using productManagement.Application.DTOs.Auth;
using productManagement.Application.Interfaces.Auth;

namespace productManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema con rol asignado.
    /// </summary>

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            await _authService.RegisterAsync(request);
            return Ok("User registration successful");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Inicia sesión de usuario y genera un token JWT.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    ///<summary>
    /// Refresca el token de acceso para cuando este expira
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAccessToken([FromBody] string refreshToken)
    {
        try
        {
            var newToken = await _authService.RefreshAccessToken(refreshToken);
            return Ok(newToken);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    ///<summary>
    /// Elimina el refresh token de la base de datos para cerrar sesion, requiere autorizacion
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        try
        {
            var logout = await _authService.LogoustAsync(refreshToken);
            if(logout == null || logout == false) throw new Exception("Logout failed");
            return Ok("Logout successful");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}