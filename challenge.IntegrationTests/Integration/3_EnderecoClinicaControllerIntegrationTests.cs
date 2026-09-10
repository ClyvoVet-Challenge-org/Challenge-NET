using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    //Atenção!!! Tanto o create quanto o update precisam ter cuidado com os dados que serão adicionados, porque, se não, pode dar erro.          
    //Recomendação faz um a cada vez, não faz eles tudo junto
    [Collection("ApiCollection")]
    public class _3EnderecoClinicaControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _3EnderecoClinicaControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CriarEnderecoClinica_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_endereco_clinica = Random.Shared.Next(1, 100);
            //cada Clinica dever ter o seu unico endereco
            var novoEnderecoClinica = new
            {
                Id_endereco_clinica = id_endereco_clinica,
                Pais = "Brasil",
                Estado = "São Paulo",
                Cidade = "São Paulo",
                Bairro = "Bairro rosa",
                Logradouro_rua = "Dom Cachorro",
                Nr_rua = "1273",
                Complemento = "1212",
                Cep = "123456",
                Id_clinica = 9999
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/enderecoclinicas/criar/enderecoclinica",novoEnderecoClinica);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();
            Assert.True(response.IsSuccessStatusCode, $"Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var enderecoCriado = await response.Content.ReadFromJsonAsync<EnderecoClinica>();

            Assert.NotNull(enderecoCriado);
            Assert.Equal("São Paulo", enderecoCriado.Cidade);
        }

        [Fact]
        public async Task AtualizarEnderecoClinica_DadosValidos_RetornaOk()
        {
            // Arrange

            var id_endereco_clinica = 39;

            var EnderecoClinicaAtualizado = new
            {
                Id_endereco_clinica = id_endereco_clinica,
                Pais = "Brasil",
                Estado = "São Paulo",
                Cidade = "São Paulo",
                Bairro = "Bairro Rosa",
                Logradouro_rua = "Dom Cachorro",
                Nr_rua = "1263",
                Complemento = "1212",
                Cep = "123456",
                Id_clinica = 43
            };
            // Act
            var response = await _client.PutAsJsonAsync($"api/enderecoclinicas/atualizar/enderecoclinica/{id_endereco_clinica}",EnderecoClinicaAtualizado);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();
            Assert.True(response.IsSuccessStatusCode, $"Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
