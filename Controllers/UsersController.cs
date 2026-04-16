using Microsoft.AspNetCore.Mvc;
using UserApiTest.Services;
using UserApiTest.Models;   

namespace UserApiTest.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var user = await _userService.GetUserById(id);
        if (user is null)
        {
            return Problem(
                title: "Not Found",
                detail: $"User with id {id} was not found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(user);
    }

    [HttpGet("by-email")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] GetUserByEmailQuery query)
    {
        var user = await _userService.GetUserByEmail(query.Email);

        if (user is null)
        {
            return Problem(
                title: "Not Found",
                detail: $"No user was found with email '{query.Email}'.",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
    {
        var newUser = await _userService.CreateUser(user);
        return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] CreateUserDto user)
    {
        var updatedUser = await _userService.UpdateUser(id, user);
        if (updatedUser is null)
        {
            return Problem(
                title: "Not Found",
                detail: $"User with id {id} was not found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(updatedUser);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        var wasDeleted = await _userService.DeleteUser(id);
        if (!wasDeleted)
        {
            return Problem(
                title: "Not Found",
                detail: $"User with id {id} was not found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        return NoContent();
    }

}