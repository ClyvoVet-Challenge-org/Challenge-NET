using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Enums;
using System.Collections.Generic;
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
                Dt_vacina_efetuada = new DateTime(2026, 9, 20),
                St_vacina = StatusVacinacao.PENDENTE,
                Id_animal = 80
            };

            //Act
            var response = await _client.PostAsJsonAsync("api/carteiravacinals/criar/carteiravacinal",novaCarteiraVacinal);

            //Assert
            var erro = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail($"Não foi criado por: {erro}");
            }

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
            var id_carteiraVacinal = 34;

            var carteiraVacinalAtualizada = new
            {
                Id_carteiraVacinal = id_carteiraVacinal,
                Nm_vacina = "semanal",
                Dt_vacina_prevista = new DateTime(2027, 9, 9),
                Dt_vacina_efetuada = new DateTime(2026, 9, 9),
                St_vacina = StatusVacinacao.EFETUADA,
                Id_animal = 80
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/carteiravacinals/atualizar/carteiravacinal/{id_carteiraVacinal}", carteiraVacinalAtualizada);

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
        public async Task GetCarteiraVacinal_dados_RetornaOk()
        {
            // Arrange
            var id_carteiraVacinal = 34;

            // Act
            var response = await _client.GetAsync($"api/carteiravacinals/relatorio/carteiravacinal/{id_carteiraVacinal}");

            // Assert
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Fail("Id nao encontrado");
            }

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var carteira = await response.Content.ReadFromJsonAsync<CarteiraVacinal>();

            Assert.NotNull(carteira);
            Assert.Equal(id_carteiraVacinal, carteira.Id_carteiraVacinal);
        }

        [Fact]
        public async Task GetAll_dados_RetornarOk()
        {
            // Act
            var response = await _client.GetAsync("api/carteiravacinals/relatorio/carteiravacinal");

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var lista = await response.Content.ReadFromJsonAsync<List<CarteiraVacinal>>();

            Assert.NotNull(lista);
        }

        [Fact]
        public async Task DeleteCarteiraVacinal_dados_RetornaOk()
        {
            // Arrange
            var id_carteiraVacinal = 38;

            // Act
            var response = await _client.DeleteAsync($"api/carteiravacinals/deleta/carteiravacinal/{id_carteiraVacinal}");

            // Assert
            var erroAchado = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}