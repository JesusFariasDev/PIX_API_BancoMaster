using BancoMaster.Src.Models;
using BancoMaster.Src.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BancoMaster.Src.Services.Implementations
{

    /// <summary>
    /// <para>Resumo: Classe responsavel por implementar IAutentication </para>
    /// <para> Versão: 1.0 </para>
    /// </summary>
    
    public class AuthenticationServices : IAuthentication
    {

        #region Attributes

        private IClient _repository;
        public IConfiguration Configuration { get; }

        #endregion

        #region Constructors

        public AuthenticationServices(IClient repository, IConfiguration configuration)
        {
            _repository = repository;
            Configuration = configuration;
        }

        #endregion

        #region Methods

        /// <summary>
        /// <para>Resumo: Método responsavel por criptografar senhas </para>
        /// </summary>
        /// <param name="password"> Senha a ser criptografada </param>
        /// <returns>string</returns>

        public string EncodePassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// <para>Resumo: Método assíncrono responsavel por cadastrar cliente sem duplicar no banco de dados </para>
        /// </summary>
        /// <param name="client"> Construtor para cadastrar cliente </param>

        public async Task CreateClientWithoutDuplicatingAsync(Client client)
        {
            var assistant = await _repository.GetClientByEmailAsync(client.Email);

            if (assistant != null) throw new Exception("Este email já está sendo utilizado");

            client.Password = EncodePassword(client.Password);

            await _repository.NewClientAsync(client);
        }

        /// <summary>
        /// <para>Resumo: Método responsavel por gerar token JWT</para>
        /// </summary>
        /// <param name="client"> Construtor de cliente que tenha parametros de e-mail e senha </param>
        /// <returns>string</returns>

        public string GenerateToken(Client client)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["Settings:Secret"]);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
            new Claim[]
            {
            new Claim(ClaimTypes.Email, client.Email.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
            )
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        #endregion
    }
}
