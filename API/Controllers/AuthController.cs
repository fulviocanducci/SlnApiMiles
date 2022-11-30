using API.Models;
using API.Repositories;
using API.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Token([FromServices] ILoginRepository loginRepository, [FromServices] ITokenService tokenService, Login login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    LoginValidationResult loginValidationResult = await loginRepository.IsValidateAsync(login);
                    if (loginValidationResult.Status)
                    {
                        LoginResult result = tokenService.GenerateToken(loginValidationResult);
                        return Ok(result);
                    }
                    return NotFound(LoginResult.Default);
                }
                return BadRequest(ModelState);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet()]
        [Authorize]
        public async Task<ActionResult> GetUser([FromServices] ILoginRepository loginRepository)
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = await loginRepository.GetUserAsync(User?.Identity?.Name);
                if (user != null)
                {
                    UserViewModel userViewModel = user;
                    return Ok(userViewModel);
                }
            }
            return NotFound();
        }
    }
}
