using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Business.Interfaces.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Utils.Expressions;

namespace ProjectManager.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuarioController : Controller
    {
        private IUsuarioBusiness _modelBusiness;

        public UsuarioController(IUsuarioBusiness modelBusiness)
        {
            _modelBusiness = modelBusiness;
        }

        //// GET: api/Usuario/grid
        [HttpGet]
        [Route("grid")]
        public IActionResult GetUsuarioGrid([FromQuery] int pagina, string pesquisa, int linhas)
        {
            return Ok(_modelBusiness.ObterTodos(new Pagination { Page = pagina, PageSize = linhas }, pesquisa));
        }

        //// GET: api/Usuario
        [HttpGet("")]
        public async Task<IActionResult> GetUsuario()
        {
            return Ok(await _modelBusiness.ObterTodos(""));
        }

        //// GET: api/Usuario/5/5/5
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUsuarioId([FromRoute] decimal Id)
        {
            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);

            if (obj == null)
                return NotFound();

            return Ok(obj);
        }

        //// PUT: api/Usuario
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutUsuario([FromRoute] decimal Id, [FromBody] Usuario model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);
            if (obj == null)
                return NotFound();

            if ((!model.Senha.Equals("")) && (model.Senha.Length < 25))
                model.Senha = BCrypt.Net.BCrypt.HashPassword(model.Senha);

            await _modelBusiness.Atualizar(model);

            return Ok(model);
        }

        //// POST: api/Usuario
        [HttpPost]
        public async Task<IActionResult> PostUsuario([FromBody] Usuario model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.Senha = BCrypt.Net.BCrypt.HashPassword(model.Senha);
            await _modelBusiness.Cadastrar(model);

            return CreatedAtAction("GetUsuario", new { Id = model.Id }, model);
        }

        // DELETE: api/Usuario/5/5/5
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUsuario([FromRoute] decimal Id)
        {
            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);
            if (obj == null)
                return NotFound();

            await _modelBusiness.Excluir(obj);

            return Ok();
        }

    }
}

