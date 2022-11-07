using BancoMaster.Src.Models;
using System.Threading.Tasks;

namespace BancoMaster.Src.Services
{

    /// <summary>
    /// <para>Resumo: Interface Responsavel por representar ações de autenticação</para>
    /// <para>Versão: 1.0</para>
    /// </summary>

    public interface IAuthentication
    {
        string EncodePassword(string password);
        Task CreateClientWithoutDuplicatingAsync(Client client);
        string GenerateToken(Client client);
    }
}
