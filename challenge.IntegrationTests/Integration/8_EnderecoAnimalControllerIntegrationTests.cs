using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    //Atenção!!! Tanto o create quanto o update precisam ter cuidado com os dados que serão adicionados, porque, se não, pode dar erro.          
    //Recomendação faz um a cada vez, não faz eles tudo junto
    [Collection("ApiCollection")]
    public class _8EnderecoAnimalControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _8EnderecoAnimalControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task CriarEnderecoAnimal_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_endereco_animal = 2;
            var novoEnderecoAnimal = new
            {
                Id_endereco_animal = id_endereco_animal,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_animal = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/enderecoanimals/criar/enderecoanimal",novoEnderecoAnimal);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var enderecoAnimalCriado =await response.Content.ReadFromJsonAsync<EnderecoAnimal>();
            Assert.NotNull(enderecoAnimalCriado);
            Assert.Equal(2,enderecoAnimalCriado.Id_endereco_animal);
        }

        [Fact]
        public async Task UpdateEnderecoAnimal_DadosValidos_RetornaSucesso()
        {
            // Arrange
            var id_endereco_animal = 2;

            var enderecoAnimalAtualizado = new
            {
                Id_endereco_animal = id_endereco_animal,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_animal = 1
            };

            // Act
            var response = await _client.PutAsJsonAsync(
                $"api/enderecoanimals/atualizar/enderecoanimal/{id_endereco_animal}",
                enderecoAnimalAtualizado);
            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}