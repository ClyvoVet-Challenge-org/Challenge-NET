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
    public class PrescricaosControllerTests
    {

        private readonly Mock<IprescricaoService> _PrescricaosServiceMock;

        private readonly PrescricaosController _controller;
        private readonly ILogger<PrescricaosController> _logger;
        private readonly AppDbContext _context;

        public PrescricaosControllerTests()
        {
            _PrescricaosServiceMock = new Mock<IprescricaoService>();
            var loggMock = new Mock<ILogger<PrescricaosController>>();
            _logger = loggMock.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _controller = new PrescricaosController(_context, _logger, _PrescricaosServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_Prescricaos_RetornaOK()
        {
            // Arrange
            var consulta = new Consulta
            {
                Id_consulta = 1,
                Historico_consulta = "Foi bom o resultado",
                St_consulta = Domain.Enums.StatusConsulta.passada,
                Dt_consulta = DateTime.Now,
                Id_vet = 1,
                Id_animal = 1,

            };
            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            var id_prescricao = 1;
            var Prescricaos = new Prescricao
            {
                Id_prescricao = id_prescricao,
                Dt_emissao = DateTime.Now,
                Dt_expiracao = new DateTime(2026,09,10),
                Id_consulta = 1,
                Observacoes_gerais="Paciente esta bem"
            };
            _PrescricaosServiceMock.Setup(service => service.CreatePrescricaoAsync(Prescricaos))
                .ReturnsAsync(Prescricaos);
            // Act
            var result = await _controller.PostPrescricao(Prescricaos);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPrescricaos = Assert.IsType<Prescricao>(okResult.Value);
            Assert.NotNull(returnedPrescricaos);
        }

        //Atualização
        [Fact]
        public async Task Update_Prescricaos_RetornaOk()
        {
            //Arrenge
            var id_Prescricaos = 1;
            var Prescricaos = new Prescricao
            {
                Id_prescricao = id_Prescricaos,
                Dt_emissao = DateTime.Now,
                Dt_expiracao = new DateTime(2026, 09, 10),
                Id_consulta = 1,
                Observacoes_gerais = "Paciente esta bem"
            };
            _context.Prescricaos.Add(Prescricaos);
            await _context.SaveChangesAsync();

            _PrescricaosServiceMock.Setup(service => service.UpdatePrescricaoAsync(id_Prescricaos, Prescricaos))
                .ReturnsAsync(Prescricaos);

            //Act
            var atualizacaoRealizada = await _controller.PutPrescricao(id_Prescricaos, Prescricaos);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoPrescricaos = Assert.IsType<Prescricao>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(Prescricaos.Id_prescricao, retornoPrescricaos.Id_prescricao);
            Assert.Equal(Prescricaos.Dt_emissao, retornoPrescricaos.Dt_emissao);
            Assert.Equal(Prescricaos.Dt_expiracao, retornoPrescricaos.Dt_expiracao);
            Assert.Equal(Prescricaos.Id_consulta, retornoPrescricaos.Id_consulta);
            Assert.Equal(Prescricaos.Observacoes_gerais, retornoPrescricaos.Observacoes_gerais);
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
