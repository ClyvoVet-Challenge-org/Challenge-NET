using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace challengeFiap.Domain.Entities
{
    [Table("T_CLYVO_CARTEIRAVACINAL")]
    public class CarteiraVacinal
    {
        [Key]
        [Column("id_carteiravacinal")]
        public int Id_carteiraVacinal { get;  set; }
        [Required]
        [Column("nm_vacina")]
        public string? Nm_vacina { get;  set; }
        [Required]
        [Column("dt_vacinacao_prevista")]
        public DateTime Dt_vacina_prevista { get; set; }
        [Required]
        [Column("dt_vacinacao_efetuada")]
        public DateTime Dt_vacina_efetuada { get;  set; }
        [Required]
        [Column("st_vacina")]
        public string? St_vacina { get;  set; }
        [Required]
        [Column("id_animal")]
        public int Id_animal { get;  set; }

        [JsonIgnore]
        [ForeignKey("Id_animal")]
        public Animal? Animal { get; set; }

        protected CarteiraVacinal() { }

        public CarteiraVacinal(int id_carteiraVacinal, string nm_vacina, DateTime dt_vacina_prevista, DateTime dt_vacina_efetuada, string st_vacina, int id_animal)
        {
            Id_carteiraVacinal = id_carteiraVacinal;
            Nm_vacina = nm_vacina;
            Dt_vacina_prevista = dt_vacina_prevista;
            Dt_vacina_efetuada = dt_vacina_efetuada;
            St_vacina = st_vacina;
            Id_animal = id_animal;
        }
    }
}
