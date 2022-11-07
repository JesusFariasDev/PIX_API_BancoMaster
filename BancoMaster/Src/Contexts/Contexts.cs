using BancoMaster.Src.Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// <para>Resumo: Classe contexto, responsavel por carregar contexto e definirDbSets </para >
/// <para>Versão: 1.0 </para>
/// </summary>
public class BankContext : DbContext
    {
    #region Attributes
        
    public DbSet<Client> Clients { get; set; }
    public DbSet<Transfer> Transfers { get; set; }

    #endregion

    #region Constructors

    public BankContext(DbContextOptions<BankContext> opt) : base(opt)
    {

    }

    #endregion
}

