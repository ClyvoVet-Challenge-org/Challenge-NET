using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using challengeFiap.Infrastruture.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var loggMock = new Mock<ILogger<CarteiraVacinalsController>>();
            _logger = loggMock.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _controller = new CarteiraVacinalsController(_context, _logger, _CarteiraVacinalServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_CarteiraVacinal_RetornaOK()
        {
            // Arrange
            var animal = new Animal
            {
                Id_animal = 1,
                Rg_animal = "123456789",
                Nr_microchip_animal = "123123123",
                Nm_animal = "Rex",
                Dt_nascimento_animal = DateTime.Now,
                Peso_animal = 1,
                Especie_animal = "Cachorro",
                Raca_animal = "Labrador",
                Id_tutor = 1
            };
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            var id_carteiraVacinal = 1;
            var CarteiraVacinal = new CarteiraVacinal
            {
                Id_carteiraVacinal = id_carteiraVacinal,
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
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCarteiraVacinal = Assert.IsType<CarteiraVacinal>(createdResult.Value);
            
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

            _context.CarteiraVacinals.Add(CarteiraVacinal);
            await _context.SaveChangesAsync();

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

        [Fact]
        public async Task GetAll_CarteiraVacinal_RetornandoOk()
        {
            // Arrange
            var list = new List<CarteiraVacinal>
            {
                new CarteiraVacinal 
                { 
                    Id_carteiraVacinal = 1,
                    Nm_vacina = "raiva",
                    Dt_vacina_efetuada = DateTime.Now,
                    Dt_vacina_prevista = new DateTime(2026,9,9),
                    St_vacina = Domain.Enums.StatusVacinacao.EFETUADA,
                    Id_animal= 1,
                }
            };

            _CarteiraVacinalServiceMock.Setup(s => s.GetAllCarteiraVacinalAsync()).ReturnsAsync(list);

            // Act
            var resultado = await _controller.GetAllCarteiraVacinal();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var returnedList = Assert.IsType<List<CarteiraVacinal>>(okResult.Value);

            Assert.NotNull(returnedList);
            Assert.Single(returnedList);
        }

        [Fact]
        public async Task GetId_CarteiraVacinal_RetornandoOk()
        {
            // Arrange
            var id_carteira = 1;

            var carteira = new CarteiraVacinal
            {
                Id_carteiraVacinal = id_carteira,
                Nm_vacina = "raiva",
                Dt_vacina_efetuada = DateTime.Now,
                Dt_vacina_prevista = new DateTime(2026, 9, 9),
                St_vacina = Domain.Enums.StatusVacinacao.EFETUADA,
                Id_animal = 1,
            };

            _CarteiraVacinalServiceMock.Setup(s => s.GetCarteiraVacinalIdAsync(id_carteira)).ReturnsAsync(carteira);

            // Act
            var resultado = await _controller.GetCarteiraVacinal(id_carteira);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var returnedCarteira = Assert.IsType<CarteiraVacinal>(okResult.Value);

            Assert.NotNull(returnedCarteira);
            Assert.Equal(id_carteira, returnedCarteira.Id_carteiraVacinal);
        }

        [Fact]
        public async Task Deleta_CarteiraVacinal()
        {
            // Arrange
            var id_carteira = 1;

            var carteira = new CarteiraVacinal
            {
                Id_carteiraVacinal = id_carteira,
                Nm_vacina = "raiva",
                Dt_vacina_efetuada = DateTime.Now,
                Dt_vacina_prevista = new DateTime(2026, 9, 9),
                St_vacina = Domain.Enums.StatusVacinacao.EFETUADA,
                Id_animal = 1,
            };

            _CarteiraVacinalServiceMock.Setup(s => s.DeleteCarteiraVacinalAsync(id_carteira)).ReturnsAsync(carteira);

            _context.CarteiraVacinals.Add(carteira);
            await _context.SaveChangesAsync();

            // Act
            var r = await _controller.DeleteCarteiraVacinal(id_carteira);

            // Assert
            Assert.IsType<NoContentResult>(r);
        }

    }
}
