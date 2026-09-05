using challengeFiap.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace challengeFiap.Domain.Entities
{
    [Table("T_CLYVO_CONSULTA")]
    public class Consulta
    {
        [Key]
        [Column("id_consulta")]
        public int Id_consulta { get; set; }
        [Key]
        [Required]
        [Column("historico_consulta")]
        public string? Historico_consulta { get; set; }
        [Required]
        [Column("st_consulta")]
        public StatusConsulta St_consulta { get; set; }
        [Required]
        [Column("dt_consulta")]
        public DateTime Dt_consulta { get; set; }

        [Required]
        [Column("id_vet")]
        public int Id_vet { get; set; }

        [JsonIgnore]
        [ForeignKey("Id_vet")]
        public Veterinario? Veterinario{ get; set; }

        [Required]
        [Column("id_animal")]
        public int Id_animal { get; set; }

        [JsonIgnore]
        [ForeignKey("Id_animal")]
        public Animal? Animal { get; set; }

        protected Consulta() { }
        public Consulta(int id_consulta, string historico_consulta, StatusConsulta st_consulta, DateTime dt_consulta, int id_vet, int id_animal)
        {
            Id_consulta = id_consulta;
            Historico_consulta = historico_consulta;
            St_consulta = st_consulta;
            Dt_consulta = dt_consulta;
            Id_vet = id_vet;
            Id_animal = id_animal;
        }
    }
}
