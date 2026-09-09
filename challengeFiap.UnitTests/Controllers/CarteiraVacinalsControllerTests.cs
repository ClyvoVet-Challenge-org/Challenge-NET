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
                Id_carteiraVacinal = 1,
                Nm_vacina = "raiva",
                Dt_vacina_efetuada = DateTime.Now,
                Dt_vacina_prevista = new DateTime(2026,9,9),
                St_vacina = Domain.Enums.StatusVacinacao.EFETUADA,
                Id_animal= 1,
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
                Id_carteiraVacinal = 1,
                Nm_vacina = "semanal",
                Dt_vacina_efetuada = DateTime.Now,
                Dt_vacina_prevista = new DateTime(2026, 9, 9),
                St_vacina = Domain.Enums.StatusVacinacao.EFETUADA,
                Id_animal = 1
            };

            _CarteiraVacinalServiceMock.Setup(service => service.UpdateCarteiraVacinalAsync(id_CarteiraVacinal, CarteiraVacinal))
                .ReturnsAsync(CarteiraVacinal);

            //Act
            var atualizacaoRealizada = await _controller.PutCarteiraVacinal(id_CarteiraVacinal, CarteiraVacinal);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoCarteiraVacinal = Assert.IsType<CarteiraVacinal>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(CarteiraVacinal.Id_carteiraVacinal, retornoCarteiraVacinal.Id_carteiraVacinal);
            Assert.Equal(CarteiraVacinal.Nm_vacina, retornoCarteiraVacinal.Nm_vacina);
            Assert.Equal(CarteiraVacinal.Dt_vacina_efetuada, retornoCarteiraVacinal.Dt_vacina_efetuada);
            Assert.Equal(CarteiraVacinal.Dt_vacina_prevista, retornoCarteiraVacinal.Dt_vacina_prevista);
            Assert.Equal(CarteiraVacinal.St_vacina, retornoCarteiraVacinal.St_vacina);
            Assert.Equal(CarteiraVacinal.Id_animal, retornoCarteiraVacinal.Id_animal);
        }
    }
}
