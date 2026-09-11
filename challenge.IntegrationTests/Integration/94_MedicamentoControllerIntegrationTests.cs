using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    //Atenção!!! Tanto o create quanto o update precisam ter cuidado com os dados que serão adicionados, porque, se não, pode dar erro.          
    //Recomendação faz um a cada vez, não faz eles tudo junto
    [Collection("ApiCollection")]
    public class _94MedicamentoControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _94MedicamentoControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CriarMedicamento_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_medicamento = Random.Shared.Next(1, 100);

            var novoMedicamento = new
            {
                Id_medicamento = id_medicamento,
                Id_prescricao = 1,
                Nm_medicamento = "AJUDA",
                Dosagem_medicamento = "GOTA",
                Frequencia = "2 vezes",
                Qtd_dias = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/medicamentoes/criar/medicamento",novoMedicamento);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);

            var medicamentoCriado =await response.Content.ReadFromJsonAsync<Medicamento>();

            Assert.NotNull(medicamentoCriado);

            Assert.Equal("AJUDA",medicamentoCriado.Nm_medicamento);
        }

        [Fact]
        public async Task AtualizarMedicamento_DadosValidos_RetornaOk()
        {
            // Arrange
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

            // Act
            var response = await _client.PutAsJsonAsync($"api/medicamentoes/atualizar/medicamento/{id_medicamento}",medicamentoAtualizado);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Ocorrido da rejeicao: {erro}");


            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }
    }
}