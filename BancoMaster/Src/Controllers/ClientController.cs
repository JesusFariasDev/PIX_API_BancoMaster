using BancoMaster.Src.Models;
using BancoMaster.Src.Repositories;
using BancoMaster.Src.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BancoMaster.Src.Controllers
{
    [ApiController]
    [Route("/Clients")]
    [Produces("application/json")]
    public class ClientController : ControllerBase
    {
        #region Attributes

        private readonly IClient _repository;
        private readonly IAuthentication _services;

        #endregion

        #region Constructors

        public ClientController(IClient repository, IAuthentication services)
        {
            _repository = repository;
            _services = services;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Carrega todos os clientes
        /// </summary>
        /// <param name="clients"> Busca todos os clientes </param>
        /// <returns> ActionResult </returns>
        /// <response code="200"> Retorna os clientes cadastrados </response>
        /// <response code="204"> Clientes inexistentes </response>
        
        [HttpGet]
        public async Task<ActionResult> GetAllClientsAsync()
        {
            var list = await _repository.GetAllClientsAsync();
            if (list.Count < 0) return NoContent();
            return Ok(list);
        }

        /// <summary>
        /// Carrega cliente pela chave PIX 
        /// </summary>
        /// <param name="pix"> Busca cliente pela chave PIX </param>
        /// <returns> ActionResult </returns>
        /// <response code="200"> Retorna o cliente </response>
        /// <response code="204"> Chave PIX inexistente </response>
        
        [HttpGet("pix/{pix}")]
        public async Task<ActionResult> GetClientByPIXKey([FromRoute] string pix)
        {
            try
            {
                return Ok(await _repository.GetClientByPIXKey(pix));
            }
            catch (Exception e)
            {
                return NotFound(new { e.Message });
            }
        }

        /// <summary>
        /// Cadastrar novo cliente
        /// </summary>
        /// <returns> ActionResult </returns>
        /// <response code="201"> Cliente cadastrado </response>
        /// <response code="401"> Chave PIX ou Documento já cadastrados </response>
        
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> NewClientAsync([FromBody] Client client)
        {
            try
            {
                await _services.CreateClientWithoutDuplicatingAsync(client);
                return Created($"Clients/email/{client.Email}", client);
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        /// <summary>
        /// Login de cliente
        /// </summary>
        /// <returns> ActionResult </returns>
        /// <response code="200"> CLiente logado </response>
        /// <response code="401"> E-mail ou senha inválidos </response>
        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAsync([FromBody] Client client)
        {
            var assistant = await _repository.GetClientByEmailAsync(client.Email);
            if (assistant == null) return Unauthorized(new
            {
                Message = "E-mail inválido"
            });
            if (assistant.Password != _services.EncodePassword(client.Password))
                return Unauthorized(new { Message = "Senha inválida" });
            var token = "Bearer " + _services.GenerateToken(assistant);
            return Ok(new { Client = assistant, Token = token });
        }

        #endregion

    }
}
