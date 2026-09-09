using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    public class _94PrescricaoControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _94PrescricaoControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task Getprescricao_Dados_RetornaCreated()
        {
            //Arrange
            var novoprescricao = new
            {
                Id_vet = 2,
                Dt_emissao = DateTime.Now,
                Dt_expiracao = new DateTime(2026, 09, 10),
                Id_consulta = 1,
                Observacoes_gerais = "Paciente esta bem"
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/prescricaos/criar/prescricao", novoprescricao);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

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
                Id_prescricao = id_prescricao,
                Dt_emissao = DateTime.Now,
                Dt_expiracao = new DateTime(2026, 09, 10),
                Id_consulta = 1,
                Observacoes_gerais = "Paciente esta bem"
            };
            //Act
            var response = await _client.PutAsJsonAsync($"api/prescricaos/atualizar/prescricao/{id_prescricao}", prescricaoAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
