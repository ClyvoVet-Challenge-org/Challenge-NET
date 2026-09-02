using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace challengeFiap.src.Models
{
    [Table("T_CLYVO_TUTOR")]
    public class Tutor
    {
        [Key]
        [Column("id_tutor")]
        public int Id_tutor { get; set; }

        [Required]
        [Column("cpf_tutor")]
        public string? Cpf_tutor { get; set; }

        [Required]
        [Column("nm_tutor")]
        public string? Nm_tutor { get; set; }

        [Required]
        [Column("nr_telefone_tutor")]
        public string? Nr_telefone_tutor { get; set; }

        protected Tutor() { }

        public Tutor(int id_tutor, string cpf_tutor, string nm_tutor, string nr_telefone_tutor)
        {
            Id_tutor = id_tutor;
            Cpf_tutor = cpf_tutor;
            Nm_tutor = nm_tutor;
            Nr_telefone_tutor = nr_telefone_tutor;
        }
    }
}
