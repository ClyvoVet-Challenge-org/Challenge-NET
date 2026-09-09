using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    public class _93MedicamentoControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _93MedicamentoControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task Getmedicamento_Dados_RetornaCreated()
        {
            //Arrange
            var novomedicamento = new
            {
                Id_medicamento = 2,
                Id_prescricao = 1,
                Nm_medicamento = "AJUDA",
                Dosagem_medicamento = "GOTA",
                Frequencia = "2 vezes",
                Qtd_dias = 1
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/medicamentoes/criar/medicamento", novomedicamento);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var medicamentoCriado = await response.Content.ReadFromJsonAsync<Medicamento>();
            Assert.NotNull(medicamentoCriado);
            Assert.Equal(1, medicamentoCriado.Id_medicamento);
        }

        [Fact]
        public async Task Updatemedicamento_DadosValidos_RetornaSucesso()
        {
            //Arrange
            var id_medicamento = 1;
            var medicamentoAtualizado = new
            {
                Id_medicamento = id_medicamento,
                Id_prescricao = 1,
                Nm_medicamento = "AJUDA",
                Dosagem_medicamento = "GOTA",
                Frequencia = "2 vezes",
                Qtd_dias = 1
            };
            //Act
            var response = await _client.PutAsJsonAsync($"api/medicamentoes/atualizar/medicamento/{id_medicamento}", medicamentoAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
