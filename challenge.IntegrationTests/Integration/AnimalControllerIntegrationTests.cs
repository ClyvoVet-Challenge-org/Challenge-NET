using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    [Collection("ApiCollection")]
    public class AnimalControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public AnimalControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateAnimal_DadosValidos_RetornaCreated()
        {
            //Arrange
            var novoAnimal = new
            {
                Id_animal = 1,
                Rg_animal = "123456789",
                Nr_microchip_animal = "123123123",
                Nm_animal = "Rex",
                Dt_nascimento_animal = DateTime.Now,
                Peso_animal = 1,
                Especie_animal = "Cachorro",
                Raca_animal = "Labrador",
                Id_tutor = 1
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/animal", novoAnimal);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var animalCriado = await response.Content.ReadFromJsonAsync<Animal>();
            Assert.NotNull(animalCriado);
            Assert.Equal("Rex", animalCriado.Nm_animal);
        }

        [Fact]
        public async Task UpdateAnimal_DadosValidos_RetornaSucesso()
        {
            //Arrange
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
            //Act
            var response = await _client.PostAsJsonAsync("api/animal/atualizar/{id_animal}", animalAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAnimal_IdIxiste_RetornaNotFound()
        {
            // Arrange
            var id_animal = 1;

            // Act
            var response = await _client.GetAsync(
                $"api/animals/relatorio/animal/{id_animal}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

    
