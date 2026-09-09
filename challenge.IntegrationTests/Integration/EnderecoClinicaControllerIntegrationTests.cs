using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    public class EnderecoClinicaControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public EnderecoClinicaControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task GetEnderecoClinica_Dados_RetornaCreated()
        {
            //Arrange
            var novoEnderecoClinica = new
            {
                id_endereco_clinica = 1,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_clinica = 1,
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/EnderecoClinica", novoEnderecoClinica);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var EnderecoClinicaCriado = await response.Content.ReadFromJsonAsync<EnderecoClinica>();
            Assert.NotNull(EnderecoClinicaCriado);
            Assert.Equal(1, EnderecoClinicaCriado.Id_endereco_clinica);
        }

        [Fact]
        public async Task UpdateEnderecoClinica_DadosValidos_RetornaSucesso()
        {
            //Arrange
            var id_EnderecoClinica = 1;
            var EnderecoClinicaAtualizado = new
            {
                Id_EnderecoClinica = id_EnderecoClinica,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_clinica = 1,
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/EnderecoClinica/atualizar/{id_EnderecoClinica}", EnderecoClinicaAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
