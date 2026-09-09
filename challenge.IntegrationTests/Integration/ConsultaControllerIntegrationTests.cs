using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    public class ConsultaControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public ConsultaControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task Getconsulta_Dados_RetornaCreated()
        {
            //Arrange
            var novoconsulta = new
            {
                Id_consulta = 1,
                Historico_consulta = "Foi bom o resultado",
                St_consulta = challengeFiap.Domain.Enums.StatusConsulta.passada,
                Dt_consulta = DateTime.Now,
                Id_vet = 1,
                Id_animal = 1
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/consulta", novoconsulta);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var consultaCriado = await response.Content.ReadFromJsonAsync<Consulta>();
            Assert.NotNull(consultaCriado);
            Assert.Equal(1, consultaCriado.Id_consulta);
        }

        [Fact]
        public async Task Updateconsulta_DadosValidos_RetornaSucesso()
        {
            //Arrange
            var id_consulta = 1;
            var consultaAtualizado = new
            {
                Id_consulta = id_consulta,
                Historico_consulta = "Foi bom o resultado",
                St_consulta = challengeFiap.Domain.Enums.StatusConsulta.passada,
                Dt_consulta = DateTime.Now,
                Id_vet = 1,
                Id_animal = 1,
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/consulta/atualizar/{id_consulta}", consultaAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
