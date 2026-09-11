using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    //Atenção!!! Tanto o create quanto o update precisam ter cuidado com os dados que serão adicionados, porque, se não, pode dar erro.          
    //Recomendação faz um a cada vez, não faz eles tudo junto
    [Collection("ApiCollection")]
    public class _93PrescricaoControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _93PrescricaoControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CriarPrescricao_DadosValidos_RetornaCreated()
        {
            // Arrange
            var id_prescricao = Random.Shared.Next(1, 100);

            var novaPrescricao = new
            {
                Id_prescricao = id_prescricao,
                Dt_emissao = new DateTime(2026, 9, 2),
                Dt_expiracao = new DateTime(2026, 9, 18),
                Id_consulta = 2,
                Observacoes_gerais = "bom"
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/prescricaos/criar/prescricao",novaPrescricao);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);

            var prescricaoCriada =await response.Content.ReadFromJsonAsync<Prescricao>();

            Assert.NotNull(prescricaoCriada);

            Assert.Equal(id_prescricao,prescricaoCriada.Id_prescricao);
        }

        [Fact]
        public async Task AtualizarPrescricao_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_prescricao =1;

            var prescricaoAtualizada = new
            {
                Id_prescricao = id_prescricao,
                Dt_emissao = new DateTime(2026, 9, 9),
                Dt_expiracao = new DateTime(2026, 9, 10),
                Id_consulta = 2,
                Observacoes_gerais = "Paciente esta bem"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/prescricaos/atualizar/prescricao/{id_prescricao}",prescricaoAtualizada);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode,$"Status: {response.StatusCode} - Erro: {erro}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }
        [Fact]
        public async Task GetPrescricao_dados_RetornaOk()
        {
            //Arrage
            var id_Prescricao = 1;

            //Act
            var response = await _client.GetAsync($"api/prescricaos/relatorio/prescricao/{id_Prescricao}");
            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var Prescricao = await response.Content.ReadFromJsonAsync<Prescricao>();

            Assert.NotNull(Prescricao);
            Assert.Equal(id_Prescricao, Prescricao.Id_prescricao);
        }

        [Fact]
        public async Task DeletePrescricao_dados_RetornaOk()
        {
            // Arrange
            var id_Prescricao = 1;

            // Act
            var response = await _client.DeleteAsync($"api/prescricaos/deleta/prescricao/{id_Prescricao}");
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}

