using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    public class EnderecoTutorControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public EnderecoTutorControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task GetEnderecoTutor_Dados_RetornaCreated()
        {
            //Arrange
            var novoEnderecoTutor = new
            {
                Id_endereco_tutor = 1,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_tutor = 1,
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/EnderecoTutor", novoEnderecoTutor);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var EnderecoTutorCriado = await response.Content.ReadFromJsonAsync<EnderecoTutor>();
            Assert.NotNull(EnderecoTutorCriado);
            Assert.Equal(1, EnderecoTutorCriado.Id_endereco_tutor);
        }

        [Fact]
        public async Task UpdateEnderecoTutor_DadosValidos_RetornaSucesso()
        {
            //Arrange
            var id_EnderecoTutor = 1;
            var EnderecoTutorAtualizado = new
            {
                id_EnderecoTutor= id_EnderecoTutor,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_tutor = 1,
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/EnderecoTutor/atualizar/{id_EnderecoTutor}", EnderecoTutorAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
