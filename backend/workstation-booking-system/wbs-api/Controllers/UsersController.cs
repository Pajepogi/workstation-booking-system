using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using wbs_api.DTOs;
using wbs_api.Models;
using wbs_api.Repositories.Interfaces;
using wbs_api.Services;

namespace wbs_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordService _passwordService;

    public UsersController(IUserRepository userRepository, PasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound(new { Message = $"User with ID {id} not found." });
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        user.PasswordHash = _passwordService.HashPassword(user, user.PasswordHash);
        user.CreatedDate = DateTime.UtcNow;
        user.UpdatedDate = DateTime.UtcNow;

        var createdUser = await _userRepository.CreateAsync(user);

        return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] User request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound(new { Message = $"User with ID {id} not found." });
        }

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.PasswordHash = request.PasswordHash;
        user.IsActive = request.IsActive;
        user.UpdatedDate = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(request.PasswordHash))
        {
            user.PasswordHash = _passwordService.HashPassword(user, request.PasswordHash);
        }

        await _userRepository.UpdateAsync(user);

        return Ok(new { Message = "User updated successfully." });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound(new { Message = $"User with ID {id} not found." });
        }

        await _userRepository.DeleteAsync(user);

        return Ok(new { Message = "User deleted successfully." });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
    [FromBody] LoginRequestDTO request)
    {
        var user = await _userRepository.GetByUserId(request.EmployeeNumber);

        if (user == null)
        {
            return Unauthorized(new
            {
                Message = "Invalid credentials."
            });
        }

        var isValid = _passwordService.VerifyPassword(
            user,
            request.Password,
            user.PasswordHash);

        if (!isValid)
        {
            return Unauthorized(new
            {
                Message = "Invalid credentials."
            });
        }

        return Ok(new
        {
            EmployeeNumber =  user.EmployeeNumber,
            FullName = user.FullName,
            Role = user.Role,
            Email = user.Email,
            DepartmentName = user.DepartmentName,
            IsAuthenticated = true,
        });
    }
}
