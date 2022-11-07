using BancoMaster.Src.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoMaster.Src.Repositories.Implementations
{
    /// <summary>
    /// <para>Summary: Classe responsável por implementar a interface ITransfer </para>
    /// <para>Created by: Jesus Farias </para>
    /// <para> Version: 1.0 </para>
    /// </summary>
    public class TransferRepository : ITransfer
    {
        #region Attributes

        private readonly BankContext _contexts;

        #endregion

        #region Contructors

        public TransferRepository(BankContext contexts)
        {
            _contexts = contexts;
        }

        #endregion

        #region Methods

        /// <summary>
        /// <para>Resumo: Método assíncrono carregar todas as transferências pela chave pix </para>
        /// </summary>
        /// <param name="pix">Chave PIX do cliente</param>
        /// <return>Client</return>
        
        public async Task<List<Transfer>> LoadMyTransfersAsync(string pix)
        {
            var pixKey = _contexts.Clients.FirstOrDefault(p => p.PIXKey == pix);

            if(pixKey != null)
            {
                return await _contexts.Transfers
                    .Include(t => t.PIXKeySend)
                    .ToListAsync();
            }
            else
            {
                throw new Exception("Chave PIX não existe!");
            }
        }

        /// <summary>
        /// <para>Resumo: Método assíncrono para iniciar uma transferências </para>
        /// </summary>
        /// <return>Client</return>

        public async Task NewTransferAsync(Transfer transfer)
        {
            if(!ExistsPIXKeySend(transfer.PIXKeySend.PIXKey)) throw new Exception("Chave de envio não existe!");
            if(!ExistsPIXKeyReceive(transfer.PIXKeyReceive.PIXKey)) throw new Exception("Chave de recebimento não existe!");

            await _contexts.Transfers.AddAsync(new Transfer
            {
                Value = transfer.Value,
                PIXKeySend = transfer.PIXKeySend,
                PIXKeyReceive = transfer.PIXKeyReceive
            });
            await _contexts.SaveChangesAsync();

            await UpdateBalanceSendAsync(transfer.PIXKeySend.PIXKey, transfer.Value);
            await UpdateBalanceReceiveAsync(transfer.PIXKeyReceive.PIXKey, transfer.Value);
        }

        // Funções auxiliares
        public async Task UpdateBalanceSendAsync(string pixKey, double LastTransfer)
        {
            var pix = _contexts.Clients.FirstOrDefault(p => p.PIXKey == pixKey);

            if (pix != null)
            {
                pix.Balance = pix.Balance - LastTransfer;
                _contexts.Clients.Update(pix);
                await _contexts.SaveChangesAsync();
            }
        }

        public async Task UpdateBalanceReceiveAsync(string pixKey, double LastTransfer)
        {
            var pix = _contexts.Clients.FirstOrDefault(p => p.PIXKey == pixKey);

            if (pix != null)
            {
                pix.Balance = pix.Balance + LastTransfer;
                _contexts.Clients.Update(pix);
                await _contexts.SaveChangesAsync();
            }
        }

        bool ExistsPIXKeySend(string pixKey)
        {
            var assistant = _contexts.Clients.FirstOrDefault(p => p.PIXKey == pixKey);
            return assistant != null;
        }

        bool ExistsPIXKeyReceive(string pixKey)
        {
            var assistant = _contexts.Clients.FirstOrDefault(p => p.PIXKey == pixKey);
            return assistant != null;
        }

        #endregion
    }
}
