using ApiCredit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
#if !DEBUG
using Microsoft.AspNetCore.Authorization;
#endif

namespace ApiCredit.Controllers
{
    [ApiController]
#if !DEBUG
    [Authorize]
#endif
    /// <summary>
    /// Controller responsible for handling the Digital Key integration callback.
    /// </summary>
    [Route("digital-key-integration")]
    public class DigitalKeyIntegrationController : ControllerBase
    {
        /// <summary>
        /// Receives user data from the Digital Key system (mocked).
        /// </summary>
        /// <param name="user">User data retrieved from the Digital Key service.</param>
        /// <returns>Returns the received user object for confirmation.</returns>
        [HttpPost]
        [ProducesResponseType<User>(StatusCodes.Status200OK)]
        [ProducesResponseType<object>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<object>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<object>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveUserCallback([FromBody] User user)
        {
            return Ok(user);
        }
    }

}
