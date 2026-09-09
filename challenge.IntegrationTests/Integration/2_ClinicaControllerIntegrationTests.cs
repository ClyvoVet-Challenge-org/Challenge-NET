using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
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
            var id_clinica = 9984;

            // Remove o cadastro anterior, caso exista
            await _client.DeleteAsync(
                $"api/clinicas/deleta/clinica/{id_clinica}");

            var novaClinica = new
            {
                Id_clinica = id_clinica,
                Cnpj_clinica = "9876543210111",
                Nm_clinica = "PetSoule"
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "api/clinicas/criar/clinica",
                novaClinica);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var clinicaCriada =
                await response.Content.ReadFromJsonAsync<Clinica>();

            Assert.NotNull(clinicaCriada);
            Assert.Equal("PetSoule", clinicaCriada.Nm_clinica);
        }

        [Fact]
        public async Task UpdateClinica_DadosValidos_RetornaSucesso()
        {
            // Arrange
            var id_clinica = 9998;

            // Remove o cadastro anterior, caso exista
            await _client.DeleteAsync(
                $"api/clinicas/deleta/clinica/{id_clinica}");

            var novaClinica = new
            {
                Id_clinica = id_clinica,
                Cnpj_clinica = "98765432100002",
                Nm_clinica = "ClinicaTeste"
            };

            var createResponse = await _client.PostAsJsonAsync(
                "api/clinicas/criar/clinica",
                novaClinica);

            createResponse.EnsureSuccessStatusCode();

            var clinicaAtualizada = new
            {
                Id_clinica = id_clinica,
                Cnpj_clinica = "98765432100002",
                Nm_clinica = "PetSoule"
            };

            // Act
            var response = await _client.PutAsJsonAsync(
                $"api/clinicas/atualizar/clinica/{id_clinica}",
                clinicaAtualizada);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}