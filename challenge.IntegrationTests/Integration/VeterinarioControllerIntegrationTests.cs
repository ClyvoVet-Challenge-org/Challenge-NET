using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    public class VeterinarioControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public VeterinarioControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Getveterinario_Dados_RetornaCreated()
        {
            //Arrange
            var novoveterinario = new
            {
                Id_vet = 1,
                Nm_vet = "lual",
                Cpf_vet = "123123123",
                Crmv_vet = "12345556",
                Email_vet = "sagafdf@gmail.com",
                Senha_vet = "21358"
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/veterinario", novoveterinario);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var veterinarioCriado = await response.Content.ReadFromJsonAsync<Veterinario>();
            Assert.NotNull(veterinarioCriado);
            Assert.Equal("lual", veterinarioCriado.Nm_vet);
        }

        [Fact]
        public async Task Updateveterinario_DadosValidos_RetornaSucesso()
        {
            //Arrange
            var id_veterinario = 1;
            var veterinarioAtualizado = new
            {
                Id_veterinario= id_veterinario,
                Nm_vet = "lual",
                Cpf_vet = "123123123",
                Crmv_vet = "12345556",
                Email_vet = "sagafdf@gmail.com",
                Senha_vet = "21358"
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/veterinario/atualizar/{id_veterinario}", veterinarioAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
