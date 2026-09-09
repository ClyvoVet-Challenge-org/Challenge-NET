using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    public class CarteiraVacinalControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public CarteiraVacinalControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task GetcarteiraVacinal_Dados_RetornaCreated()
        {
            //Arrange
            var novocarteiraVacinal = new
            {
                Id_carteiraVacinal = 1,
                Nm_vacina = "raiva",
                Dt_vacina_efetuada = DateTime.Now,
                Dt_vacina_prevista = new DateTime(2026, 9, 9),
                St_vacina = challengeFiap.Domain.Enums.StatusVacinacao.EFETUADA,
                Id_animal = 1,
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/carteiraVacinal", novocarteiraVacinal);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var carteiraVacinalCriado = await response.Content.ReadFromJsonAsync<CarteiraVacinal>();
            Assert.NotNull(carteiraVacinalCriado);
            Assert.Equal(1, carteiraVacinalCriado.Id_carteiraVacinal);
        }

        [Fact]
        public async Task UpdatecarteiraVacinal_DadosValidos_RetornaSucesso()
        {
            //Arrange
            var id_carteiraVacinal = 1;
            var carteiraVacinalAtualizado = new
            {
                Id_carteiraVacinal = id_carteiraVacinal,
                Nm_vacina = "semanal",
                Dt_vacina_efetuada = DateTime.Now,
                Dt_vacina_prevista = new DateTime(2026, 9, 9),
                St_vacina = challengeFiap.Domain.Enums.StatusVacinacao.EFETUADA,
                Id_animal = 1
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/carteiraVacinal/atualizar/{id_carteiraVacinal}", carteiraVacinalAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
