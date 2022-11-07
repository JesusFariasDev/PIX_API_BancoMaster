using BancoMaster.Src.Models;
using BancoMaster.Src.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BancoMaster.Src.Controllers
{
    [ApiController]
    [Route("/Transfers")]
    [Produces("application/json")]
    public class TransferController : ControllerBase
    {
        #region Attributes

        private readonly ITransfer _repository;

        #endregion

        #region Constructors

        public TransferController(ITransfer repository)
        {
            _repository = repository;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Carregar transferências pela chave PIX
        /// </summary>
        /// <returns> ActionResult </returns>
        /// <response code="201"> Transação encontrada </response>
        /// <response code="204"> Chave PIX não encontrada </response>
        
        [HttpGet]
        public async Task<ActionResult> LoadMyTransfersAsync(string pix)
        {
            var list = await _repository.LoadMyTransfersAsync(pix);
            if (list.Count < 1) return NoContent();
            return Ok(list);
        }

        /// <summary>
        /// Iniciar uma nova transferência
        /// </summary>
        /// <returns> ActionResult </returns>
        /// <response code="201"> Transação realizada </response>
        /// <response code="400"> Transação não realizada </response>
        
        [HttpPost]
        public async Task<ActionResult> NewTransferAsync([FromBody] Transfer transfer)
        {
            try
            {
                await _repository.NewTransferAsync(transfer);
                return Created($"/Transfer", transfer);
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        #endregion
    }
}
