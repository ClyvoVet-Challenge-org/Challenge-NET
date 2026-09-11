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
    public class EnderecoenderecoAnimalsControllerTests
    {

        private readonly Mock<IenderecoAnimalService> _enderecoAnimalServiceMock;

        private readonly EnderecoAnimalsController _controller;
        private readonly ILogger<EnderecoAnimalsController> _logger;
        private readonly AppDbContext _context;

        public EnderecoenderecoAnimalsControllerTests()
        {
            _enderecoAnimalServiceMock = new Mock<IenderecoAnimalService>();
            var loggMock = new Mock<ILogger<EnderecoAnimalsController>>();
            _logger = loggMock.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _controller = new EnderecoAnimalsController(_context, _logger, _enderecoAnimalServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_enderecoAnimal_RetornaOK()
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

            var enderecoAnimal = new EnderecoAnimal
            {
                Id_endereco_animal = 1,
                Pais = "brasil",
                Estado= "são paulo",
                Cidade= "são paulo",
                Bairro= "bairro roxo",
                Logradouro_rua= "1263",
                Nr_rua= "Dom Cachorro",
                Complemento= "1212",
                Cep = "123456",
                Id_animal= 1,
            };
            _enderecoAnimalServiceMock.Setup(service => service.CreateEnderecoAnimalAsync(enderecoAnimal))
                .ReturnsAsync(enderecoAnimal);
            // Act
            var result = await _controller.PostEnderecoAnimal(enderecoAnimal);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedenderecoAnimal = Assert.IsType<EnderecoAnimal>(okResult.Value);
            Assert.NotNull(returnedenderecoAnimal);
        }

        //Atualização
        [Fact]
        public async Task Update_enderecoAnimal_RetornaOk()
        {
            //Arrenge
            var id_enderecoAnimal = 1;
            var enderecoAnimal = new EnderecoAnimal
            {
                Id_endereco_animal = id_enderecoAnimal,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_animal = 1,
            };
            _context.EnderecoAnimals.Add(enderecoAnimal);
            await _context.SaveChangesAsync();

            _enderecoAnimalServiceMock.Setup(service => service.UpdateEnderecoAnimalAsync(id_enderecoAnimal, enderecoAnimal))
                .ReturnsAsync(enderecoAnimal);

            //Act
            var atualizacaoRealizada = await _controller.PutEnderecoAnimal(id_enderecoAnimal, enderecoAnimal);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoenderecoAnimal = Assert.IsType<EnderecoAnimal>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(enderecoAnimal.Id_endereco_animal, retornoenderecoAnimal.Id_endereco_animal);
            Assert.Equal(enderecoAnimal.Pais, retornoenderecoAnimal.Pais);
            Assert.Equal(enderecoAnimal.Estado, retornoenderecoAnimal.Estado);
            Assert.Equal(enderecoAnimal.Cidade, retornoenderecoAnimal.Cidade);
            Assert.Equal(enderecoAnimal.Bairro, retornoenderecoAnimal.Bairro);
            Assert.Equal(enderecoAnimal.Logradouro_rua, retornoenderecoAnimal.Logradouro_rua);
            Assert.Equal(enderecoAnimal.Nr_rua, retornoenderecoAnimal.Nr_rua);
            Assert.Equal(enderecoAnimal.Complemento, retornoenderecoAnimal.Complemento);
            Assert.Equal(enderecoAnimal.Cep, retornoenderecoAnimal.Cep);
            Assert.Equal(enderecoAnimal.Id_animal, retornoenderecoAnimal.Id_animal);

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
