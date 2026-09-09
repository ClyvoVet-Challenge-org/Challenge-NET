using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    public class PrescricaoControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public PrescricaoControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task Getprescricao_Dados_RetornaCreated()
        {
            //Arrange
            var novoprescricao = new
            {
                Id_vet = 1,
                Dt_emissao = DateTime.Now,
                Dt_expiracao = new DateTime(2026, 09, 10),
                Id_consulta = 1,
                Observacoes_gerais = "Paciente esta bem"
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/prescricao", novoprescricao);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var prescricaoCriado = await response.Content.ReadFromJsonAsync<Prescricao>();
            Assert.NotNull(prescricaoCriado);
            Assert.Equal(1, prescricaoCriado.Id_prescricao);
        }

        [Fact]
        public async Task Updateprescricao_DadosValidos_RetornaSucesso()
        {
            //Arrange
            var id_prescricao = 1;
            var prescricaoAtualizado = new
            {
                Id_tutor = id_prescricao,
                Dt_emissao = DateTime.Now,
                Dt_expiracao = new DateTime(2026, 09, 10),
                Id_consulta = 1,
                Observacoes_gerais = "Paciente esta bem"
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/prescricao/atualizar/{id_prescricao}", prescricaoAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
