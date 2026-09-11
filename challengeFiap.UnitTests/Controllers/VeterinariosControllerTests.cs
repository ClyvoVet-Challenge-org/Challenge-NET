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
using System.Net;
using System.Text;

namespace challengeFiap.UnitTests.Controllers
{
    public class VeterinariosControllerTests
    {
        private readonly Mock<IVeterinarioService> _veterinarioServiceMock;

        private readonly VeterinariosController _controller;
        private readonly ILogger<VeterinariosController> _logger;
        private readonly AppDbContext _context;



        public VeterinariosControllerTests()
        {
            _veterinarioServiceMock = new Mock<IVeterinarioService>();
            var loggMock = new Mock<ILogger<VeterinariosController>>();
            _logger = loggMock.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _controller = new VeterinariosController(_context, _logger, _veterinarioServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_veterinario_RetornaOK()
        {
            // Arrange
            var id_vet = 1;
            var veterinario = new Veterinario
            {
                Id_vet = id_vet,
                Nm_vet = "lual",
                Cpf_vet = "123123123",
                Crmv_vet = "12345556",
                Email_vet = "sagafdf@gmail.com",
                Senha_vet = "21358"
            };
            _veterinarioServiceMock.Setup(service => service.CreateVeterinarioAsync(veterinario))
                .ReturnsAsync(veterinario);
            // Act
            var result = await _controller.PostVeterinario(veterinario);
            // Assert

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedveterinario = Assert.IsType<Veterinario>(okResult.Value);

            Assert.NotNull(returnedveterinario);
        }

        //Atualização
        [Fact]
        public async Task Update_veterinario_RetornaOk()
        {
            //Arrenge
            var id_veterinario = 1;
            var veterinario = new Veterinario
            {
                Id_vet = id_veterinario,
                Nm_vet = "lual",
                Cpf_vet = "123123123",
                Crmv_vet = "12345556",
                Email_vet = "sagafdf@gmail.com",
                Senha_vet = "21359"
            };

            _context.Veterinarios.Add(veterinario);
            await _context.SaveChangesAsync();

            _veterinarioServiceMock.Setup(service => service.UpdateVeterinarioAsync(id_veterinario, veterinario))
                .ReturnsAsync(veterinario);

            //Act
            var atualizacaoRealizada = await _controller.PutVeterinario(id_veterinario, veterinario);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoveterinario = Assert.IsType<Veterinario>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(veterinario.Id_vet, retornoveterinario.Id_vet);
            Assert.Equal(veterinario.Nm_vet, retornoveterinario.Nm_vet);
            Assert.Equal(veterinario.Cpf_vet, retornoveterinario.Cpf_vet);
            Assert.Equal(veterinario.Crmv_vet, retornoveterinario.Crmv_vet);
            Assert.Equal(veterinario.Email_vet, retornoveterinario.Email_vet);
            Assert.Equal(veterinario.Senha_vet, retornoveterinario.Senha_vet);
        }

    }
}
