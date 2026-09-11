using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Enums;
using System;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    //Atenção!!! Tanto o create quanto o update precisam ter cuidado com os dados que serão adicionados, porque, se não, pode dar erro.          
    //Recomendação faz um a cada vez, não faz eles tudo junto
    [Collection("ApiCollection")]
    public class _9CarteiraVacinalControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _9CarteiraVacinalControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CriarCarteiraVacinal_DadosValidos_RetornaCreated()
        {
            //Arrange
            var id_carteiraVacinal = Random.Shared.Next(1, 100);

            var novaCarteiraVacinal = new
            {
                Id_carteiraVacinal = id_carteiraVacinal,
                Nm_vacina = "raiva",
                Dt_vacina_prevista = new DateTime(2027, 9, 9),
                Dt_vacina_efetuada = new DateTime(2026, 9, 9),
                St_vacina = StatusVacinacao.EFETUADA,
                Id_animal = 1
            };

            //Act
            var response = await _client.PostAsJsonAsync("api/carteiravacinals/criar/carteiravacinal",novaCarteiraVacinal);

            //Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var carteiraVacinalCriada = await response.Content.ReadFromJsonAsync<CarteiraVacinal>();

            Assert.NotNull(carteiraVacinalCriada);

            Assert.Equal(id_carteiraVacinal,carteiraVacinalCriada.Id_carteiraVacinal);
        }

        [Fact]
        public async Task AtualizarCarteiraVacinal_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_carteiraVacinal = 2;

            var carteiraVacinalAtualizada = new
            {
                Id_carteiraVacinal = id_carteiraVacinal,
                Nm_vacina = "semanal",
                Dt_vacina_prevista = new DateTime(2027, 9, 9),
                Dt_vacina_efetuada = new DateTime(2026, 9, 9),
                St_vacina = StatusVacinacao.EFETUADA,
                Id_animal = 1
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/carteiravacinals/atualizar/carteiravacinal/{id_carteiraVacinal}", carteiraVacinalAtualizada);

            // Assert
            var erro = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Ocorrido da rejeicao: {erro}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

         }
        [Fact]
        public async Task GetCarteiraVacinal_dados_RetornaOk()
        {
            //Arrage
            var id_CarteiraVacinal = 1;

            //Act
            var response = await _client.GetAsync($"api/carteiraVacinals/relatorio/carteiravacinal/{id_CarteiraVacinal}");
            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var CarteiraVacinal = await response.Content.ReadFromJsonAsync<CarteiraVacinal>();

            Assert.NotNull(CarteiraVacinal);
            Assert.Equal(id_CarteiraVacinal, CarteiraVacinal.Id_carteiraVacinal);
        }

        [Fact]
        public async Task DeleteCarteiraVacinal_dados_RetornaOk()
        {
            // Arrange
            var id_CarteiraVacinal = 1;

            // Act
            var response = await _client.DeleteAsync($"api/carteiraVacinals/deleta/carteiravacinal/{id_CarteiraVacinal}");
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}