using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    public class VetClinicaControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public VetClinicaControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task GetVetClinica_Dados_RetornaCreated()
        {
            //Arrange
            var novovetClinica = new
            {
                Id_clinica_vet = 1,
                Id_vet = 1,
                Id_clinica = 1,
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/vetClinica", novovetClinica);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var vetClinicaCriado = await response.Content.ReadFromJsonAsync<VetClinica>();
            Assert.NotNull(vetClinicaCriado);
            Assert.Equal(1, vetClinicaCriado.Id_clinica_vet);
        }

        [Fact]
        public async Task UpdateVetClinica_DadosValidos_RetornaSucesso()
        {
            //Arrange
            var id_vetClinica = 1;
            var vetClinicaAtualizado = new
            {
                Id_vetClinica = id_vetClinica,
                Id_vet = 1,
                Id_clinica = 1,
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/vetClinica/atualizar/{id_vetClinica}", vetClinicaAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
