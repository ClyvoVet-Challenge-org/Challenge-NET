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
            var id_endereco_animal = Random.Shared.Next(1, 100);
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
                Id_animal = 80
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/enderecoanimals/criar/enderecoanimal",novoEnderecoAnimal);

            // Assert
            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                Assert.Fail($"Não foi criado por: {erro}");
            }


            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var enderecoAnimalCriado =await response.Content.ReadFromJsonAsync<EnderecoAnimal>();
            Assert.NotNull(enderecoAnimalCriado);
            Assert.Equal(id_endereco_animal,enderecoAnimalCriado.Id_endereco_animal);
        }

        [Fact]
        public async Task UpdateEnderecoAnimal_DadosValidos_RetornaSucesso()
        {
            // Arrange
            var id_endereco_animal = 49;

            var enderecoAnimalAtualizado = new
            {
                Id_endereco_animal = id_endereco_animal,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro azuk",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "45454",
                Id_animal = 26
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/enderecoanimals/atualizar/enderecoanimal/{id_endereco_animal}",enderecoAnimalAtualizado);
            // Assert
            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                Assert.Fail($"Não foi atualizado por: {erro}");
            }


            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetEnderecoAnimal_dados_RetornaOk()
        {
            // Arrange
            var id_endereco_animal = 11;

            // Act
            var response = await _client.GetAsync("api/enderecoanimals/relatorio/enderecoanimal/" + id_endereco_animal);

            // Assert
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Fail("Id nao encontrado");
            }

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var endereco = await response.Content.ReadFromJsonAsync<EnderecoAnimal>();

            Assert.NotNull(endereco);
            Assert.Equal(id_endereco_animal, endereco.Id_endereco_animal);
        }

        [Fact]
        public async Task GetAllEnderecoAnimal_dados_RetornarOk()
        {
            //Act
            var response = await _client.GetAsync("api/enderecoanimals/relatorio/enderecoanimal");

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var enderecos = await response.Content.ReadFromJsonAsync<List<EnderecoAnimal>>();

            Assert.NotNull(enderecos);
        }

        [Fact]
        public async Task DeleteEnderecoAnimal_dados_RetornaOk()
        {
            // Arrange
            var id_endereco_animal = 49;

            // Act
            var response = await _client.DeleteAsync($"api/enderecoanimals/deleta/enderecoanimal/{id_endereco_animal}");

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