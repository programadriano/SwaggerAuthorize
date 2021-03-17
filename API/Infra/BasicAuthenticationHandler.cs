using API.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace API.Infra
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        readonly IUsuarioServico _usuarioServico;

        public BasicAuthenticationHandler(IUsuarioServico usuarioServico,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _usuarioServico = usuarioServico;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string usuario;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                usuario = credentials[0];
                var senha = credentials[1];

                if (!_usuarioServico.ValidaCredenciais(usuario, senha))
                    throw new ArgumentException("Credenciais invalidas!");
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Erro de autenticação: {ex.Message}");
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, usuario),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

    }
}
