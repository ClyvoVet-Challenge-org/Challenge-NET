using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace challengeFiap.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_CLYVO_CLINICA",
                columns: table => new
                {
                    id_clinica = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    cnpj_clinica = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    nm_clinica = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLYVO_CLINICA", x => x.id_clinica);
                });

            migrationBuilder.CreateTable(
                name: "T_CLYVO_RESPONSAVEL",
                columns: table => new
                {
                    id_responsavel = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    cpf_responsavel = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    nm_responsavel = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    nr_telefone_responsavel = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLYVO_RESPONSAVEL", x => x.id_responsavel);
                });

            migrationBuilder.CreateTable(
                name: "T_CLYVO_VET",
                columns: table => new
                {
                    id_vet = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nm_vet = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    cpf_vet = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    crmv_vet = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    email = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    senha = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLYVO_VET", x => x.id_vet);
                });

            migrationBuilder.CreateTable(
                name: "T_CLYVO_ENDERECO_CLINICA",
                columns: table => new
                {
                    id_endereco_clinica = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    pais = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    estado = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    cidade = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    bairro = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    logradouro_rua = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    nr_rua = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    complemento = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    cep = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    id_clinica = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLYVO_ENDERECO_CLINICA", x => x.id_endereco_clinica);
                    table.ForeignKey(
                        name: "FK_T_CLYVO_ENDERECO_CLINICA_T_CLYVO_CLINICA_id_clinica",
                        column: x => x.id_clinica,
                        principalTable: "T_CLYVO_CLINICA",
                        principalColumn: "id_clinica",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_CLYVO_ANIMAL",
                columns: table => new
                {
                    id_animal = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    rg_animal = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    nr_microchip_animal = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    nm_animal = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dt_nascimento_animal = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    peso_animal = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    especie_animal = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    raca_animal = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    id_responsavel = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLYVO_ANIMAL", x => x.id_animal);
                    table.ForeignKey(
                        name: "FK_T_CLYVO_ANIMAL_T_CLYVO_RESPONSAVEL_id_responsavel",
                        column: x => x.id_responsavel,
                        principalTable: "T_CLYVO_RESPONSAVEL",
                        principalColumn: "id_responsavel",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_CLYVO_ENDERECO_RESPONSAVEL",
                columns: table => new
                {
                    id_endereco_responsavel = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    pais = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    estado = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    cidade = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    bairro = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    logradouro_rua = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    nr_rua = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    complemento = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    cep = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    id_responsavel = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLYVO_ENDERECO_RESPONSAVEL", x => x.id_endereco_responsavel);
                    table.ForeignKey(
                        name: "FK_T_CLYVO_ENDERECO_RESPONSAVEL_T_CLYVO_RESPONSAVEL_id_responsavel",
                        column: x => x.id_responsavel,
                        principalTable: "T_CLYVO_RESPONSAVEL",
                        principalColumn: "id_responsavel",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_CLYVO_VET_CLINICA",
                columns: table => new
                {
                    id_clinica_vet = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    id_vet = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    id_clinica = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLYVO_VET_CLINICA", x => x.id_clinica_vet);
                    table.ForeignKey(
                        name: "FK_T_CLYVO_VET_CLINICA_T_CLYVO_CLINICA_id_clinica",
                        column: x => x.id_clinica,
                        principalTable: "T_CLYVO_CLINICA",
                        principalColumn: "id_clinica",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_CLYVO_VET_CLINICA_T_CLYVO_VET_id_vet",
                        column: x => x.id_vet,
                        principalTable: "T_CLYVO_VET",
                        principalColumn: "id_vet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_CLYVO_CARTEIRAVACINAL",
                columns: table => new
                {
                    id_carteiravacinal = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nm_vacina = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dt_vacinacao_prevista = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    dt_vacinacao_efetuada = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    st_vacinacao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    id_animal = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLYVO_CARTEIRAVACINAL", x => x.id_carteiravacinal);
                    table.ForeignKey(
                        name: "FK_T_CLYVO_CARTEIRAVACINAL_T_CLYVO_ANIMAL_id_animal",
                        column: x => x.id_animal,
                        principalTable: "T_CLYVO_ANIMAL",
                        principalColumn: "id_animal",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_CLYVO_CONSULTA",
                columns: table => new
                {
                    id_consulta = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    historico_consulta = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    st_consulta = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dt_consulta = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    id_vet = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    id_animal = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLYVO_CONSULTA", x => x.id_consulta);
                    table.ForeignKey(
                        name: "FK_T_CLYVO_CONSULTA_T_CLYVO_ANIMAL_id_animal",
                        column: x => x.id_animal,
                        principalTable: "T_CLYVO_ANIMAL",
                        principalColumn: "id_animal",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_CLYVO_CONSULTA_T_CLYVO_VET_id_vet",
                        column: x => x.id_vet,
                        principalTable: "T_CLYVO_VET",
                        principalColumn: "id_vet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_CLYVO_ENDERECO_ANIMAL",
                columns: table => new
                {
                    id_endereco_animal = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    pais = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    estado = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    cidade = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    bairro = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    logradouro_rua = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    nr_rua = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    complemento = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    cep = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    id_animal = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLYVO_ENDERECO_ANIMAL", x => x.id_endereco_animal);
                    table.ForeignKey(
                        name: "FK_T_CLYVO_ENDERECO_ANIMAL_T_CLYVO_ANIMAL_id_animal",
                        column: x => x.id_animal,
                        principalTable: "T_CLYVO_ANIMAL",
                        principalColumn: "id_animal",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_CLYVO_PRESCRICAO",
                columns: table => new
                {
                    id_prescricao = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    dt_emissao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    dt_expiracao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    id_consulta = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    observacoes_gerais = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLYVO_PRESCRICAO", x => x.id_prescricao);
                    table.ForeignKey(
                        name: "FK_T_CLYVO_PRESCRICAO_T_CLYVO_CONSULTA_id_consulta",
                        column: x => x.id_consulta,
                        principalTable: "T_CLYVO_CONSULTA",
                        principalColumn: "id_consulta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_CLYVO_MEDICAMENTO",
                columns: table => new
                {
                    id_medicamento = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    id_prescricao = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    nm_medicamento = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dosagem_medicamento = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    frequencia = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    qtd_dias = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLYVO_MEDICAMENTO", x => x.id_medicamento);
                    table.ForeignKey(
                        name: "FK_T_CLYVO_MEDICAMENTO_T_CLYVO_PRESCRICAO_id_prescricao",
                        column: x => x.id_prescricao,
                        principalTable: "T_CLYVO_PRESCRICAO",
                        principalColumn: "id_prescricao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_CLYVO_ANIMAL_id_responsavel",
                table: "T_CLYVO_ANIMAL",
                column: "id_responsavel");

            migrationBuilder.CreateIndex(
                name: "IX_T_CLYVO_CARTEIRAVACINAL_id_animal",
                table: "T_CLYVO_CARTEIRAVACINAL",
                column: "id_animal");

            migrationBuilder.CreateIndex(
                name: "IX_T_CLYVO_CONSULTA_id_animal",
                table: "T_CLYVO_CONSULTA",
                column: "id_animal");

            migrationBuilder.CreateIndex(
                name: "IX_T_CLYVO_CONSULTA_id_vet",
                table: "T_CLYVO_CONSULTA",
                column: "id_vet");

            migrationBuilder.CreateIndex(
                name: "IX_T_CLYVO_ENDERECO_ANIMAL_id_animal",
                table: "T_CLYVO_ENDERECO_ANIMAL",
                column: "id_animal");

            migrationBuilder.CreateIndex(
                name: "IX_T_CLYVO_ENDERECO_CLINICA_id_clinica",
                table: "T_CLYVO_ENDERECO_CLINICA",
                column: "id_clinica");

            migrationBuilder.CreateIndex(
                name: "IX_T_CLYVO_ENDERECO_RESPONSAVEL_id_responsavel",
                table: "T_CLYVO_ENDERECO_RESPONSAVEL",
                column: "id_responsavel");

            migrationBuilder.CreateIndex(
                name: "IX_T_CLYVO_MEDICAMENTO_id_prescricao",
                table: "T_CLYVO_MEDICAMENTO",
                column: "id_prescricao");

            migrationBuilder.CreateIndex(
                name: "IX_T_CLYVO_PRESCRICAO_id_consulta",
                table: "T_CLYVO_PRESCRICAO",
                column: "id_consulta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "cpf_responsavel",
                table: "T_CLYVO_RESPONSAVEL",
                column: "cpf_responsavel",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "cpf_vet",
                table: "T_CLYVO_VET",
                column: "cpf_vet",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "crmv_vet",
                table: "T_CLYVO_VET",
                column: "crmv_vet",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_CLYVO_VET_CLINICA_id_clinica",
                table: "T_CLYVO_VET_CLINICA",
                column: "id_clinica");

            migrationBuilder.CreateIndex(
                name: "IX_T_CLYVO_VET_CLINICA_id_vet",
                table: "T_CLYVO_VET_CLINICA",
                column: "id_vet");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_CLYVO_CARTEIRAVACINAL");

            migrationBuilder.DropTable(
                name: "T_CLYVO_ENDERECO_ANIMAL");

            migrationBuilder.DropTable(
                name: "T_CLYVO_ENDERECO_CLINICA");

            migrationBuilder.DropTable(
                name: "T_CLYVO_ENDERECO_RESPONSAVEL");

            migrationBuilder.DropTable(
                name: "T_CLYVO_MEDICAMENTO");

            migrationBuilder.DropTable(
                name: "T_CLYVO_VET_CLINICA");

            migrationBuilder.DropTable(
                name: "T_CLYVO_PRESCRICAO");

            migrationBuilder.DropTable(
                name: "T_CLYVO_CLINICA");

            migrationBuilder.DropTable(
                name: "T_CLYVO_CONSULTA");

            migrationBuilder.DropTable(
                name: "T_CLYVO_ANIMAL");

            migrationBuilder.DropTable(
                name: "T_CLYVO_VET");

            migrationBuilder.DropTable(
                name: "T_CLYVO_RESPONSAVEL");
        }
    }
}
