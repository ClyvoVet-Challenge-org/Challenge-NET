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
    public class _7AnimalControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _7AnimalControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateAnimal_DadosValidos_RetornaCreated()
        {
            // Arrange
            var id_animal = Random.Shared.Next(1, 100);
            var novoAnimal = new
            {
                Id_animal = id_animal,
                Rg_animal = "987654321",
                Nr_microchip_animal = "987654321",
                Nm_animal = "Rex",
                Dt_nascimento_animal = DateTime.Now,
                Peso_animal = 1,
                Especie_animal = "Cachorro",
                Raca_animal = "Labrador",
                Id_tutor = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/animals/criar/animal",novoAnimal);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var animalCriado = await response.Content.ReadFromJsonAsync<Animal>();

            Assert.NotNull(animalCriado);
            Assert.Equal("Rex", animalCriado.Nm_animal);
        }

        [Fact]
        public async Task UpdateAnimal_DadosValidos_RetornaSucesso()
        {
            // Arrange
            var id_animal = 1;

            var animalAtualizado = new
            {
                Id_animal = id_animal,
                Rg_animal = "123456789",
                Nr_microchip_animal = "123123123",
                Nm_animal = "mel",
                Dt_nascimento_animal = DateTime.Now,
                Peso_animal = 1,
                Especie_animal = "Cachorro",
                Raca_animal = "Labrador",
                Id_tutor = 1
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/animals/atualizar/animal/{id_animal}",animalAtualizado);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAnimal_dados_RetornaOk()
        {
            //Arrage
            var id_Animal = 1;

            //Act
            var response = await _client.GetAsync($"api/animals/relatorio/animal/{id_Animal}");

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var animal = await response.Content.ReadFromJsonAsync<Animal>();

            Assert.NotNull(animal);
            Assert.Equal(id_Animal, animal.Id_animal);
        }

        [Fact]
        public async Task DeleteEnderecoTutor_dados_RetornaOk()
        {
            // Arrange
            var id_animal = 1;

            // Act
            var response = await _client.DeleteAsync($"api/animals/deleta/animal/{id_animal}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}