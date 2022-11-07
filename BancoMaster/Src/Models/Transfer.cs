using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoMaster.Src.Models
{

    /// <summary>
    /// <para> Resumo: Responsavel por representar tb_transfers no banco de dados </para>
    /// <para> Criado por: Jesus Farias </para>
    /// <para>Versão: 1.0</para>
    /// </summary>
    
    [Table("tb_transfers")]
    public class Transfer
    {
        #region Attributes

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [InverseProperty("LastTransfer")]
        public double Value { get; set; }

        [Required]
        [ForeignKey("FK_PIXSend")]
        public Client PIXKeySend { get; set; }

        [Required]
        public Client PIXKeyReceive { get; set; }

        #endregion
    }
}
