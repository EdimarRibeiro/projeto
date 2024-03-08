using Microsoft.AspNetCore.Mvc;
using ProjectManager.Db.Context;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Web.Controllers
{
    public static class ControllerExtentions
    {
        public static string ObterContaDoUsuarioCorrente(this Controller controller)
        {
            var conta = controller.User.FindFirst(x => x.Type == "conta")?.Value ?? $"{Guid.Empty}.{Guid.Empty}";
            return conta;
        }

        public static Guid UsuarioIdUsuarioCorrente(this Controller controller)
        {
            String[] contas = ObterContaDoUsuarioCorrente(controller).Split(".");

            return Guid.Parse(contas[1]);
        }

        public static Guid EmpresaIdInternoCorrente(this Controller controller)
        {
            String[] contas = ObterContaDoUsuarioCorrente(controller).Split(".");

            return Guid.Parse(contas[2]);
        }

        public static void AtribuirContaDoUsuarioCorrente(this Controller controller, IComConta comConta)
        {
            var contaId = controller.ObterContaDoUsuarioCorrente();

            comConta.ContaId = contaId;
        }

        public static Usuario ObterDadosDoUsuarioCorrente(this Controller controller, DbProjectManagerContext db)
        {
            var contaDoUsuario = controller.ObterContaDoUsuarioCorrente();
            var nomeDoUsuario = controller.User.Identity.Name;

            int x = 0;
            Guid UsuarioId = Guid.Empty; 

            String[] chaves = contaDoUsuario.Split('.');

            foreach (var chave in chaves)
            {
                switch (x)
                {
                    case 1: UsuarioId = Guid.Parse(chave); break;
                }
                x++;
            }

            Usuario usuario = db.Usuario
                .Where(a => a.Login == nomeDoUsuario && a.IdExterno == UsuarioId).FirstOrDefault();

            if (usuario == null)
            {
                throw new Exception("Algum problema grave aconteceu, falha na autenticação do token da conexão corrente.");
            }

            return usuario;
        }
    }

    public interface IComConta
    {
        string ContaId { get; set; }
    }
}
