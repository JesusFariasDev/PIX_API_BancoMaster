using BancoMaster.Src.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancoMaster.Src.Repositories
{
    /// <summary>
    /// <para>Resumo: Responsavel por representar transferências entre contas</para>
    /// <para>Criado por: Jesus Farias</para>
    /// <para>Versão: 1.0</para>
    /// </summary>
    public interface ITransfer
    {
        Task<List<Transfer>> LoadMyTransfersAsync(string pix);
        Task NewTransferAsync(Transfer transfer);
    }
}
