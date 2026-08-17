using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace wbs_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("current-user")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            UserName = User.Identity?.Name,
            IsAuthenticated = User.Identity?.IsAuthenticated
        });
    }
}