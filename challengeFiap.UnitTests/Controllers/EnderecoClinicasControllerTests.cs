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
    public class enderecoClinicasControllerTests
    {

        private readonly Mock<IenderecoClinicaService> _enderecoClinicaServiceMock;

        private readonly EnderecoClinicasController _controller;
        private readonly ILogger<EnderecoClinicasController> _logger;
        private readonly AppDbContext _context;

        public enderecoClinicasControllerTests()
        {
            _enderecoClinicaServiceMock = new Mock<IenderecoClinicaService>();
            var loggMock = new Mock<ILogger<EnderecoClinicasController>>();
            _logger = loggMock.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _controller = new EnderecoClinicasController(_context, _logger, _enderecoClinicaServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_enderecoClinica_RetornaOK()
        {
            // Arrange
            var Clinica = new Clinica
            {
                Id_clinica = 1,
                Cnpj_clinica = "123456789",
                Nm_clinica = "PetSoule"
            };

            _context.Clinicas.Add(Clinica);
            await _context.SaveChangesAsync();

            var id_endereco_clinica = 1;
            var enderecoClinica = new EnderecoClinica
            {
                Id_endereco_clinica = id_endereco_clinica,
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
            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
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

            _context.EnderecoClinicas.Add(enderecoClinica);
            await _context.SaveChangesAsync();

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

        [Fact]
        public async Task GetAll_EnderecoClinica_RetornandoOk()
        {
            // Arrange
            var list = new List<EnderecoClinica>
            {
                new EnderecoClinica 
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
                    Id_clinica = 1
                }

            };

            _enderecoClinicaServiceMock.Setup(s => s.GetAllEnderecoClinicaAsync()).ReturnsAsync(list);

            // Act
            var resultado = await _controller.GetAllEnderecoClinica();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var returnedList = Assert.IsType<List<EnderecoClinica>>(okResult.Value);

            Assert.NotNull(returnedList);
            Assert.Single(returnedList);
        }

        [Fact]
        public async Task GetId_EnderecoClinica_RetornandoOk()
        {
            // Arrange
            var id_endereco_clinica = 1;

            var endereco = new EnderecoClinica
            {
                Id_endereco_clinica = id_endereco_clinica,
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

            _enderecoClinicaServiceMock.Setup(s => s.GetEnderecoClinicaIdAsync(id_endereco_clinica)).ReturnsAsync(endereco);

            // Act
            var resultado = await _controller.GetEnderecoClinica(id_endereco_clinica);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var returnedEndereco = Assert.IsType<EnderecoClinica>(okResult.Value);

            Assert.NotNull(returnedEndereco);
            Assert.Equal(id_endereco_clinica, returnedEndereco.Id_endereco_clinica);
        }

        [Fact]
        public async Task Deleta_EnderecoClinica()
        {
            // Arrange
            var id_endereco_clinica = 1;

            var endereco = new EnderecoClinica 
            { 
                Id_endereco_clinica = id_endereco_clinica,
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

            _enderecoClinicaServiceMock.Setup(s => s.DeleteEnderecoClinicaAsync(id_endereco_clinica)).ReturnsAsync(endereco);

            _context.EnderecoClinicas.Add(endereco);
            await _context.SaveChangesAsync();

            // Act
            var r = await _controller.DeleteEnderecoClinica(id_endereco_clinica);

            // Assert
            Assert.IsType<NoContentResult>(r);
        }
    }
}
