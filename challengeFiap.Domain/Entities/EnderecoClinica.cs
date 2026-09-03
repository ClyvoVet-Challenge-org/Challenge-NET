using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace challengeFiap.Domain.Entities
{
    [Table("T_CLYVO_ENDERECO_CLINICA")]
    public class EnderecoClinica 
    {
        [Key]
        [Column("id_endereco_clinica")]
        public int Id_endereco_clinica { get; set; }
        [Required]
        [Column("pais")]
        public string? Pais { get; set; }
        [Required]
        [Column("estado")]
        public string? Estado { get; set; }
        [Required]
        [Column("cidade")]
        public string? Cidade { get; set; }
        [Required]
        [Column("bairro")]
        public string? Bairro { get; set; }
        [Required]
        [Column("logradouro_rua")]
        public string? Logradouro_rua { get; set; }
        [Required]
        [Column("nr_rua")]
        public string? Nr_rua { get;  set; }
        [Required]
        [Column("complemento")]
        public string? Complemento { get; set; }
        [Required]
        [Column("cep")]
        public string? Cep { get;  set; }

        [Required]
        [Column("id_clinica")]
        public int Id_clinica { get; set; }

        [JsonIgnore]
        [ForeignKey("Id_clinica")]
        public Clinica? Clinica { get; set; }
        
        protected EnderecoClinica() { }

        public EnderecoClinica(int id_endereco_clinica, string pais, string estado, string cidade, string bairro, string logradouro_rua, string nr_rua, string complemento, string cep, int id_clinica)
        {
            Id_endereco_clinica = id_endereco_clinica;
            Pais = pais;
            Estado = estado;
            Cidade = cidade;
            Bairro = bairro;
            Logradouro_rua = logradouro_rua;
            Nr_rua = nr_rua;
            Complemento = complemento;
            Cep = cep;
            Id_clinica = id_clinica;
        }
    }
}
