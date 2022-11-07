using Microsoft.EntityFrameworkCore;
using BancoMaster.Src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace BancoMaster.Src.Repositories.Implementations
{

    /// <summary>
    /// <para>Summary: Classe responsável por implementar a interface IClient </para>
    /// <para>Created by: Jesus Farias </para>
    /// <para> Version: 1.0 </para>
    /// </summary>
    
    public class ClientRepository : IClient
    {
        #region Attributes

        private readonly BankContext _contexts;

        #endregion

        #region Constructors

        public ClientRepository(BankContext contexts)
        {
            _contexts = contexts;
        }


        #endregion

        #region Methods

        /// <summary>
        /// <para>Resumo: Método assíncrono que busca todos os clientes registrados </para>
        /// </summary>
        /// <return> Lista de clientes registrados </return>
        
        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await _contexts.Clients.ToListAsync();
        }

        /// <summary>
        /// <para>Resumo: Método assíncrono busca cliente pela chave PIX</para>
        /// </summary>
        /// <param> "name="pix"> PIX do cliente </param>
        /// <return> Cliente pela chave PIX </return>
        /// <exception cref="Exception"> Valor da chave PIX do cliente não pode ser nulo </exception>
        
        public async Task<Client> GetClientByPIXKey(string pix)
        {
            if (!ExistsPIX(pix)) throw new Exception("Chave PIX não encontrada");
            return await _contexts.Clients.FirstOrDefaultAsync(p => p.PIXKey == pix);           
        }

        /// <summary>
        /// Resumo: Método assíncrono para cadastrar novo cliente
        /// </summary>
        /// <param> "name="clients"> Construtor para cadastrar clientes </param>
        /// <exception cref="Exception">Documento já existe</exception>
        /// <exception cref="Exception">Chave PIX já existe</exception>

        public async Task NewClientAsync(Client clients)
        {
            if (await ExistsDocument(clients.Document)) throw new Exception("Cliente já existe no sistema!");
            if (await ExistsPIXKey(clients.PIXKey)) throw new Exception("Chave PIX já existe no sistema!");

            await _contexts.Clients.AddAsync(new Client
            {
                Name = clients.Name,
                Email = clients.Email,
                Password = clients.Password,
                Document = clients.Document,
                PIXKey = clients.PIXKey,
                Balance = clients.Balance
            });
            await _contexts.SaveChangesAsync();
        }

        /// <summary>
        /// <para>Resumo: Método assíncrono para pegar um cliente pelo documento</para>
        /// </summary>
        /// <param name="documento">Documento do cliente</param>
        /// <return>Client</return>

        public async Task<Client> GetClientByEmailAsync(string email)
        {
            return await _contexts.Clients.FirstOrDefaultAsync(c => c.Email == email);
        }

        // Funções auxiliares
        bool ExistsPIX(string pix)
        {
            var assistant = _contexts.Clients.FirstOrDefault(k => k.PIXKey == pix);
            return assistant != null;
        }
        private async Task<bool> ExistsPIXKey(string pix)
        {
            var assistant = await _contexts.Clients.FirstOrDefaultAsync(c => c.PIXKey == pix);
            return assistant != null;
        }
        private async Task<bool> ExistsDocument(string document)
        {
            var assistant = await _contexts.Clients.FirstOrDefaultAsync(c => c.Document == document);
            return assistant != null;
        }

        public string EncodePassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(bytes);
        }

        #endregion
    }
}
