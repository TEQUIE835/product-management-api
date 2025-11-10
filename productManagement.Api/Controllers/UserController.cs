using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using productManagement.Application.Interfaces.Users;
using productManagement.Domain.Entities;

namespace productManagement.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Obtiene la lista de todos los usuarios registrados (solo para administradores).
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    
    /// <summary>
    /// Obtiene la información de un usuario específico por su ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    
    /// <summary>
    /// Actualiza los datos de un usuario existente.
    /// </summary>
    [HttpPut("id")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] User user)
    {
        try
        {
            await _userService.UpdateAsync(id, user);
            return Ok("user updated");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Elimina un usuario del sistema (solo para administradores).
    /// </summary>
    [HttpDelete("id")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            await _userService.DeleteAsync(id);
            return Ok("user deleted");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}