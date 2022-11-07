using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace BancoMaster.Src.Models
{

    /// <summary>
    /// <para> Resumo: Responsavel por representar tb_clients no banco de dados </para>
    /// <para> Criado por: Jesus Farias </para>
    /// <para>Versão: 1.0</para>
    /// </summary>
    
    [Table("tb_clients")]    
    public class Client
    {
        #region Attributes

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Document { get; set; }

        [Required]
        [InverseProperty("PIXKeySend")]
        public string PIXKey { get; set; }

        [ForeignKey("FK_ValueTransfer")]
        public double LastTransfer { get; set; }
        public double Balance { get; set; }

        #endregion
    }
}
