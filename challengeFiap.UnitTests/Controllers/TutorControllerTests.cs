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
    public class TutorControllerTests
    {

        private readonly Mock<ItutorService> _TutorServiceMock;

        private readonly TutorController _controller;
        private readonly ILogger<TutorController> _logger;
        private readonly AppDbContext _context;

        public TutorControllerTests()
        {
            _TutorServiceMock = new Mock<ItutorService>();

            var loggMock = new Mock<ILogger<TutorController>>();
            _logger = loggMock.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);


            _controller = new TutorController(_context, _logger, _TutorServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_Tutor_RetornaOK()
        {
            // Arrange
            var id_tutor = 1;
            var tutor = new Tutor
            {
                Id_tutor = id_tutor,
                Cpf_tutor = "123456789",
                Nm_tutor = "Leticia",
                Nr_telefone_tutor = "11987562335"
            };
            _TutorServiceMock.Setup(service => service.CreateTutorAsync(tutor))
                .ReturnsAsync(tutor);
            // Act
            var result = await _controller.PostTutor(tutor);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTutor = Assert.IsType<Tutor>(okResult.Value);
            Assert.NotNull(returnedTutor);
        }

        //Atualização
        [Fact]
        public async Task Update_Tutor_RetornaOk()
        {
            //Arrenge
            var id_Tutor = 1;
            var Tutor = new Tutor
            {
                Id_tutor = id_Tutor,
                Cpf_tutor = "123456789",
                Nm_tutor = "Leticia",
                Nr_telefone_tutor = "11987562335"
            };
            _context.Tutor.Add(Tutor);
            await _context.SaveChangesAsync();

            _TutorServiceMock.Setup(service => service.UpdateTutorAsync(id_Tutor, Tutor))
                .ReturnsAsync(Tutor);

            //Act
            var atualizacaoRealizada = await _controller.PutTutor(id_Tutor, Tutor);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoTutor = Assert.IsType<Tutor>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(Tutor.Id_tutor, retornoTutor.Id_tutor);
            Assert.Equal(Tutor.Cpf_tutor, retornoTutor.Cpf_tutor);
            Assert.Equal(Tutor.Nm_tutor, retornoTutor.Nm_tutor);
            Assert.Equal(Tutor.Nr_telefone_tutor, retornoTutor.Nr_telefone_tutor);
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
