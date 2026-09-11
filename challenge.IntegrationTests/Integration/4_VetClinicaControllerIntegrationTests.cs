using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    //Atenção!!! Tanto o create quanto o update precisam ter cuidado com os dados que serão adicionados, porque, se não, pode dar erro.          
    //Recomendação faz um a cada vez, não faz eles tudo junto
    [Collection("ApiCollection")]
    public class _4_VetClinicaControllerIntegration
    {
        private readonly HttpClient _client;

        public _4_VetClinicaControllerIntegration(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task CriarVetClinica_Dados_validos_RetornarOk()
        {
            var id_vetClinica = Random.Shared.Next(1, 100);

            var novoDadosDeVetClinica = new
            {
                id_clinica_vet = id_vetClinica,
                Id_vet = 2,
                Id_clinica = 43
            };

            //Act
            var response = await _client.PostAsJsonAsync("api/vetclinicas/criar/vetclinica", novoDadosDeVetClinica);
            //Assert
            var erro = await response.Content.ReadAsStringAsync();
            Assert.True(response.IsSuccessStatusCode, $"Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var novoVetClinica = await response.Content.ReadFromJsonAsync<VetClinica>();
            Assert.NotNull(novoVetClinica);
            Assert.Equal(id_vetClinica, novoVetClinica.Id_clinica_vet);


        }

        [Fact]
        public async Task AtualizarVetClinica_DadosValidos_RetornaOk()
        {
            // Arrange

            var id_vetClinica = 42;

            var novoDadosDeVetClinica = new
            {
                id_clinica_vet = id_vetClinica,
                Id_vet = 74,
                Id_clinica = 43
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/vetclinicas/atualizar/vetclinica/{id_vetClinica}",novoDadosDeVetClinica);
            // Assert
            var erro = await response.Content.ReadAsStringAsync();
            Assert.True(response.IsSuccessStatusCode, $"Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
