using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.Db.Context;
using ProjectManager.Domain.Entities;
using ProjectManager.Web.Models.Authenticacao;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace ProjectManager.Web.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly DbProjectManagerContext _db;
        private readonly TokenConfigurations _tokenConfigurations;

        public LoginController(DbProjectManagerContext db, TokenConfigurations tokenConfigurations)
        {
            _db = db;
            _tokenConfigurations = tokenConfigurations;
        }

        [AllowAnonymous]
        [HttpPost("verificar")]
        public object PostVerificar([FromBody] UsuarioSenha usuario)
        {
            ResultadoAutenticacao autent = new ResultadoAutenticacao();
            autent.Message = "Usuário com conta registrada !";
            autent.Authenticated = true;

            Usuario usuarioBanco = _db.Usuario
                .Where(b => b.Login == usuario.UserName).FirstOrDefault();

            if (usuarioBanco == null)
            {
                autent.Authenticated = false;
                autent.Message = "Usuário sem conta registrada !";
            }

            return Ok(autent);
        }

        [AllowAnonymous]
        [HttpPost]
        public object Post([FromBody] UsuarioSenha usuario)
        {
            var usuarioNaoConfere = new ResultadoAutenticacao
            {
                Message = "Usuário ou Senha não confere !"
            };

            var usuarioDeviceInvalido = new ResultadoAutenticacao
            {
                Message = "Device não está ativo !"
            };

            var usuarioSenhaNaoInformado = string.IsNullOrWhiteSpace(usuario?.UserName) ||
                                             string.IsNullOrWhiteSpace(usuario.Password);

            if (usuarioSenhaNaoInformado)
            {
                return Ok(usuarioNaoConfere);
            }

            Usuario usuarioBanco = _db.Usuario
                .Where(b => b.Login == usuario.UserName && b.AtivoId == 1 && b.ExcluidoId != 1)
                .FirstOrDefault();

            var usuarioEncontrado = usuarioBanco == null;

            if (usuarioEncontrado || !BCrypt.Net.BCrypt.Verify(usuario.Password, usuarioBanco.Senha))
            {
                return Ok(usuarioNaoConfere);
            }

            return ObterToken(usuarioBanco);
        }

        [Authorize]
        [HttpGet("refresh")]
        public object Refresh()
        {
            var usuario = this.ObterDadosDoUsuarioCorrente(_db);

            return ObterToken(usuario);
        }

        private object ObterToken(Usuario usuarioBanco)
        {
            var dataCriacao = DateTime.Now;
            bool emailVerifica = false;
            var dataExpiracao = dataCriacao.AddMinutes(_tokenConfigurations.TokenLifetimeInMinutes);

            if (usuarioBanco.EmailVerificado != 1)
                emailVerifica = true;

            var identity = new ClaimsIdentity(
                new GenericIdentity(usuarioBanco.Login, "Login"),
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti,        Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.Iss,        _tokenConfigurations.Issuer),
                    new Claim(JwtRegisteredClaimNames.Aud,        _tokenConfigurations.Audience),
                    new Claim(JwtRegisteredClaimNames.Exp,        dataExpiracao.Ticks.ToString()),
                    new Claim("name",                             $"{usuarioBanco.Nome}"),
                    new Claim(JwtRegisteredClaimNames.GivenName,  usuarioBanco.Nome),
                    new Claim(JwtRegisteredClaimNames.FamilyName, ""),
                    new Claim(JwtRegisteredClaimNames.Email,      usuarioBanco.Login),
                    new Claim("email_verified",                   emailVerifica.ToString()),
                    new Claim("picture",                          ""),
                    new Claim("conta",                            "0"+"."+usuarioBanco.IdExterno.ToString()+". "),
                    new Claim("phone_number",                     usuarioBanco?.Celular ?? "")
                }
            );

            var handler = new JwtSecurityTokenHandler();
            var siginCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfigurations.SymmetricSecurityKey)),
                    SecurityAlgorithms.HmacSha256);

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = string.Empty,
                Audience = string.Empty,
                SigningCredentials = siginCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });

            var token = handler.WriteToken(securityToken);
            usuarioBanco.Senha = null;

            return new ResultadoAutenticacao
            {
                Authenticated = true,
                Created = dataCriacao,
                Expiration = dataExpiracao,
                AccessToken = token,
                Message = "OK",
                Usuario = usuarioBanco
            };
        }
        public class UsuarioSenha
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Imei { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public string Versao { get; set; }
            public string VersaoApp { get; set; }
            public string Start { get; set; }
        }

        public class ResultadoAutenticacao
        {
            public bool Authenticated { get; set; }
            public DateTime Created { get; set; }
            public DateTime Expiration { get; set; }
            public string AccessToken { get; set; }
            public string Message { get; set; }
            public Usuario Usuario { get; set; }
        }
    }
}
