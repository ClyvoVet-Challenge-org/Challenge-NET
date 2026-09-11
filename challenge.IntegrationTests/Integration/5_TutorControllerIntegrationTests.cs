using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    //Atenção!!! Tanto o create quanto o update precisam ter cuidado com os dados que serão adicionados, porque, se não, pode dar erro.          
    //Recomendação faz um a cada vez, não faz eles tudo junto
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
            var id_tutor = Random.Shared.Next(1, 100);

            var novoTutor = new
            {
                Id_tutor = id_tutor,
                Cpf_tutor = "12356799",
                Nm_tutor = "Leticia",
                Nr_telefone_tutor = "11987562335"
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/tutor/criar/Tutor",novoTutor);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var tutorCriado = await response.Content.ReadFromJsonAsync<Tutor>();
            Assert.NotNull(tutorCriado);
            Assert.Equal(id_tutor, tutorCriado.Id_tutor);
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
            var response = await _client.PutAsJsonAsync($"api/tutor/atualizar/Tutor/{id_tutor}",tutorAtualizado);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetTutor_dados_RetornaOk()
        {
            //Arrage
            var id_Tutor = 1;

            //Act
            var response = await _client.GetAsync($"api/tutor/relatorio/Tutor/{id_Tutor}");

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var Tutor = await response.Content.ReadFromJsonAsync<Tutor>();

            Assert.NotNull(Tutor);
            Assert.Equal(id_Tutor, Tutor.Id_tutor);
        }

        [Fact]
        public async Task DeleteTutor_dados_RetornaOk()
        {
            // Arrange
            var id_Tutor = 1;

            // Act
            var response = await _client.DeleteAsync($"api/tutor/deleta/Tutor/{id_Tutor}");
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
