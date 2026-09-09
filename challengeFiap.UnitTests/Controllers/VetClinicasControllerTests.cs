using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using challengeFiap.Infrastruture.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            _controller = new VetClinicasController(_context, _logger, _VetClinicaServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_VetClinica_RetornaOK()
        {
            // Arrange
            var VetClinica = new VetClinica
            {
                Id_clinica_vet=1,
                Id_vet=1,
                Id_clinica=1,
            };
            _VetClinicaServiceMock.Setup(service => service.CreateVetClinicaAsync(VetClinica))
                .ReturnsAsync(VetClinica);
            // Act
            var result = await _controller.PostVetClinica(VetClinica);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedVetClinica = Assert.IsType<VetClinica>(okResult.Value);
            Assert.NotNull(returnedVetClinica);
        }

        //Atualização
        [Fact]
        public async Task Update_VetClinica_RetornaOk()
        {
            //Arrenge
            var id_VetClinica = 1;
            var VetClinica = new VetClinica
            {
                Id_clinica_vet = id_VetClinica,
                Id_vet = 1,
                Id_clinica = 1,
            };

            _VetClinicaServiceMock.Setup(service => service.UpdateVetClinicaAsync(id_VetClinica, VetClinica))
                .ReturnsAsync(VetClinica);

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
    }
}
