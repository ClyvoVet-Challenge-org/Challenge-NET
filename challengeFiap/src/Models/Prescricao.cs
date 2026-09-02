using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace challengeFiap.src.Models
{
    [Table("T_CLYVO_PRESCRICAO")]
    public class Prescricao
    {
        [Key]
        [Column("id_prescricao")]
        public int Id_prescricao { get; set; }

        [Required]
        [Column("dt_emissao")]
        public DateTime Dt_emissao { get; set; }

        [Required]
        [Column("dt_expiracao")]
        public DateTime Dt_expiracao {  get; set; }
        
        [Required]
        [Column("id_consulta")]
        public int Id_consulta { get; set; }

        [JsonIgnore]
        [ForeignKey("Id_consulta")]
        public Consulta? Consulta { get; set; }

        [Required]
        [Column("observacoes_gerais")]
        public string? Observacoes_gerais { get; set; }

        protected Prescricao() { }

        public Prescricao(int id_prescricao, DateTime dt_emissao, DateTime dt_expiracao, int id_consulta, string observacoes_gerais)
        {
            Id_prescricao = id_prescricao;
            Dt_emissao = dt_emissao;
            Dt_expiracao = dt_expiracao;
            Id_consulta = id_consulta;
            Observacoes_gerais = observacoes_gerais;
        }
    }
}
