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
    public class ClinicaControllerTests
    {

        private readonly Mock<IClinicaService> _ClinicaServiceMock;

        private readonly ClinicasController _controller;
        private readonly ILogger<ClinicasController> _logger;
        private readonly AppDbContext _context;

        public ClinicaControllerTests()
        {
            _ClinicaServiceMock = new Mock<IClinicaService>();
            var loggMock = new Mock<ILogger<ClinicasController>>();
            _logger = loggMock.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _controller = new ClinicasController(_context, _logger, _ClinicaServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_Clinica_RetornaOK()
        {
            // Arrange
            var Clinica = new Clinica
            {
                Id_clinica = 1,
                Cnpj_clinica = "123456789",
                Nm_clinica = "PetSoule"
            };
            _ClinicaServiceMock.Setup(service => service.CreateadAsync(Clinica))
                .ReturnsAsync(Clinica);
            // Act
            var result = await _controller.PostClinica(Clinica);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
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
                Id_clinica = id_Clinica,
                Cnpj_clinica = "123456789",
                Nm_clinica = "PetSoule"
            };

            _context.Clinicas.Add(Clinica);
            await _context.SaveChangesAsync();

            _ClinicaServiceMock.Setup(service => service.UpdateClinicaAsync(id_Clinica, Clinica))
                .ReturnsAsync(Clinica);

            //Act
            var atualizacaoRealizada = await _controller.PutClinica(id_Clinica, Clinica);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoClinica = Assert.IsType<Clinica>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(Clinica.Id_clinica, retornoClinica.Id_clinica);
            Assert.Equal(Clinica.Cnpj_clinica, retornoClinica.Cnpj_clinica);
            Assert.Equal(Clinica.Nm_clinica, retornoClinica.Nm_clinica);

        }
        [Fact]
        public async Task GetId_Animal_RetornandoOk()
        {
            // Arrange
            var id_animal = 1;

            var animal = new Animal
            {
                Id_animal = id_animal,
                Rg_animal = "123456789",
                Nr_microchip_animal = "123123123",
                Nm_animal = "Rex",
                Dt_nascimento_animal = DateTime.Now,
                Peso_animal = 1,
                Especie_animal = "Cachorro",
                Raca_animal = "Labrador",
                Id_tutor = 1
            };

            _animalServiceMock
                .Setup(s => s.GetAnimalIdAsync(id_animal))
                .ReturnsAsync(animal);

            // Act
            var resultado = await _controller.GetAnimal(id_animal);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var returnedAnimal = Assert.IsType<Animal>(okResult.Value);

            Assert.NotNull(returnedAnimal);
            Assert.Equal(id_animal, returnedAnimal.Id_animal);
        }
        [Fact]
        public async Task Deleta_animal()
        {
            // Arrange
            var id_animal = 1;

            var animal = new Animal
            {
                Id_animal = id_animal,
                Rg_animal = "123456789",
                Nr_microchip_animal = "123123123",
                Nm_animal = "Rex",
                Dt_nascimento_animal = DateTime.Now,
                Peso_animal = 1,
                Especie_animal = "Cachorro",
                Raca_animal = "Labrador",
                Id_tutor = 1
            };

            _animalServiceMock
                .Setup(s => s.DeleteAnimalAsync(id_animal))
                .ReturnsAsync(animal);

            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            //Act
            var r = await _controller.DeleteAnimal(id_animal);

            //Assert
            Assert.IsType<NoContentResult>(r);
        }
    }
}
