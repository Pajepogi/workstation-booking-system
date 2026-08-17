using Microsoft.AspNetCore.Mvc;
using wbs_user_api.Models;
using wbs_user_api.Repositories.Interfaces;

namespace wbs_user_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userRepository.GetAllAsync();

        return Ok(users);
    }

    /// <summary>
    /// Get user by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound(new
            {
                Message = $"User with ID {id} not found."
            });
        }

        return Ok(user);
    }

    /// <summary>
    /// Create user
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        user.CreatedDate = DateTime.UtcNow;
        user.UpdatedDate = DateTime.UtcNow;

        var createdUser = await _userRepository.CreateAsync(user);

        return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
    }

    /// <summary>
    /// Update user
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] User request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound(new
            {
                Message = $"User with ID {id} not found."
            });
        }

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.PasswordHash = request.PasswordHash;
        user.IsActive = request.IsActive;
        user.UpdatedDate = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return Ok(new
        {
            Message = "User updated successfully."
        });
    }

    /// <summary>
    /// Delete user
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound(new
            {
                Message = $"User with ID {id} not found."
            });
        }

        await _userRepository.DeleteAsync(user);

        return Ok(new
        {
            Message = "User deleted successfully."
        });
    }
}
