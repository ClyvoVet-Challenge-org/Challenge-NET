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
        public async Task GetAll_Tutor_RetornandoOk()
        {
            // Arrange
            var list = new List<Tutor>
            {
                new Tutor 
                { 
                    Id_tutor = 1,
                    Cpf_tutor = "123456789",
                    Nm_tutor = "Leticia",
                    Nr_telefone_tutor = "11987562335"
                }
            };

            _TutorServiceMock.Setup(s => s.GetAllTutorAsync())
                .ReturnsAsync(list);

            // Act
            var resultado = await _controller.GetAllTutor();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var returnedList = Assert.IsType<List<Tutor>>(okResult.Value);

            Assert.NotNull(returnedList);
            Assert.Single(returnedList);
        }

        [Fact]
        public async Task GetId_Tutor_RetornandoOk()
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

            _TutorServiceMock.Setup(s => s.GetTutorIdAsync(id_tutor)).ReturnsAsync(tutor);

            // Act
            var resultado = await _controller.GetTutor(id_tutor);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var returnedTutor = Assert.IsType<Tutor>(okResult.Value);

            Assert.NotNull(returnedTutor);
            Assert.Equal(id_tutor, returnedTutor.Id_tutor);
        }

        [Fact]
        public async Task Deleta_Tutor()
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

            _TutorServiceMock.Setup(s => s.DeleteTutorAsync(id_tutor)).ReturnsAsync(tutor);

            _context.Tutor.Add(tutor);
            await _context.SaveChangesAsync();

            // Act
            var r = await _controller.DeleteTutor(id_tutor);

            // Assert
            Assert.IsType<NoContentResult>(r);
        }

    }
}
