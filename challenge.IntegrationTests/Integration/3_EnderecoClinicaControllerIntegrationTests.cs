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
                Nr_rua = "11221",
                Complemento = "454",
                Cep = "454554",
                Id_clinica = 3
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/enderecoclinicas/criar/enderecoclinica",novoEnderecoClinica);

            // Assert
            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                Assert.Fail($"Não foi criado por: {erro}");
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var enderecoCriado = await response.Content.ReadFromJsonAsync<EnderecoClinica>();

            Assert.NotNull(enderecoCriado);
        }

        [Fact]
        public async Task AtualizarEnderecoClinica_DadosValidos_RetornaOk()
        {
            // Arrange

            var id_endereco_clinica = 87;

            var EnderecoClinicaAtualizado = new
            {
                Id_endereco_clinica = id_endereco_clinica,
                Pais = "Brasil",
                Estado = "São Paulo",
                Cidade = "campinhas",
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
            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                Assert.Fail($"Não foi atualizado por: {erro}");
            }

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetEnderecoClinica_dados_RetornaOk()
        {
            // Arrange
            var id_endereco_clinica = 21;

            // Act
            var response = await _client.GetAsync($"api/enderecoclinicas/relatorio/enderecoclinica/{id_endereco_clinica}");

            // Assert
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Fail("Id nao encontrado");
            }

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var endereco = await response.Content.ReadFromJsonAsync<EnderecoClinica>();

            Assert.NotNull(endereco);
            Assert.Equal(id_endereco_clinica, endereco.Id_endereco_clinica);
        }

        [Fact]
        public async Task GetAllEnderecoClinica_dados_RetornarOk()
        {
            //Act
            var response = await _client.GetAsync("api/enderecoclinicas/relatorio/enderecoclinica");

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var enderecos = await response.Content.ReadFromJsonAsync<List<EnderecoClinica>>();

            Assert.NotNull(enderecos);
        }

        [Fact]
        public async Task DeleteEnderecoClinica_dados_RetornaOk()
        {
            // Arrange
            var id_endereco_clinica = 99;

            // Act
            var response = await _client.DeleteAsync($"api/enderecoclinicas/deleta/enderecoclinica/{id_endereco_clinica}");

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
