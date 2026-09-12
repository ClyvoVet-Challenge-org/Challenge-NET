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
                St_consulta = challengeFiap.Domain.Enums.StatusConsulta.futura,
                Dt_consulta = new DateTime(2026, 8, 9),
                Id_vet = 45,
                Id_animal = 80
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/consultas/criar/consulta",novaConsulta);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail($"Não foi criado por: {erro}");
            }

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
            var id_consulta = 12;

            var consultaAtualizada = new
            {
                Id_consulta = id_consulta,
                Historico_consulta = "Foi bom o resultado",
                St_consulta = challengeFiap.Domain.Enums.StatusConsulta.cancelada,
                Dt_consulta = new DateTime(2026, 10, 12),
                Id_vet = 45,
                Id_animal = 80
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/consultas/atualizar/consulta/{id_consulta}",consultaAtualizada);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail($"Não foi atualizado por: {erro}");
            }


            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetConsulta_dados_RetornaOk()
        {
            // Arrange
            var id_consulta = 61;

            // Act
            var response = await _client.GetAsync($"api/consultas/relatorio/consulta/{id_consulta}");

            // Assert
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Fail("Id nao encontrado");
            }

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var consulta = await response.Content.ReadFromJsonAsync<Consulta>();

            Assert.NotNull(consulta);
            Assert.Equal(id_consulta, consulta.Id_consulta);
        }

        [Fact]
        public async Task GetAllConsulta_dados_RetornarOk()
        {
            //Act
            var response = await _client.GetAsync("api/consultas/relatorio/consulta");

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var consultas = await response.Content.ReadFromJsonAsync<List<Consulta>>();

            Assert.NotNull(consultas);
        }

        [Fact]
        public async Task DeleteConsulta_dados_RetornaOk()
        {
            // Arrange
            var id_consulta = 12;

            // Act
            var response = await _client.DeleteAsync($"api/consultas/deleta/consulta/{id_consulta}");

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