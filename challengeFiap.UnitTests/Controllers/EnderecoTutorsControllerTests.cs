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
    public class EnderecoTutorsControllerTests
    {

        private readonly Mock<IenderecoTutorService> _enderecoTutorServiceMock;

        private readonly EnderecoTutorsController _controller;
        private readonly ILogger<EnderecoTutorsController> _logger;
        private readonly AppDbContext _context;

        public EnderecoTutorsControllerTests()
        {
            _enderecoTutorServiceMock = new Mock<IenderecoTutorService>();
            var loggMock = new Mock<ILogger<EnderecoTutorsController>>();
            _logger = loggMock.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _controller = new EnderecoTutorsController(_context, _logger, _enderecoTutorServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_enderecoTutor_RetornaOK()
        {
            // Arrange
            var tutor = new Tutor
            {
                Id_tutor = 1,
                Cpf_tutor = "123456789",
                Nm_tutor = "Leticia",
                Nr_telefone_tutor = "11987562335"
            };
            _context.Tutor.Add(tutor);
            await _context.SaveChangesAsync();

            var id_endereco_tutor = 1;
            var enderecoTutor = new EnderecoTutor
            {
                Id_endereco_tutor = id_endereco_tutor,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_tutor = 1,
            };
            _enderecoTutorServiceMock.Setup(service => service.CreateEnderecoTutorAsync(enderecoTutor))
                .ReturnsAsync(enderecoTutor);
            // Act
            var result = await _controller.PostEnderecoResponsavel(enderecoTutor);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedenderecoTutor = Assert.IsType<EnderecoTutor>(okResult.Value);
            Assert.NotNull(returnedenderecoTutor);
        }

        //Atualização
        [Fact]
        public async Task Update_enderecoTutor_RetornaOk()
        {
            //Arrenge
            var id_enderecoTutor = 1;
            var enderecoTutor = new EnderecoTutor
            {
                Id_endereco_tutor = id_enderecoTutor,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_tutor = 1,
            };

            _context.EnderecoTutors.Add(enderecoTutor);
            await _context.SaveChangesAsync();

            _enderecoTutorServiceMock.Setup(service => service.UpdateEnderecoTutorAsync(id_enderecoTutor, enderecoTutor))
                .ReturnsAsync(enderecoTutor);

            //Act
            var atualizacaoRealizada = await _controller.PutEnderecoResponsavel(id_enderecoTutor, enderecoTutor);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoenderecoTutor = Assert.IsType<EnderecoTutor>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(enderecoTutor.Id_endereco_tutor, retornoenderecoTutor.Id_endereco_tutor);
            Assert.Equal(enderecoTutor.Pais, retornoenderecoTutor.Pais);
            Assert.Equal(enderecoTutor.Estado, retornoenderecoTutor.Estado);
            Assert.Equal(enderecoTutor.Cidade, retornoenderecoTutor.Cidade);
            Assert.Equal(enderecoTutor.Bairro, retornoenderecoTutor.Bairro);
            Assert.Equal(enderecoTutor.Logradouro_rua, retornoenderecoTutor.Logradouro_rua);
            Assert.Equal(enderecoTutor.Nr_rua, retornoenderecoTutor.Nr_rua);
            Assert.Equal(enderecoTutor.Complemento, retornoenderecoTutor.Complemento);
            Assert.Equal(enderecoTutor.Cep, retornoenderecoTutor.Cep);
            Assert.Equal(enderecoTutor.Id_tutor, retornoenderecoTutor.Id_tutor);

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
