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