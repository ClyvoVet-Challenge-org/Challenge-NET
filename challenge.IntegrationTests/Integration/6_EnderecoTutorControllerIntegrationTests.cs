using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    //Atenção!!! Tanto o create quanto o update precisam ter cuidado com os dados que serão adicionados, porque, se não, pode dar erro.          
    //Recomendação faz um a cada vez, não faz eles tudo junto
    [Collection("ApiCollection")]
    public class _6EnderecoTutorControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _6EnderecoTutorControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        //Parte de teste de cricao 
        [Fact]
        public async Task CriarEnderecoTutor_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_endereco_tutor = Random.Shared.Next(1, 100);

            var novoEnderecoTutor = new
            {
                Id_endereco_tutor = id_endereco_tutor,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_tutor = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/enderecotutors/criar/enderecoresponsavel",novoEnderecoTutor);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var enderecoTutorCriado = await response.Content.ReadFromJsonAsync<EnderecoTutor>();
            Assert.NotNull(enderecoTutorCriado);
            Assert.Equal(2,enderecoTutorCriado.Id_endereco_tutor);
        }

        [Fact]
        public async Task UpdateEnderecoTutor_DadosValidos_RetornaSucesso()
        {
            // Arrange
            var id_enderecoTutor = 2;

            var enderecoTutorAtualizado = new
            {
                Id_endereco_tutor = id_enderecoTutor,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_tutor = 1
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/enderecotutors/atualizar/enderecoresponsavel/{id_enderecoTutor}",enderecoTutorAtualizado);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
