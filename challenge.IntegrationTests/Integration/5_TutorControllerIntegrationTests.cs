using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    [Collection("ApiCollection")]
    public class _5TutorControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _5TutorControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task CriarTutor_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_tutor = 2;

            await _client.DeleteAsync(
                $"api/tutor/deleta/Tutor/{id_tutor}");

            var novoTutor = new
            {
                Id_tutor = id_tutor,
                Cpf_tutor = "12356789",
                Nm_tutor = "Leticia",
                Nr_telefone_tutor = "11987562335"
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "api/tutor/criar/Tutor",
                novoTutor);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var tutorCriado =
                await response.Content.ReadFromJsonAsync<Tutor>();

            Assert.NotNull(tutorCriado);
            Assert.Equal("Leticia", tutorCriado.Nm_tutor);
        }

        [Fact]
        public async Task AtualizarTutor_DadosValidos_RetornaSucesso()
        {
            // Arrange
            var id_tutor = 1;

            var tutorAtualizado = new
            {
                Id_tutor = id_tutor,
                Cpf_tutor = "123456789",
                Nm_tutor = "Leticia",
                Nr_telefone_tutor = "11987562335"
            };

            // Act
            var response = await _client.PutAsJsonAsync(
                $"api/tutor/atualizar/Tutor/{id_tutor}",
                tutorAtualizado);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}