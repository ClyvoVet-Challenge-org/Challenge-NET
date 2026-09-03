using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace challengeFiap.Domain.Entities
{
    [Table("T_CLYVO_CLINICA")]
    public class Clinica
    {
        [Key]
        [Column("id_clinica")]
        public int Id_clinica { get; set; }
     
        [Required]
        [Column("cnpj_clinica")]
        public string? Cnpj_clinica { get; set; }

        [Required]
        [Column("nm_clinica")]
        public string? Nm_clinica { get; set; }

        protected Clinica() { }

        public Clinica(int id_clinica, string cnpj_clinica, string nm_clinica)
        {
            Id_clinica = id_clinica;
            Cnpj_clinica = cnpj_clinica;
            Nm_clinica = nm_clinica;
        }
    }
}
