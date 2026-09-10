using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
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

        public async Task CriarVetClinica_Dados_validos_RetornarOk()
        {
            var id_vetClinica = 1;

            var novoDadosDeVetClinica = new
            {
                id_clinica_vet = id_vetClinica,
                Id_vet = 1,
                Id_clinica = 1
            };

            //Act
            var response = await _client.PostAsJsonAsync("api/vetclinicas/criar/vetclinica", novoDadosDeVetClinica);
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var novoVetClinica = await response.Content.ReadFromJsonAsync<VetClinica>();
            Assert.NotNull(novoVetClinica);
            Assert.Equal(1, novoVetClinica.Id_clinica_vet);


        }
    }
}
