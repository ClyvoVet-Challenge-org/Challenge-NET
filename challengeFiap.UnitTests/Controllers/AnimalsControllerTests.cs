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
    public class AnimalsControllerTests
    {
        private readonly Mock<IAnimalService> _animalServiceMock;
        private readonly AnimalsController _controller;
        private readonly ILogger<AnimalsController> _logger;
        private readonly AppDbContext _context;

        public AnimalsControllerTests()
        {
            _animalServiceMock = new Mock<IAnimalService>();
            var loggMock = new Mock<ILogger<AnimalsController>>();
            _logger = loggMock.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _controller = new AnimalsController(_context, _logger, _animalServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_Animal_RetornaOK()
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
            _animalServiceMock.Setup(service => service.CreateAnimalAsync(animal))
                .ReturnsAsync(animal);
            // Act
            var result = await _controller.PostAnimal(animal);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAnimal = Assert.IsType<Animal>(okResult.Value);
            Assert.NotNull(returnedAnimal);
        }

        //Atualização
        [Fact]
        public async Task Update_Animal_RetornaOk()
        {
            //Arrenge
            var id_animal = 1;
            var animal = new Animal
            {
                Id_animal = id_animal,
                Rg_animal = "123456789",
                Nr_microchip_animal = "123123123",
                Nm_animal = "mel",
                Dt_nascimento_animal = DateTime.Now,
                Peso_animal = 1,
                Especie_animal = "Cachorro",
                Raca_animal = "Labrador",
                Id_tutor = 1
            };

            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            _animalServiceMock.Setup(service => service.UpdateAnimalAsync(id_animal, animal))
                .ReturnsAsync(animal);

            //Act
            var atualizacaoRealizada = await _controller.PutAnimal(id_animal, animal);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoAnimal = Assert.IsType<Animal>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(animal.Id_animal, retornoAnimal.Id_animal);
            Assert.Equal(animal.Rg_animal, retornoAnimal.Rg_animal);
            Assert.Equal(animal.Nr_microchip_animal, retornoAnimal.Nr_microchip_animal);
            Assert.Equal(animal.Nm_animal, retornoAnimal.Nm_animal);
            Assert.Equal(animal.Dt_nascimento_animal, retornoAnimal.Dt_nascimento_animal);
            Assert.Equal(animal.Peso_animal, retornoAnimal.Peso_animal);
            Assert.Equal(animal.Especie_animal, retornoAnimal.Especie_animal);
            Assert.Equal(animal.Raca_animal, retornoAnimal.Raca_animal);
            Assert.Equal(animal.Id_tutor, retornoAnimal.Id_tutor);
        }
    }
}