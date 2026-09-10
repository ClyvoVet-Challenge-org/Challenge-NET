using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    //Atenção!!! Tanto o create quanto o update precisam ter cuidado com os dados que serão adicionados, porque, se não, pode dar erro.          
    //Recomendação faz um a cada vez, não faz eles tudo junto
    [Collection("ApiCollection")]
    public class _92ConsultaControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _92ConsultaControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CriarConsulta_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_consulta = Random.Shared.Next(1, 100);

            var novaConsulta = new
            {
                Id_consulta = id_consulta,
                Historico_consulta = "Foi bom o resultado",
                St_consulta = challengeFiap.Domain.Enums.StatusConsulta.passada,
                Dt_consulta = new DateTime(2026, 9, 9),
                Id_vet = 1,
                Id_animal = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/consultas/criar/consulta",novaConsulta);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var consultaCriada = await response.Content.ReadFromJsonAsync<Consulta>();

            Assert.NotNull(consultaCriada);
            Assert.Equal(id_consulta,consultaCriada.Id_consulta);
        }

        [Fact]
        public async Task AtualizarConsulta_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_consulta = 2;

            var consultaAtualizada = new
            {
                Id_consulta = id_consulta,
                Historico_consulta = "Foi bom o resultado",
                St_consulta = challengeFiap.Domain.Enums.StatusConsulta.passada,
                Dt_consulta = new DateTime(2026, 9, 9),
                Id_vet = 1,
                Id_animal = 1
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/consultas/atualizar/consulta/{id_consulta}",consultaAtualizada);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Ocorrido da rejeicao: {erro}");


            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}