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
                Id_prescricao = 88,
                Nm_medicamento = "AJUDA",
                Dosagem_medicamento = "GOTA",
                Frequencia = "2 vezes",
                Qtd_dias = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/medicamentoes/criar/medicamento",novoMedicamento);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail($"Não foi criado por: {erro}");
            }

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);

            var medicamentoCriado =await response.Content.ReadFromJsonAsync<Medicamento>();

            Assert.NotNull(medicamentoCriado);

            Assert.Equal(id_medicamento,medicamentoCriado.Id_medicamento);
        }

        [Fact]
        public async Task AtualizarMedicamento_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_medicamento = 44;

            var medicamentoAtualizado = new
            {
                Id_medicamento = id_medicamento,
                Id_prescricao = 88,
                Nm_medicamento = "Raiva",
                Dosagem_medicamento = "GOTA",
                Frequencia = "2 vezes",
                Qtd_dias = 10
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/medicamentoes/atualizar/medicamento/{id_medicamento}",medicamentoAtualizado);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail($"Não foi atualizado por: {erro}");
            }


            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }

        [Fact]
        public async Task GetMedicamento_dados_RetornaOk()
        {
            // Arrange
            var id_medicamento = 44;

            // Act
            var response = await _client.GetAsync($"api/medicamentoes/relatorio/medicamento/{id_medicamento}");

            // Assert
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Fail("Id nao encontrado");
            }

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);

            var medicamento = await response.Content.ReadFromJsonAsync<Medicamento>();

            Assert.NotNull(medicamento);
            Assert.Equal(id_medicamento, medicamento.Id_medicamento);
        }

        [Fact]
        public async Task GetAllMedicamento_dados_RetornarOk()
        {
            //Act
            var response = await _client.GetAsync("api/medicamentoes/relatorio/medicamento");

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);

            var medicamentos = await response.Content.ReadFromJsonAsync<List<Medicamento>>();

            Assert.NotNull(medicamentos);
        }

        [Fact]
        public async Task DeleteMedicamento_dados_RetornaOk()
        {
            // Arrange
            var id_medicamento = 32;

            // Act
            var response = await _client.DeleteAsync($"api/medicamentoes/deleta/medicamento/{id_medicamento}");

            // Assert
            if (!response.IsSuccessStatusCode)
            {
                var erroAchado = await response.Content.ReadAsStringAsync();

                Assert.Fail($"Não foi deletado pelo morivo: {erroAchado}");
            }

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}