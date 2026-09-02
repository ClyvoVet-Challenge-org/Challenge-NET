using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace challengeFiap.src.Models
{
    [Table("T_CLYVO_MEDICAMENTO")]
    public class Medicamento
    {
        [Key]
        [Column("id_medicamento")]
        public int Id_medicamento { get; set; }

        [Required]
        [Column("id_prescricao")]
        public int Id_prescricao { get; set; }

        [JsonIgnore]
        [ForeignKey("Id_prescricao")]
        public Prescricao? Prescricao { get; set; }
        [Required]
        [Column("nm_medicamento")]
        public string? Nm_medicamento { get; set; }
        [Required]
        [Column("dosagem_medicamento")]
        public string? Dosagem_medicamento { get; set; }
        [Required]
        [Column("frequencia")]
        public string? Frequencia { get; set; }
        [Required]
        [Column("qtd_dias")]
        public int Qtd_dias { get; set; }

        protected Medicamento() { }

        public Medicamento(int id_medicamento, int id_prescricao, string nm_medicamento, string dosagem_medicamento, string frequencia, int qtd_dias)
        {
            Id_medicamento = id_medicamento;
            Id_prescricao = id_prescricao;
            Nm_medicamento = nm_medicamento;
            Dosagem_medicamento = dosagem_medicamento;
            Frequencia = frequencia;
            Qtd_dias = qtd_dias;
        }
    }
}
