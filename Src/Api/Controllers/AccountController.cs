using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("Account")]
public class AccountController : Controller
{
    [HttpGet, Route("Login")]
    public IActionResult Login()
    {
        return Ok("Wtf");
    }
}