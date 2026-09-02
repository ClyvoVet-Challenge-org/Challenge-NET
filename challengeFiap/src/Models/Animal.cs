using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace challengeFiap.src.Models
{
    [Table("T_CLYVO_ANIMAL")]
    public class Animal
    {
        [Key]
        [Column("id_animal")]
        public int Id_animal { get; set; }
        [Required]
        [Column("rg_animal")]
        public string? Rg_animal { get; set; }
        [Required]
        [Column("nr_microchip_animal")]
        public string? Nr_microchip_animal { get; set; } 
        [Required]
        [Column("nm_animal")]
        public string? Nm_animal { get; set; }
        [Required]
        [Column("dt_nascimento_animal")]
        public DateTime Dt_nascimento_animal { get; set; }
        [Required]
        [Column("peso_animal")]
        public decimal Peso_animal { get; set; }
        [Required]
        [Column("especie_animal")]
        public string? Especie_animal { get; set; }
        [Required]
        [Column("raca_animal")]
        public string? Raca_animal { get; set; } 

        [Required]
        [Column("id_tutor")]
        public int Id_tutor { get; set; }

        [JsonIgnore]
        [ForeignKey("Id_tutor")]
        public Tutor? Tutor{ get; set; }

        protected Animal() { }

        public Animal(int id_animal, string rg_animal, string nr_microchip_animal, string nm_animal, DateTime dt_nascimento_animal, decimal peso_animal, string especie_animal, string raca_animal, int id_tutor)
        {
            Id_animal = id_animal;
            Rg_animal = rg_animal;
            Nr_microchip_animal = nr_microchip_animal;
            Nm_animal = nm_animal;
            Dt_nascimento_animal = dt_nascimento_animal;
            Peso_animal = peso_animal;
            Especie_animal = especie_animal;
            Raca_animal = raca_animal;
            Id_tutor = id_tutor;
        }
    }
}
