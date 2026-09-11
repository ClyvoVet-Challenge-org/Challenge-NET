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
    public class ConsultasControllerTests
    {

        private readonly Mock<IConsultaService> _ConsultaserviceMock;

        private readonly ConsultasController _controller;
        private readonly ILogger<ConsultasController> _logger;
        private readonly AppDbContext _context;

        public ConsultasControllerTests()
        {
            _ConsultaserviceMock = new Mock<IConsultaService>();
            var loggMock = new Mock<ILogger<ConsultasController>>();
            _logger = loggMock.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _controller = new ConsultasController(_context, _logger, _ConsultaserviceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_consulta_RetornaOK()
        {
            // Arrange
            var veterinario = new Veterinario
            {
                Id_vet = 1,
                Nm_vet = "lual",
                Cpf_vet = "123123123",
                Crmv_vet = "12345556",
                Email_vet = "sagafdf@gmail.com",
                Senha_vet = "21358"
            };
            _context.Veterinarios.Add(veterinario);
            await _context.SaveChangesAsync();

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

            var id_consulta=1;
            var consulta = new Consulta
            {
                Id_consulta = id_consulta,
                Historico_consulta = "Foi bom o resultado",
                St_consulta = Domain.Enums.StatusConsulta.passada,
                Dt_consulta = DateTime.Now,
                Id_vet = 1,
                Id_animal = 1,

            };
            _ConsultaserviceMock.Setup(service => service.CreateConsultaAsync(consulta))
                .ReturnsAsync(consulta);
            // Act
            var result = await _controller.PostConsulta(consulta);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedconsulta = Assert.IsType<Consulta>(okResult.Value);
            Assert.NotNull(returnedconsulta);
        }

        //Atualização
        [Fact]
        public async Task Update_consulta_RetornaOk()
        {
            //Arrenge
            var id_consulta = 1;
            var consulta = new Consulta
            {
                Id_consulta = id_consulta,
                Historico_consulta= "Foi bom o resultado",
                St_consulta = Domain.Enums.StatusConsulta.passada,
                Dt_consulta = DateTime.Now,
                Id_vet = 1,
                Id_animal = 1,
            };

            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            _ConsultaserviceMock.Setup(service => service.UpdateConsultaAsync(id_consulta, consulta))
                .ReturnsAsync(consulta);

            //Act
            var atualizacaoRealizada = await _controller.PutConsulta(id_consulta, consulta);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoconsulta = Assert.IsType<Consulta>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(consulta.Id_consulta, retornoconsulta.Id_consulta);
            Assert.Equal(consulta.Historico_consulta, retornoconsulta.Historico_consulta);
            Assert.Equal(consulta.St_consulta, retornoconsulta.St_consulta);
            Assert.Equal(consulta.Dt_consulta, retornoconsulta.Dt_consulta);
            Assert.Equal(consulta.Id_vet, retornoconsulta.Id_vet);
            Assert.Equal(consulta.Id_animal, retornoconsulta.Id_animal);
        }

    }
}
