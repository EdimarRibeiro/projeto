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
    public class ProjetoController : Controller
    {
        private IProjetoBusiness _modelBusiness;
        private IResponsavelBusiness _modelRespBusiness;

        public ProjetoController(IProjetoBusiness modelBusiness, IResponsavelBusiness modelRespBusiness)
        {
            _modelBusiness = modelBusiness;
            _modelRespBusiness = modelRespBusiness;
        }

        // GET: api/Projeto/grid
        [HttpGet]
        [Route("grid")]
        public IActionResult GetProjetoGrid([FromQuery] int pagina, string pesquisa, int linhas)
        {
            return Ok(_modelBusiness.ObterTodos(new Pagination { Page = pagina, PageSize = linhas }, pesquisa, includes: i=> i.Include(a => a.Responsavel)) );
        }

        // GET: api/Projeto
        [HttpGet]
        public async Task<IActionResult> GetProjeto()
        {
            return Ok(await _modelBusiness.ObterTodos(""));
        }

        // GET: api/Projeto/responsavel/auto
        [HttpGet("responsavel/auto/{query}")]
        public async Task<IActionResult> GetResponsanvelAuto([FromRoute] string query)
        {
            var obj = await _modelRespBusiness.ObterTodos("", a=> a.Nome.Contains(query) || a.Sobrenome.Contains(query));

            if (obj == null || obj.Count() == 0)
                return NotFound();

            return Ok(obj);
        }

        // GET: api/Projeto/5
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProjetoId([FromRoute] decimal Id)
        {
            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id, includes: i => i.Include(a => a.Responsavel));

            if (obj == null)
                return NotFound();

            return Ok(obj);
        }

        // PUT: api/Projeto
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutProjeto([FromRoute] decimal Id, [FromBody] Projeto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);
            if (obj == null)
                return NotFound();

            await _modelBusiness.Atualizar(model);

            return Ok(model);
        }

        // GET: api/Projeto/5
        [HttpGet("status/{Id}")]
        public async Task<IActionResult> GetStatusProjetoId([FromRoute] decimal Id)
        {
            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);

            if (obj == null)
                return NotFound();

            if (obj.Status != ProjetoStatus.Cancelado && obj.Status != ProjetoStatus.Encerrado)
                ++obj.Status;

            if (obj.Status == ProjetoStatus.Encerrado)
                obj.DataTermino = DateTime.Now.ToUniversalTime();

            if (obj.DataTermino == DateTime.MinValue.AddYears(3)) obj.DataTermino = null;
            if (obj.DataCancelado == DateTime.MinValue.AddYears(3)) obj.DataCancelado = null;

            await _modelBusiness.Atualizar(obj);
            return Ok(obj);
        }

        // GET: api/Projeto/5
        [HttpGet("cancelar/{Id}")]
        public async Task<IActionResult> GetCancelarProjetoId([FromRoute] decimal Id)
        {
            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);

            if (obj == null)
                return NotFound();

            obj.Status = ProjetoStatus.Cancelado;
            obj.DataCancelado = DateTime.Now.ToUniversalTime();

            if (obj.DataTermino == DateTime.MinValue.AddYears(3)) obj.DataTermino = null;
            if (obj.DataCancelado == DateTime.MinValue.AddYears(3)) obj.DataCancelado = null;

            await _modelBusiness.Atualizar(obj);

            return Ok(obj);
        }

        // POST: api/Projeto
        [HttpPost]
        public async Task<IActionResult> PostProjeto([FromBody] Projeto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.ResponsavelId = await GetResponsavelAsync();

            await _modelBusiness.Cadastrar(model);

            return CreatedAtAction("GetProjeto", new { model.Id }, model);
        }

        // GET: api/Projeto/5
        [HttpGet("responsavel")]
        public async Task<IActionResult> GetResponsavel()
        {
            var responsavel = await GenereteResponsavel();

            if (responsavel == null)
                return NotFound();

            return Ok(responsavel);
        }

        private async Task<decimal> GetResponsavelAsync()
        {
            var responsavel = await GenereteResponsavel();

            return responsavel.Id;
        }

        private async Task<Responsavel> GenereteResponsavel()
        {
            Responsavel responsavel = new Responsavel();
            var resp = new ConsultaResponsavel();
            var user = resp.Consultar().Result.results.FirstOrDefault();

            if (user != null)
            {
                responsavel = await _modelRespBusiness.ObterPorChave(x => x.Codigo == user.login.uuid);
            }

            if (responsavel == null && user != null)
            {
                responsavel = new Responsavel()
                {
                    Codigo = user.login.uuid,
                    IdExterno = Guid.Empty,
                    Email = user.email,
                    Nome = user.name.first,
                    Sobrenome = user.name.last,
                    AtivoId = 1,
                    ExcluidoId = 0,
                };

                await _modelRespBusiness.Cadastrar(responsavel);

                return responsavel;
            }
            else if (user == null)
            {
                throw new Exception("Falha ao obter responsável.");
            }

            return responsavel;
        }

        // DELETE: api/Projeto/5
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProjeto([FromRoute] decimal Id)
        {
            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);
            if (obj == null)
                return NotFound();

            await _modelBusiness.Excluir(obj);

            return Ok();
        }
    }
}

