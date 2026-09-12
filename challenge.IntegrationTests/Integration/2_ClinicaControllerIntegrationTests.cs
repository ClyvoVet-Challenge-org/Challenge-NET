using challenge.IntegrationTests.FactoryFixture;
using challengeFiap.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace challenge.IntegrationTests.Integration
{
    //Atenção!!! Tanto o create quanto o update precisam ter cuidado com os dados que serão adicionados, porque, se não, pode dar erro.          
    //Recomendação faz um a cada vez, não faz eles tudo junto
    [Collection("ApiCollection")]
    public class _2ClinicaControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public _2ClinicaControllerIntegrationTests(ApiFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateClinica_DadosValidos_RetornaOk()
        {
            // Arrange
            var id_clinica = Random.Shared.Next(1, 100);

            //Essa questão foi colocada aqui para evitar precisa mudar os dados o tempo todos para ver se ta funcionando o testes.
            await _client.DeleteAsync($"api/clinicas/deleta/clinica/{id_clinica}");

            var novaClinica = new
            {
                Id_clinica = id_clinica,
                Cnpj_clinica = "1233",
                Nm_clinica = "VeterinarioVet"
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/clinicas/criar/clinica",novaClinica);

            // Assert
            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                Assert.Fail($"Não foi criado por: {erro}");
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var clinicaCriada = await response.Content.ReadFromJsonAsync<Clinica>();
            Assert.NotNull(clinicaCriada);
        }

        [Fact]
        public async Task UpdateClinica_DadosValidos_RetornaSucesso()
        {
            // Arrange
            var id_clinica = 6;

            var clinicaAtualizada = new
            {
                Id_clinica = id_clinica,
                Cnpj_clinica = "122442",
                Nm_clinica = "PetSoule"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"api/clinicas/atualizar/clinica/{id_clinica}",clinicaAtualizada);

            // Assert
            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                Assert.Fail($"Não foi atualizado por: {erro}");
            }

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetClinica_dados_RetornaOk()
        {
            // Arrange
            var id_clinica = 6;

            // Act
            var response = await _client.GetAsync($"api/clinicas/relatorio/clinica/{id_clinica}");

            // Assert
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Assert.Fail("Id nao encontrado");
            }

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var clinica = await response.Content.ReadFromJsonAsync<Clinica>();

            Assert.NotNull(clinica);
            Assert.Equal(id_clinica, clinica.Id_clinica);
        }

        [Fact]
        public async Task GetAllClinica_dados_RetornarOk()
        {
            //Act
            var response = await _client.GetAsync("api/clinicas/relatorio/clinica");

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var clinicas = await response.Content.ReadFromJsonAsync<List<Clinica>>();

            Assert.NotNull(clinicas);
        }

        [Fact]
        public async Task DeleteClinica_dados_RetornaOk()
        {
            // Arrange
            var id_clinica = 32;

            // Act
            var response = await _client.DeleteAsync($"api/clinicas/deleta/clinica/{id_clinica}");

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
