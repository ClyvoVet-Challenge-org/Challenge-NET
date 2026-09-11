using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    //Atenção!!! Tanto o create quanto o update precisam ter cuidado com os dados que serão adicionados, porque, se não, pode dar erro.          
    //Recomendação faz um a cada vez, não faz eles tudo junto
    [Collection("ApiCollection")]
    public class _2ClinicaControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _2ClinicaControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateClinica_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_clinica = Random.Shared.Next(1, 100);

            //Essa questão foi colocada aqui para evitar precisa mudar os dados o tempo todos para ver se ta funcionando o testes.
            await _client.DeleteAsync($"api/clinicas/deleta/clinica/{id_clinica}");

            var novaClinica = new
            {
                Id_clinica = id_clinica,
                Cnpj_clinica = "987654321010",
                Nm_clinica = "PetSoule"
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/clinicas/criar/clinica",novaClinica);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var clinicaCriada = await response.Content.ReadFromJsonAsync<Clinica>();
            Assert.NotNull(clinicaCriada);
            Assert.Equal("PetSoule", clinicaCriada.Nm_clinica);
        }

        [Fact]
        public async Task UpdateClinica_DadosValidos_RetornaSucesso()
        {
            // Arrange
            var id_clinica = 9984;

            var clinicaAtualizada = new
            {
                Id_clinica = id_clinica,
                Cnpj_clinica = "98765432100002",
                Nm_clinica = "PetSoule"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/clinicas/atualizar/clinica/{id_clinica}",clinicaAtualizada);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


    }
}
