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
                Estado = "rj",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_tutor = 2
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/enderecotutors/criar/enderecoresponsavel",novoEnderecoTutor);

            // Assert
            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                Assert.Fail($"Não foi criado por: {erro}");
            }

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var enderecoTutorCriado = await response.Content.ReadFromJsonAsync<EnderecoTutor>();
            Assert.NotNull(enderecoTutorCriado);
            Assert.Equal(id_endereco_tutor,enderecoTutorCriado.Id_endereco_tutor);
        }

        [Fact]
        public async Task UpdateEnderecoTutor_DadosValidos_RetornaSucesso()
        {
            // Arrange
            var id_enderecoTutor = 35;

            var enderecoTutorAtualizado = new
            {
                Id_endereco_tutor = id_enderecoTutor,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "nao sei",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_tutor = 2
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/enderecotutors/atualizar/enderecoresponsavel/{id_enderecoTutor}",enderecoTutorAtualizado);

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
        public async Task GetEnderecoTutor_dados_RetornaOk()
        {
            // Arrange
            var id_endereco_tutor = 35;

            // Act
            var response = await _client.GetAsync($"api/enderecotutors/relatorio/enderecoresponsavel/{id_endereco_tutor}");

            // Assert
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Fail("Id nao encontrado");
            }

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var endereco = await response.Content.ReadFromJsonAsync<EnderecoTutor>();

            Assert.NotNull(endereco);
            Assert.Equal(id_endereco_tutor, endereco.Id_endereco_tutor);
        }

        [Fact]
        public async Task GetAllEnderecoTutor_dados_RetornarOk()
        {
            //Act
            var response = await _client.GetAsync("api/enderecotutors/relatorio/enderecoresponsavel");

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var enderecos = await response.Content.ReadFromJsonAsync<List<EnderecoTutor>>();

            Assert.NotNull(enderecos);
        }

        [Fact]
        public async Task DeleteEnderecoTutor_dados_RetornaOk()
        {
            // Arrange
            var id_endereco_tutor = 62;

            // Act
            var response = await _client.DeleteAsync($"api/enderecotutors/deleta/enderecoresponsavel/{id_endereco_tutor}");

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
