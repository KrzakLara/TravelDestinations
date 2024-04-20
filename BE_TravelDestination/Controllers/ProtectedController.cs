using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_TravelDestination.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProtectedController : ControllerBase
    {
        // This action requires a valid JWT access token to be accessed.
        [HttpGet("data")]
        [Authorize]
        public IActionResult GetProtectedData()
        {
            // You can use User.Claims to access user-specific information if needed.
            return Ok(new { message = "This is protected data." });
        }
    }
}
