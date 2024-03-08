using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ProjectManager.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class HomeController : Controller
    {
        //// GET: api/Home/Versao
        [HttpGet("Versao")]
        public IActionResult GetVersao()
        {
            return Ok(new { versao = Assembly.GetEntryAssembly().GetName().Version.ToString() });
        }

    }
}
