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
    public class CarteiraVacinalsControllerTests
    {

        private readonly Mock<ICarteiraVacinalService> _CarteiraVacinalServiceMock;

        private readonly CarteiraVacinalsController _controller;
        private readonly ILogger<CarteiraVacinalsController> _logger;
        private readonly AppDbContext _context;

        public CarteiraVacinalsControllerTests()
        {
            _CarteiraVacinalServiceMock = new Mock<ICarteiraVacinalService>();
            _controller = new CarteiraVacinalsController(_context, _logger, _CarteiraVacinalServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_CarteiraVacinal_RetornaOK()
        {
            // Arrange
            var CarteiraVacinal = new CarteiraVacinal
            {
                Id_CarteiraVacinal = 1,
                Rg_CarteiraVacinal = "123456789",
                Nr_microchip_CarteiraVacinal = "123123123",
                Nm_CarteiraVacinal = "Rex",
                Dt_nascimento_CarteiraVacinal = DateTime.Now,
                Peso_CarteiraVacinal = 1,
                Especie_CarteiraVacinal = "Cachorro",
                Raca_CarteiraVacinal = "Labrador",
                Id_tutor = 1
            };
            _CarteiraVacinalServiceMock.Setup(service => service.CreateCarteiraVacinalAsync(CarteiraVacinal))
                .ReturnsAsync(CarteiraVacinal);
            // Act
            var result = await _controller.PostCarteiraVacinal(CarteiraVacinal);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCarteiraVacinal = Assert.IsType<CarteiraVacinal>(okResult.Value);
            Assert.NotNull(returnedCarteiraVacinal);
        }

        //Atualização
        [Fact]
        public async Task Update_CarteiraVacinal_RetornaOk()
        {
            //Arrenge
            var id_CarteiraVacinal = 1;
            var CarteiraVacinal = new CarteiraVacinal
            {
                Id_CarteiraVacinal = id_CarteiraVacinal,
                Rg_CarteiraVacinal = "123456789",
                Nr_microchip_CarteiraVacinal = "123123123",
                Nm_CarteiraVacinal = "mel",
                Dt_nascimento_CarteiraVacinal = DateTime.Now,
                Peso_CarteiraVacinal = 1,
                Especie_CarteiraVacinal = "Cachorro",
                Raca_CarteiraVacinal = "Labrador",
                Id_tutor = 1
            };

            _CarteiraVacinalServiceMock.Setup(service => service.UpdateCarteiraVacinalAsync(id_CarteiraVacinal, CarteiraVacinal))
                .ReturnsAsync(CarteiraVacinal);

            //Act
            var atualizacaoRealizada = await _controller.PutCarteiraVacinal(id_CarteiraVacinal, CarteiraVacinal);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoCarteiraVacinal = Assert.IsType<CarteiraVacinal>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(CarteiraVacinal.Id_CarteiraVacinal, retornoCarteiraVacinal.Id_CarteiraVacinal);
            Assert.Equal(CarteiraVacinal.Rg_CarteiraVacinal, retornoCarteiraVacinal.Rg_CarteiraVacinal);
            Assert.Equal(CarteiraVacinal.Nr_microchip_CarteiraVacinal, retornoCarteiraVacinal.Nr_microchip_CarteiraVacinal);
            Assert.Equal(CarteiraVacinal.Nm_CarteiraVacinal, retornoCarteiraVacinal.Nm_CarteiraVacinal);
            Assert.Equal(CarteiraVacinal.Dt_nascimento_CarteiraVacinal, retornoCarteiraVacinal.Dt_nascimento_CarteiraVacinal);
            Assert.Equal(CarteiraVacinal.Peso_CarteiraVacinal, retornoCarteiraVacinal.Peso_CarteiraVacinal);
            Assert.Equal(CarteiraVacinal.Especie_CarteiraVacinal, retornoCarteiraVacinal.Especie_CarteiraVacinal);
            Assert.Equal(CarteiraVacinal.Raca_CarteiraVacinal, retornoCarteiraVacinal.Raca_CarteiraVacinal);
            Assert.Equal(CarteiraVacinal.Id_tutor, retornoCarteiraVacinal.Id_tutor);
        }
    }
}
