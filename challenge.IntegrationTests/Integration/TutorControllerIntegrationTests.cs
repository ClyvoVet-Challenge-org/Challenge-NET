using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace challenge.IntegrationTests.Integration
{
    public class TutorControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public TutorControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task GetTutor_Dados_RetornaCreated()
        {
            //Arrange
            var novotutor = new
            {
                Id_tutor = 1,
                Cpf_tutor = "123456789",
                Nm_tutor = "Leticia",
                Nr_telefone_tutor = "11987562335"
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/tutor", novotutor);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var tutorCriado = await response.Content.ReadFromJsonAsync<Tutor>();
            Assert.NotNull(tutorCriado);
            Assert.Equal(1, tutorCriado.Id_tutor);
        }

        [Fact]
        public async Task UpdateTutor_DadosValidos_RetornaSucesso()
        {
            //Arrange
            var id_tutor = 1;
            var tutorAtualizado = new
            {
                Id_tutor = id_tutor,
                Cpf_tutor = "123456789",
                Nm_tutor = "Leticia",
                Nr_telefone_tutor = "11987562335"
            };
            //Act
            var response = await _client.PostAsJsonAsync("api/tutor/atualizar/{id_tutor}", tutorAtualizado);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
