using challengeFiap.Domain;
using challengeFiap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChallengeFiap.Infrastruture.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Clinica> Clinicas { get; set; }
        public DbSet<EnderecoClinica> EnderecoClinicas { get; set; }
        public DbSet<VetClinica> VetClinicas { get; set; }
        public DbSet<Tutor> Tutor { get; set; }
        public DbSet<EnderecoTutor> EnderecoTutors { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<EnderecoAnimal> EnderecoAnimals { get; set; }
        public DbSet<CarteiraVacinal> CarteiraVacinals { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Prescricao> Prescricaos { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Veterinario>().ToTable("T_CLYVO_VET");
            modelBuilder.Entity<Veterinario>().HasKey(g => g.Id_vet);
            modelBuilder.Entity<Veterinario>().Property(g => g.Id_vet).HasColumnName("id_vet").IsRequired();
            modelBuilder.Entity<Veterinario>().Property(g => g.Nm_vet).HasColumnName("nm_vet").IsRequired();
            modelBuilder.Entity<Veterinario>().Property(g => g.Cpf_vet).HasColumnName("cpf_vet").IsRequired();
            modelBuilder.Entity<Veterinario>().HasIndex(g => g.Cpf_vet).HasDatabaseName("cpf_vet").IsUnique();
            modelBuilder.Entity<Veterinario>().Property(g => g.Crmv_vet).HasColumnName("crmv_vet").IsRequired();
            modelBuilder.Entity<Veterinario>().HasIndex(g => g.Crmv_vet).HasDatabaseName("crmv_vet").IsUnique();
            modelBuilder.Entity<Veterinario>().Property(g => g.Email_vet).HasColumnName("email").IsRequired();
            modelBuilder.Entity<Veterinario>().Property(g => g.Senha_vet).HasColumnName("senha").IsRequired();

            modelBuilder.Entity<Clinica>().ToTable("T_CLYVO_CLINICA");
            modelBuilder.Entity<Clinica>().HasKey(g => g.Id_clinica);
            modelBuilder.Entity<Clinica>().Property(g => g.Id_clinica).HasColumnName("id_clinica").IsRequired();
            modelBuilder.Entity<Clinica>().Property(g => g.Cnpj_clinica).HasColumnName("cnpj_clinica").IsRequired();
            modelBuilder.Entity<Clinica>().Property(g => g.Nm_clinica).HasColumnName("nm_clinica").IsRequired();

            modelBuilder.Entity<EnderecoClinica>().ToTable("T_CLYVO_ENDERECO_CLINICA");
            modelBuilder.Entity<EnderecoClinica>().HasKey(g => g.Id_endereco_clinica);
            modelBuilder.Entity<EnderecoClinica>().Property(g => g.Id_endereco_clinica).HasColumnName("id_endereco_clinica").IsRequired();
            modelBuilder.Entity<EnderecoClinica>().Property(g => g.Estado).HasColumnName("estado").IsRequired();
            modelBuilder.Entity<EnderecoClinica>().Property(g => g.Cidade).HasColumnName("cidade").IsRequired();
            modelBuilder.Entity<EnderecoClinica>().Property(g => g.Bairro).HasColumnName("bairro").IsRequired();
            modelBuilder.Entity<EnderecoClinica>().Property(g => g.Logradouro_rua).HasColumnName("logradouro_rua").IsRequired();
            modelBuilder.Entity<EnderecoClinica>().Property(g => g.Nr_rua).HasColumnName("nr_rua").IsRequired();
            modelBuilder.Entity<EnderecoClinica>().Property(g => g.Complemento).HasColumnName("complemento").IsRequired();
            modelBuilder.Entity<EnderecoClinica>().Property(g => g.Cep).HasColumnName("cep").IsRequired();
            modelBuilder.Entity<EnderecoClinica>().HasOne(g => g.Clinica).WithMany().HasForeignKey(g => g.Id_clinica);


            modelBuilder.Entity<VetClinica>().ToTable("T_CLYVO_VET_CLINICA");
            modelBuilder.Entity<VetClinica>().HasKey(g => g.Id_clinica_vet);
            modelBuilder.Entity<VetClinica>().Property(g => g.Id_clinica_vet).HasColumnName("id_clinica_vet").IsRequired();
            modelBuilder.Entity<VetClinica>().HasOne(g => g.Veterinario).WithMany().HasForeignKey(g => g.Id_vet).IsRequired();
            modelBuilder.Entity<VetClinica>().HasOne(g => g.Clinica).WithMany().HasForeignKey(g => g.Id_clinica).IsRequired();

            modelBuilder.Entity<Tutor>().ToTable("T_CLYVO_TUTOR");
            modelBuilder.Entity<Tutor>().HasKey(g => g.Id_tutor);
            modelBuilder.Entity<Tutor>().Property(g => g.Id_tutor).HasColumnName("id_tutor").IsRequired();
            modelBuilder.Entity<Tutor>().HasIndex(g => g.Cpf_tutor).HasDatabaseName("cpf_tutor").IsUnique();
            modelBuilder.Entity<Tutor>().Property(g => g.Nm_tutor).HasColumnName("nm_tutor").IsRequired();
            modelBuilder.Entity<Tutor>().Property(g => g.Nr_telefone_tutor).HasColumnName("nr_telefone_tutor").IsRequired();

            modelBuilder.Entity<EnderecoTutor>().ToTable("T_CLYVO_ENDERECO_TUTOR");
            modelBuilder.Entity<EnderecoTutor>().HasKey(g => g.Id_endereco_tutor);
            modelBuilder.Entity<EnderecoTutor>().Property(g => g.Id_endereco_tutor).HasColumnName("id_endereco_tutor").IsRequired();
            modelBuilder.Entity<EnderecoTutor>().Property(g => g.Pais).HasColumnName("pais").IsRequired();
            modelBuilder.Entity<EnderecoTutor>().Property(g => g.Estado).HasColumnName("estado").IsRequired();
            modelBuilder.Entity<EnderecoTutor>().Property(g => g.Cidade).HasColumnName("cidade").IsRequired();
            modelBuilder.Entity<EnderecoTutor>().Property(g => g.Bairro).HasColumnName("bairro").IsRequired();
            modelBuilder.Entity<EnderecoTutor>().Property(g => g.Logradouro_rua).HasColumnName("logradouro_rua").IsRequired();
            modelBuilder.Entity<EnderecoTutor>().Property(g => g.Nr_rua).HasColumnName("nr_rua").IsRequired();
            modelBuilder.Entity<EnderecoTutor>().Property(g => g.Complemento).HasColumnName("complemento").IsRequired();
            modelBuilder.Entity<EnderecoTutor>().Property(g => g.Cep).HasColumnName("cep").IsRequired();
            modelBuilder.Entity<EnderecoTutor>().HasOne(g => g.Tutor).WithMany().HasForeignKey(g => g.Id_tutor).IsRequired();


            modelBuilder.Entity<Animal>().ToTable("T_CLYVO_ANIMAL");
            modelBuilder.Entity<Animal>().HasKey(g => g.Id_animal);
            modelBuilder.Entity<Animal>().Property(g => g.Id_animal).HasColumnName("id_animal").IsRequired();
            modelBuilder.Entity<Animal>().Property(g => g.Rg_animal).HasColumnName("rg_animal").IsRequired();
            modelBuilder.Entity<Animal>().Property(g => g.Nr_microchip_animal).HasColumnName("nr_microchip_animal").IsRequired();
            modelBuilder.Entity<Animal>().Property(g => g.Nm_animal).HasColumnName("nm_animal").IsRequired();
            modelBuilder.Entity<Animal>().Property(g => g.Dt_nascimento_animal).HasColumnName("dt_nascimento_animal").IsRequired();
            modelBuilder.Entity<Animal>().Property(g => g.Peso_animal).HasPrecision(18, 2).HasColumnName("peso_animal").IsRequired();
            modelBuilder.Entity<Animal>().Property(g => g.Especie_animal).HasColumnName("especie_animal").IsRequired();
            modelBuilder.Entity<Animal>().Property(g => g.Raca_animal).HasColumnName("raca_animal").IsRequired();
            modelBuilder.Entity<Animal>().HasOne(g => g.Tutor).WithMany().HasForeignKey(g => g.Id_tutor).IsRequired();

            modelBuilder.Entity<EnderecoAnimal>().ToTable("T_CLYVO_ENDERECO_ANIMAL");
            modelBuilder.Entity<EnderecoAnimal>().HasKey(g => g.Id_endereco_animal);
            modelBuilder.Entity<EnderecoAnimal>().Property(g => g.Id_endereco_animal).HasColumnName("id_endereco_animal").IsRequired();
            modelBuilder.Entity<EnderecoAnimal>().Property(g => g.Pais).HasColumnName("pais").IsRequired();
            modelBuilder.Entity<EnderecoAnimal>().Property(g => g.Estado).HasColumnName("estado").IsRequired();
            modelBuilder.Entity<EnderecoAnimal>().Property(g => g.Cidade).HasColumnName("cidade").IsRequired();
            modelBuilder.Entity<EnderecoAnimal>().Property(g => g.Bairro).HasColumnName("bairro").IsRequired();
            modelBuilder.Entity<EnderecoAnimal>().Property(g => g.Logradouro_rua).HasColumnName("logradouro_rua").IsRequired();
            modelBuilder.Entity<EnderecoAnimal>().Property(g => g.Nr_rua).HasColumnName("nr_rua").IsRequired();
            modelBuilder.Entity<EnderecoAnimal>().Property(g => g.Complemento).HasColumnName("complemento").IsRequired();
            modelBuilder.Entity<EnderecoAnimal>().Property(g => g.Cep).HasColumnName("cep").IsRequired();
            modelBuilder.Entity<EnderecoAnimal>().HasOne(g => g.Animal).WithMany().HasForeignKey(g => g.Id_animal).IsRequired();

            modelBuilder.Entity<CarteiraVacinal>().ToTable("T_CLYVO_CARTEIRAVACINAL");
            modelBuilder.Entity<CarteiraVacinal>().HasKey(g => g.Id_carteiraVacinal);
            modelBuilder.Entity<CarteiraVacinal>().Property(g => g.Id_carteiraVacinal).HasColumnName("id_carteiravacinal").IsRequired();
            modelBuilder.Entity<CarteiraVacinal>().Property(g => g.Nm_vacina).HasColumnName("nm_vacina").IsRequired();
            modelBuilder.Entity<CarteiraVacinal>().Property(g => g.Dt_vacina_prevista).HasColumnName("dt_vacinacao_prevista").IsRequired();
            modelBuilder.Entity<CarteiraVacinal>().Property(g => g.Dt_vacina_efetuada).HasColumnName("dt_vacinacao_efetuada").IsRequired();
            modelBuilder.Entity<CarteiraVacinal>().Property(g => g.St_vacina).HasColumnName("st_vacinacao").IsRequired();
            modelBuilder.Entity<CarteiraVacinal>().HasOne(g => g.Animal).WithMany().HasForeignKey(g => g.Id_animal).IsRequired();

            modelBuilder.Entity<Consulta>().ToTable("T_CLYVO_CONSULTA");
            modelBuilder.Entity<Consulta>().HasKey(g => g.Id_consulta);
            modelBuilder.Entity<Consulta>().Property(g => g.Id_consulta).HasColumnName("id_consulta").IsRequired();
            modelBuilder.Entity<Consulta>().Property(g => g.Historico_consulta).HasColumnName("historico_consulta").IsRequired();
            modelBuilder.Entity<Consulta>().Property(g => g.St_consulta).HasColumnName("st_consulta").IsRequired();
            modelBuilder.Entity<Consulta>().Property(g => g.Dt_consulta).HasColumnName("dt_consulta").IsRequired();
            modelBuilder.Entity<Consulta>().HasOne(g => g.Veterinario).WithMany().HasForeignKey(g => g.Id_vet).IsRequired();
            modelBuilder.Entity<Consulta>().HasOne(g => g.Animal).WithMany().HasForeignKey(g => g.Id_animal).IsRequired();

            modelBuilder.Entity<Prescricao>().ToTable("T_CLYVO_PRESCRICAO");
            modelBuilder.Entity<Prescricao>().HasKey(g => g.Id_prescricao);
            modelBuilder.Entity<Prescricao>().Property(g => g.Id_prescricao).HasColumnName("id_prescricao").IsRequired();
            modelBuilder.Entity<Prescricao>().Property(g => g.Dt_emissao).HasColumnName("dt_emissao").IsRequired();
            modelBuilder.Entity<Prescricao>().Property(g => g.Dt_expiracao).HasColumnName("dt_expiracao").IsRequired();
            modelBuilder.Entity<Prescricao>().HasOne(g => g.Consulta).WithOne().HasForeignKey<Prescricao>(g => g.Id_consulta).IsRequired();
            modelBuilder.Entity<Prescricao>().Property(g => g.Observacoes_gerais).HasColumnName("observacoes_gerais").IsRequired();

            modelBuilder.Entity<Medicamento>().ToTable("T_CLYVO_MEDICAMENTO");
            modelBuilder.Entity<Medicamento>().HasKey(g => g.Id_medicamento);
            modelBuilder.Entity<Medicamento>().Property(g => g.Id_medicamento).HasColumnName("id_medicamento").IsRequired();

            modelBuilder.Entity<Medicamento>().Property(g => g.Id_prescricao).HasColumnName("id_prescricao").IsRequired();
            modelBuilder.Entity<Medicamento>().HasOne(g => g.Prescricao).WithMany().HasForeignKey(g => g.Id_prescricao).IsRequired();

            modelBuilder.Entity<Medicamento>().Property(g => g.Dosagem_medicamento).HasColumnName("dosagem_medicamento").IsRequired();
            modelBuilder.Entity<Medicamento>().Property(g => g.Frequencia).HasColumnName("frequencia").IsRequired();
            modelBuilder.Entity<Medicamento>().Property(g => g.Qtd_dias).HasColumnName("qtd_dias").IsRequired();


            base.OnModelCreating(modelBuilder);

        }



    }
}
