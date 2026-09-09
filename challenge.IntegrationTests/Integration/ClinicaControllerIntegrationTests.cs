using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    public class ClinicaControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public ClinicaControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task Getclinica_Dados_RetornaCreated()
        {
            //Arrange
            var novoclinica = new
            {
                Id_clinica = 1,
                Cnpj_clinica = "123456789",
                Nm_clinica = "PetSoule"
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/clinica", novoclinica);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var clinicaCriado = await response.Content.ReadFromJsonAsync<Clinica>();
            Assert.NotNull(clinicaCriado);
            Assert.Equal(1, clinicaCriado.Id_clinica);
        }

        [Fact]
        public async Task Updateclinica_DadosValidos_RetornaSucesso()
        {
            //Arrange
            var id_clinica = 1;
            var clinicaAtualizado = new
            {
                Id_clinica = id_clinica,
                Cnpj_clinica = "123456789",
                Nm_clinica = "PetSoule"
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/clinica/atualizar/{id_clinica}", clinicaAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
