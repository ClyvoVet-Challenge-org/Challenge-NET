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
    public class enderecoClinicasControllerTests
    {

        private readonly Mock<IenderecoClinicaService> _enderecoClinicaServiceMock;

        private readonly EnderecoClinicasController _controller;
        private readonly ILogger<EnderecoClinicasController> _logger;
        private readonly AppDbContext _context;

        public enderecoClinicasControllerTests()
        {
            _enderecoClinicaServiceMock = new Mock<IenderecoClinicaService>();
            _controller = new EnderecoClinicasController(_context, _logger, _enderecoClinicaServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_enderecoClinica_RetornaOK()
        {
            // Arrange
            var enderecoClinica = new EnderecoClinica
            {
                Id_endereco_clinica = 1,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_clinica = 1,
            };
            _enderecoClinicaServiceMock.Setup(service => service.CreateEnderecoClinicaAsync(enderecoClinica))
                .ReturnsAsync(enderecoClinica);
            // Act
            var result = await _controller.PostEnderecoClinica(enderecoClinica);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedenderecoClinica = Assert.IsType<EnderecoClinica>(okResult.Value);
            Assert.NotNull(returnedenderecoClinica);
        }

        //Atualização
        [Fact]
        public async Task Update_enderecoClinica_RetornaOk()
        {
            //Arrenge
            var id_enderecoClinica = 1;
            var enderecoClinica= new EnderecoClinica
            {
                Id_endereco_clinica = id_enderecoClinica,
                Pais = "brasil",
                Estado = "são paulo",
                Cidade = "são paulo",
                Bairro = "bairro roxo",
                Logradouro_rua = "1263",
                Nr_rua = "Dom Cachorro",
                Complemento = "1212",
                Cep = "123456",
                Id_clinica = 1,
            };

            _enderecoClinicaServiceMock.Setup(service => service.UpdateEnderecoClinicaAsync(id_enderecoClinica, enderecoClinica))
                .ReturnsAsync(enderecoClinica);

            //Act
            var atualizacaoRealizada = await _controller.PutEnderecoClinica(id_enderecoClinica, enderecoClinica);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoenderecoClinica = Assert.IsType<EnderecoClinica>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(enderecoClinica.Id_endereco_clinica, retornoenderecoClinica.Id_endereco_clinica);
            Assert.Equal(enderecoClinica.Pais, retornoenderecoClinica.Pais);
            Assert.Equal(enderecoClinica.Estado, retornoenderecoClinica.Estado);
            Assert.Equal(enderecoClinica.Cidade, retornoenderecoClinica.Cidade);
            Assert.Equal(enderecoClinica.Bairro, retornoenderecoClinica.Bairro);
            Assert.Equal(enderecoClinica.Logradouro_rua, retornoenderecoClinica.Logradouro_rua);
            Assert.Equal(enderecoClinica.Nr_rua, retornoenderecoClinica.Nr_rua);
            Assert.Equal(enderecoClinica.Complemento, retornoenderecoClinica.Complemento);
            Assert.Equal(enderecoClinica.Cep, retornoenderecoClinica.Cep);
            Assert.Equal(enderecoClinica.Id_clinica, retornoenderecoClinica.Id_clinica);

        }
    }
}
