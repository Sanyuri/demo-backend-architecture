using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackendArchitecture.API.Controllers;

public class CsrfController : BaseController
{
    [HttpGet("token")]
    public IActionResult GetToken([FromServices] IAntiforgery antiforgery)
    {
            var tokens = antiforgery.GetAndStoreTokens(HttpContext);
            return Ok(new {token = tokens.RequestToken});
    }
}