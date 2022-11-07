using BancoMaster.Src.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancoMaster.Src.Repositories
{
    /// <summary>
    /// <para>Resumo: Responsavel por representar ações de CRUD relacionadas a clientes </para>
    /// <para>Criado por: Jesus Farias</para>
    /// <para>Versão: 1.0</para>
    /// </summary>
    
    public interface IClient
    {
        Task<List<Client>> GetAllClientsAsync();
        Task<Client> GetClientByPIXKey(string pix);
        Task NewClientAsync(Client clients);
        Task<Client> GetClientByEmailAsync(string email);
    }
}
