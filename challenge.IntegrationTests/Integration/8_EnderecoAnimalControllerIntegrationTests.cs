using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    public class _8EnderecoAnimalControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _8EnderecoAnimalControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task GetEnderecoAnimal_Dados_RetornaCreated()
        {
            //Arrange
            var novoEnderecoAnimal = new
            {
                Id_endereco_animal = 2,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_animal = 1,
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/enderecoanimals/criar/enderecoanimal", novoEnderecoAnimal);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var EnderecoAnimalCriado = await response.Content.ReadFromJsonAsync<EnderecoAnimal>();
            Assert.NotNull(EnderecoAnimalCriado);
            Assert.Equal(1, EnderecoAnimalCriado.Id_animal);
        }

        [Fact]
        public async Task UpdateEnderecoAnimal_DadosValidos_RetornaSucesso()
        {
            //Arrange
            var id_EnderecoAnimal = 1;
            var EnderecoAnimalAtualizado = new
            {
                Id_EnderecoAnimal = id_EnderecoAnimal,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_animal = 1,
            };
            //Act
            var response = await _client.PutAsJsonAsync($"api/enderecoanimals/atualizar/enderecoanimal/{id_EnderecoAnimal}", EnderecoAnimalAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}