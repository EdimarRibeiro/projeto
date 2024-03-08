using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Business.Interfaces.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Utils.Expressions;
using ProjectManager.Web.Rotinas;

namespace ProjectManager.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjetoResponsavelController : Controller
    {
        private IProjetoResponsavelBusiness _modelBusiness;
        private IResponsavelBusiness _modelResponsavelBusiness;

        public ProjetoResponsavelController(IProjetoResponsavelBusiness modelBusiness, IResponsavelBusiness modelResponsavelBusiness)
        {
            _modelBusiness = modelBusiness;
            _modelResponsavelBusiness = modelResponsavelBusiness;
        }

        // GET: api/ProjetoResponsavel/grid
        [HttpGet]
        [Route("grid")]
        public IActionResult GetProjetoResponsavelGrid([FromQuery] int pagina, string pesquisa, int linhas)
        {
            return Ok(_modelBusiness.ObterTodos(new Pagination { Page = pagina, PageSize = linhas }, pesquisa) );
        }

        // GET: api/ProjetoResponsavel
        [HttpGet("all/{projetoId}")]
        public async Task<IActionResult> GetProjetoResponsavelAll([FromRoute] decimal projetoId)
        {
            return Ok(await _modelBusiness.ObterTodos("", a=> a.ProjetoId == projetoId, includes: i => i.Include(a => a.Responsavel)));
        }

        // GET: api/ProjetoResponsavel/5
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProjetoResponsavel([FromRoute] decimal Id)
        {
            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);

            if (obj == null)
                return NotFound();

            return Ok(obj);
        }

        // PUT: api/ProjetoResponsavel
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutProjetoResponsavel([FromRoute] decimal Id, [FromBody] ProjetoResponsavel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);
            if (obj == null)
                return NotFound();

            await _modelBusiness.Atualizar(model);

            return Ok(model);
        }

        // POST: api/ProjetoResponsavel
        [HttpPost]
        public async Task<IActionResult> PostProjetoResponsavel([FromBody] ProjetoResponsavel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.ResponsavelId == null ) model.ResponsavelId = await GetResponsavelAsync();

            await _modelBusiness.Cadastrar(model);

            return CreatedAtAction("GetProjetoResponsavel", new { model.Id }, model);
        }

        private async Task<decimal> GetResponsavelAsync()
        {
            Responsavel responsavel = null;

            var resp = new ConsultaResponsavel();
            var user = resp.Consultar().Result.results.FirstOrDefault();

            if (user != null)
            {
                responsavel = await _modelResponsavelBusiness.ObterPorChave(x=> x.Codigo == user.login.uuid.ToString());
            }

            if (responsavel == null && user != null) {
                responsavel = new Responsavel() {
                    Codigo = user.login.uuid.ToString(),
                    IdExterno = Guid.Empty,
                    Email = user.email,
                    Nome = user.name.first,
                    Sobrenome = user.name.last,
                    AtivoId = 1,
                    ExcluidoId = 0,
                };

                await _modelResponsavelBusiness.Cadastrar(responsavel);

                return responsavel.Id;
            } else if (user == null)
            {
                throw new Exception("Falha ao obter responsável.");
            }

            return responsavel.Id;
        }

        // DELETE: api/ProjetoResponsavel/5
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProjetoResponsavel([FromRoute] decimal Id)
        {
            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);
            if (obj == null)
                return NotFound();

            await _modelBusiness.Excluir(obj);

            return Ok();
        }
    }
}

