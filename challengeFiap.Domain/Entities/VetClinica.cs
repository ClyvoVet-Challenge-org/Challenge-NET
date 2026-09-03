using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace challengeFiap.Domain.Entities
{
    [Table("T_CLYVO_VET_CLINICA")]
    public class VetClinica
    {
        [Key]
        [Column("id_clinica_vet")]
        public int Id_clinica_vet { get; set; }

        [Required]
        [Column("id_vet")]
        public int Id_vet { get; set; }
        [JsonIgnore]
        [ForeignKey("Id_vet")]
        public Veterinario? Veterinario { get; set; }
        [Required]
        [Column("id_clinica")]
        public int Id_clinica { get; set; }

        [JsonIgnore]
        [ForeignKey("Id_clinica")]
        public Clinica? Clinica { get; set; }

        protected VetClinica() { }

        public VetClinica(int id_clinica_vet, int id_vet, int id_clinica)
        {
            Id_clinica_vet = id_clinica_vet;
            Id_vet = id_vet;
            Id_clinica = id_clinica;
        }
    }
}
