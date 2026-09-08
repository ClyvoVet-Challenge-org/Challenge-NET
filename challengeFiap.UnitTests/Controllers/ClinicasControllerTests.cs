using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using challengeFiap.Infrastruture.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.UnitTests.Controllers
{
    public class ClinicaControllerTests
    {

        private readonly Mock<IClinicaervice> _ClinicaServiceMock;

        private readonly ClinicaController _controller;
        private readonly ILogger<ClinicaController> _logger;
        private readonly AppDbContext _context;

        public ClinicaControllerTests()
        {
            _ClinicaServiceMock = new Mock<IClinicaervice>();
            _controller = new ClinicaController(_context, _logger, _ClinicaServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_Clinica_RetornaOK()
        {
            // Arrange
            var Clinica = new Clinica
            {
                Id_Clinica = 1,
                Rg_Clinica = "123456789",
                Nr_microchip_Clinica = "123123123",
                Nm_Clinica = "Rex",
                Dt_nascimento_Clinica = DateTime.Now,
                Peso_Clinica = 1,
                Especie_Clinica = "Cachorro",
                Raca_Clinica = "Labrador",
                Id_tutor = 1
            };
            _ClinicaServiceMock.Setup(service => service.CreateadAsync(Clinica))
                .ReturnsAsync(Clinica);
            // Act
            var result = await _controller.PostClinica(Clinica);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedClinica = Assert.IsType<Clinica>(okResult.Value);
            Assert.NotNull(returnedClinica);
        }

        //Atualização
        [Fact]
        public async Task Update_Clinica_RetornaOk()
        {
            //Arrenge
            var id_Clinica = 1;
            var Clinica = new Clinica
            {
                Id_Clinica = id_Clinica,
                Rg_Clinica = "123456789",
                Nr_microchip_Clinica = "123123123",
                Nm_Clinica = "mel",
                Dt_nascimento_Clinica = DateTime.Now,
                Peso_Clinica = 1,
                Especie_Clinica = "Cachorro",
                Raca_Clinica = "Labrador",
                Id_tutor = 1
            };

            _ClinicaServiceMock.Setup(service => service.UpdateClinicaAsync(id_Clinica, Clinica))
                .ReturnsAsync(Clinica);

            //Act
            var atualizacaoRealizada = await _controller.PutClinica(id_Clinica, Clinica);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoClinica = Assert.IsType<Clinica>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(Clinica.Id_Clinica, retornoClinica.Id_Clinica);
            Assert.Equal(Clinica.Rg_Clinica, retornoClinica.Rg_Clinica);
            Assert.Equal(Clinica.Nr_microchip_Clinica, retornoClinica.Nr_microchip_Clinica);
            Assert.Equal(Clinica.Nm_Clinica, retornoClinica.Nm_Clinica);
            Assert.Equal(Clinica.Dt_nascimento_Clinica, retornoClinica.Dt_nascimento_Clinica);
            Assert.Equal(Clinica.Peso_Clinica, retornoClinica.Peso_Clinica);
            Assert.Equal(Clinica.Especie_Clinica, retornoClinica.Especie_Clinica);
            Assert.Equal(Clinica.Raca_Clinica, retornoClinica.Raca_Clinica);
            Assert.Equal(Clinica.Id_tutor, retornoClinica.Id_tutor);
        }
    }
}
