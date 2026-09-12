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
    public class VetClinicasControllerTests
    {

        private readonly Mock<IVetClinica> _VetClinicaServiceMock;

        private readonly VetClinicasController _controller;
        private readonly ILogger<VetClinicasController> _logger;
        private readonly AppDbContext _context;

        public VetClinicasControllerTests()
        {
            _VetClinicaServiceMock = new Mock<IVetClinica>();

            var loggMock = new Mock<ILogger<VetClinicasController>>();
            _logger = loggMock.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _controller = new VetClinicasController(_context, _logger, _VetClinicaServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_VetClinica_RetornaOK()
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

            var Clinica = new Clinica
            {
                Id_clinica = 1,
                Cnpj_clinica = "123456789",
                Nm_clinica = "PetSoule"
            };
            _context.Clinicas.Add(Clinica);
            await _context.SaveChangesAsync();

            var id_clinica_vet = 1;
            var VetClinica = new VetClinica
            {
                Id_clinica_vet=id_clinica_vet,
                Id_vet=1,
                Id_clinica=1,
            };
            _VetClinicaServiceMock.Setup(service => service.CreateVetClinicaAsync(VetClinica))
                .ReturnsAsync(VetClinica);

            // Act
            var result = await _controller.PostVetClinica(VetClinica);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedVetClinica = Assert.IsType<VetClinica>(okResult.Value);
            Assert.NotNull(returnedVetClinica);
        }

        //Atualização
        [Fact]
        public async Task Update_VetClinica_RetornaOk()
        {
            //Arrenge
            var id_VetClinica = 1;

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

            var Clinica = new Clinica
            {
                Id_clinica = 1,
                Cnpj_clinica = "123456789",
                Nm_clinica = "PetSoule"
            };
            _context.Clinicas.Add(Clinica);
            await _context.SaveChangesAsync();

            var VetClinica = new VetClinica
            {
                Id_clinica_vet = id_VetClinica,
                Id_vet = 1,
                Id_clinica = 1,
            };

            _context.VetClinicas.Add(VetClinica);
            await _context.SaveChangesAsync();

            _VetClinicaServiceMock.Setup(service => service.UpdateVetClinicaAsync(id_VetClinica, VetClinica)).ReturnsAsync(VetClinica);

            //Act
            var atualizacaoRealizada = await _controller.PutVetClinica(id_VetClinica, VetClinica);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoVetClinica = Assert.IsType<VetClinica>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(VetClinica.Id_clinica_vet, retornoVetClinica.Id_clinica_vet);
            Assert.Equal(VetClinica.Id_vet, retornoVetClinica.Id_vet);
            Assert.Equal(VetClinica.Id_clinica, retornoVetClinica.Id_clinica);
        }

        [Fact]
        public async Task GetAll_VetClinica_RetornandoOk()
        {
            // Arrange
            var list = new List<VetClinica>
            {
                new VetClinica 
                { 
                    Id_clinica_vet = 1, 
                    Id_vet = 1, 
                    Id_clinica = 1 
                }
            };

            _VetClinicaServiceMock.Setup(s => s.GetAllVetClinicaAsync()).ReturnsAsync(list);

            // Act
            var resultado = await _controller.GetAllVetClinica();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var returnedList = Assert.IsType<List<VetClinica>>(okResult.Value);

            Assert.NotNull(returnedList);
            Assert.Single(returnedList);
        }

        [Fact]
        public async Task GetId_VetClinica_RetornandoOk()
        {
            // Arrange
            var id_clinica_vet = 1;

            var vetclinica = new VetClinica
            {
                Id_clinica_vet = id_clinica_vet,
                Id_vet = 1,
                Id_clinica = 1
            };

            _VetClinicaServiceMock.Setup(s => s.GetVetClinicaIdAsync(id_clinica_vet)).ReturnsAsync(vetclinica);

            // Act
            var resultado = await _controller.GetVetClinica(id_clinica_vet);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var returnedVetClinica = Assert.IsType<VetClinica>(okResult.Value);

            Assert.NotNull(returnedVetClinica);
            Assert.Equal(id_clinica_vet, returnedVetClinica.Id_clinica_vet);
        }

        [Fact]
        public async Task Deleta_VetClinica()
        {
            // Arrange
            var id_clinica_vet = 1;

            var vetclinica = new VetClinica 
            { 
                Id_clinica_vet = id_clinica_vet, 
                Id_vet = 1, 
                Id_clinica = 1 
            };

            _VetClinicaServiceMock.Setup(s => s.DeleteVetClinicaAsync(id_clinica_vet)).ReturnsAsync(vetclinica);

            _context.VetClinicas.Add(vetclinica);
            await _context.SaveChangesAsync();

            // Act
            var r = await _controller.DeleteVetClinica(id_clinica_vet);

            // Assert
            Assert.IsType<NoContentResult>(r);
        }
        
    }
}
