using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace challengeFiap.src.Models
{
    [Table("T_CLYVO_VET")]
    public class Veterinario
    {
        [Key]
        [Column("id_vet")]
        public int Id_vet { get; set; }
        [Required]
        [Column("nm_vet")]
        public string? Nm_vet { get; set; }
        [Required]
        [Column("cpf_vet")]
        public string? Cpf_vet { get; set; }
        [Required]
        [Column("crmv_vet")]
        public string? Crmv_vet { get; set; }
        [Required]
        [Column("email")]
        public string? Email_vet { get; set; }
        [Required]
        [Column("senha")]
        public string? Senha_vet { get; set; }

        protected Veterinario() { }

        public Veterinario(int id_vet, string nm_vet, string cpf_vet, string crmv_vet, string email_vet, string senha_vet)
        {
            Id_vet = id_vet;
            Nm_vet = nm_vet;
            Cpf_vet = cpf_vet;
            Crmv_vet = crmv_vet;
            Email_vet = email_vet;
            Senha_vet = senha_vet;
        }
    }
}
