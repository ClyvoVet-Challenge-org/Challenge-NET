using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
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

            // Garante que a clínica necessária exista
            await _client.DeleteAsync(
                "api/clinicas/deleta/clinica/9984");

            var novaClinica = new
            {
                Id_clinica = 9984,
                Cnpj_clinica = "9876543210111",
                Nm_clinica = "PetSoule"
            };

            var respostaClinica = await _client.PostAsJsonAsync(
                "api/clinicas/criar/clinica",
                novaClinica);

            respostaClinica.EnsureSuccessStatusCode();

            // Remove o endereço anterior, caso exista
            await _client.DeleteAsync(
                "api/enderecoclinicas/deleta/enderecoclinica/9997");

            var novoEnderecoClinica = new
            {
                Id_endereco_clinica = 9997,
                Pais = "Brasil",
                Estado = "São Paulo",
                Cidade = "São Paulo",
                Bairro = "Bairro Roxo",
                Logradouro_rua = "Dom Cachorro",
                Nr_rua = "1263",
                Complemento = "1212",
                Cep = "123456",
                Id_clinica = 9984
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "api/enderecoclinicas/criar/enderecoclinica",
                novoEnderecoClinica);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var enderecoCriado =
                await response.Content.ReadFromJsonAsync<EnderecoClinica>();

            Assert.NotNull(enderecoCriado);
            Assert.Equal("São Paulo", enderecoCriado.Cidade);
        }

        [Fact]
        public async Task AtualizarEnderecoClinica_DadosValidos_RetornaOk()
        {
            // Arrange

            // Garante que a clínica exista
            await _client.DeleteAsync(
                "api/clinicas/deleta/clinica/9984");

            var novaClinica = new
            {
                Id_clinica = 9984,
                Cnpj_clinica = "9876543210111",
                Nm_clinica = "PetSoule"
            };

            var respostaClinica = await _client.PostAsJsonAsync(
                "api/clinicas/criar/clinica",
                novaClinica);

            respostaClinica.EnsureSuccessStatusCode();

            // Garante que o endereço exista
            await _client.DeleteAsync(
                "api/enderecoclinicas/deleta/enderecoclinica/9997");

            var novoEndereco = new
            {
                Id_endereco_clinica = 9997,
                Pais = "Brasil",
                Estado = "São Paulo",
                Cidade = "São Paulo",
                Bairro = "Bairro Roxo",
                Logradouro_rua = "Dom Cachorro",
                Nr_rua = "1263",
                Complemento = "1212",
                Cep = "123456",
                Id_clinica = 9984
            };

            var respostaEndereco = await _client.PostAsJsonAsync(
                "api/enderecoclinicas/criar/enderecoclinica",
                novoEndereco);

            respostaEndereco.EnsureSuccessStatusCode();

            var enderecoAtualizado = new
            {
                Id_endereco_clinica = 9997,
                Pais = "Brasil",
                Estado = "São Paulo",
                Cidade = "São Paulo",
                Bairro = "Bairro Roxo",
                Logradouro_rua = "Rua Atualizada",
                Nr_rua = "1263",
                Complemento = "1212",
                Cep = "123456",
                Id_clinica = 9984
            };

            // Act
            var response = await _client.PutAsJsonAsync(
                "api/enderecoclinicas/atualizar/enderecoclinica/9997",
                enderecoAtualizado);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}