using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    //Atenção!!! Tanto o create quanto o update precisam ter cuidado com os dados que serão adicionados, porque, se não, pode dar erro.          
    //Recomendação faz um a cada vez, não faz eles tudo junto
    [Collection("ApiCollection")]
    public class _1VetControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _1VetControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateVeterinario_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_vet = Random.Shared.Next(1,100);
            
            var novoVeterinario = new
            {
                Id_vet = id_vet,
                Nm_vet = "lual",
                Cpf_vet = "98745432111",
                Crmv_vet = "999949-SP",
                Email_vet = "luall9991@gmail.com",
                Senha_vet = "21358"
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/veterinarios/criar/veterinario",novoVeterinario);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Ocorrido da rejeicao: {erro}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var veterinarioCriado = await response.Content.ReadFromJsonAsync<Veterinario>();

            Assert.NotNull(veterinarioCriado);
            Assert.Equal("lual", veterinarioCriado.Nm_vet);
        }

        [Fact]
        public async Task UpdateVeterinario_DadosValidos_RetornaSucesso()
        {
            // Arrange
            var id_veterinario = 1;

            var veterinarioAtualizado = new
            {
                Id_vet = id_veterinario,
                Nm_vet = "nome",
                Cpf_vet = "123123123",
                Crmv_vet = "12345556",
                Email_vet = "sagafdf@gmail.com",
                Senha_vet = "21358"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/veterinarios/atualizar/veterinario/{id_veterinario}",veterinarioAtualizado);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetVet_dados_RetornaOk()
        {
            //Arrage
            var id_veterinario = 1;

            //Act
            var response = await _client.GetAsync($"api/veterinarios/relatorio/veterinario/{id_veterinario}");

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK , response.StatusCode);
            var vet = await response.Content.ReadFromJsonAsync<Veterinario>();

            Assert.NotNull( vet );
            Assert.Equal(id_veterinario, vet.Id_vet);
        }

        [Fact]
        public async Task DeleteVet_dados_RetornaOk()
        {
            // Arrange
            var id_veterinario = 1;

            // Act
            var response = await _client.DeleteAsync($"api/veterinarios/deleta/veterinario/{id_veterinario}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
