using ProjectManager.Business.Interfaces.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Utils.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ProjectManager.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class SimNaoController : Controller
    {
        private ISimNaoBusiness _modelBusiness;

        public SimNaoController(ISimNaoBusiness modelBusiness)
        {
            _modelBusiness = modelBusiness;
        }

        //// GET: api/SimNao/grid
        [HttpGet]
        [Route("grid")]
        public IActionResult GetSimNaoGrid([FromQuery] int pagina, string pesquisa, int linhas)
        {
            return Ok(_modelBusiness.ObterTodos(new Pagination { Page = pagina, PageSize = linhas }, pesquisa));
        }

        //// GET: api/SimNao
        [HttpGet]
        public async Task<IActionResult> GetSimNao()
        {
            return Ok(await _modelBusiness.ObterTodos(""));
        }

        //// GET: api/SimNao/5/5/5
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSimNao([FromRoute] Int16 Id)
        {
            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);

            if (obj == null)
                return NotFound();

            return Ok(obj);
        }

        //// PUT: api/SimNao
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutSimNao([FromRoute] Int16 Id, [FromBody] SimNao model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);
            if (obj == null)
                return NotFound();

            await _modelBusiness.Atualizar(model);

            return Ok(model);
        }

        //// POST: api/SimNao
        [HttpPost]
        public async Task<IActionResult> PostSimNao([FromBody] SimNao model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _modelBusiness.Cadastrar(model);

            return CreatedAtAction("GetSimNao", new { Id = model.Id }, model);
        }

        // DELETE: api/SimNao/5/5/5
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteSimNao([FromRoute] Int16 Id)
        {
            var obj = await _modelBusiness.ObterPorChave(p => p.Id == Id);
            if (obj == null)
                return NotFound();

            await _modelBusiness.Excluir(obj);

            return Ok();
        }
    }
}

