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
    public class MedicamentoesControllerTests
    {

        private readonly Mock<IMedicamentoService> _MedicamentoServiceMock;

        private readonly MedicamentoesController _controller;
        private readonly ILogger<MedicamentoesController> _logger;
        private readonly AppDbContext _context;

        public MedicamentoesControllerTests()
        {
            _MedicamentoServiceMock = new Mock<IMedicamentoService>();
            var loggMock = new Mock<ILogger<MedicamentoesController>>();
            _logger = loggMock.Object;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _controller = new MedicamentoesController(_context, _logger, _MedicamentoServiceMock.Object);
        }

        //Criação
        [Fact]
        public async Task Create_Medicamento_RetornaOK()
        {
            // Arrange
            var Prescricaos = new Prescricao
            {
                Id_prescricao = 1,
                Dt_emissao = DateTime.Now,
                Dt_expiracao = new DateTime(2026, 09, 10),
                Id_consulta = 1,
                Observacoes_gerais = "Paciente esta bem"
            };
            _context.Prescricaos.Add(Prescricaos);
            await _context.SaveChangesAsync();

            var id_medicamento = 1;
            var Medicamento = new Medicamento
            {
                Id_medicamento = id_medicamento,
                Id_prescricao = 1,
                Nm_medicamento = "AJUDA",
                Dosagem_medicamento = "GOTA",
                Frequencia = "2 vezes",
                Qtd_dias = 1
            };
            _MedicamentoServiceMock.Setup(service => service.CreateMedicamentoAsync(Medicamento))
                .ReturnsAsync(Medicamento);
            // Act
            var result = await _controller.PostMedicamento(Medicamento);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMedicamento = Assert.IsType<Medicamento>(okResult.Value);
            Assert.NotNull(returnedMedicamento);
        }

        //Atualização
        [Fact]
        public async Task Update_Medicamento_RetornaOk()
        {
            //Arrenge
            var id_Medicamento = 1;
            var Medicamento = new Medicamento
            {
                Id_medicamento = id_Medicamento,
                Id_prescricao = 1,
                Nm_medicamento = "AJUDA",
                Dosagem_medicamento = "GOTA",
                Frequencia = "2 vezes",
                Qtd_dias = 1
            };

            _context.Medicamentos.Add(Medicamento);
            await _context.SaveChangesAsync();

            _MedicamentoServiceMock.Setup(service => service.UpdateMedicamentoAsync(id_Medicamento, Medicamento))
                .ReturnsAsync(Medicamento);

            //Act
            var atualizacaoRealizada = await _controller.PutMedicamento(id_Medicamento, Medicamento);

            //Assert
            var OkResultado = Assert.IsType<OkObjectResult>(atualizacaoRealizada);
            var retornoMedicamento = Assert.IsType<Medicamento>(OkResultado.Value);
            //Dados atualizados
            Assert.Equal(Medicamento.Id_medicamento, retornoMedicamento.Id_medicamento);
            Assert.Equal(Medicamento.Id_prescricao, retornoMedicamento.Id_prescricao);
            Assert.Equal(Medicamento.Nm_medicamento, retornoMedicamento.Nm_medicamento);
            Assert.Equal(Medicamento.Dosagem_medicamento, retornoMedicamento.Dosagem_medicamento);
            Assert.Equal(Medicamento.Frequencia, retornoMedicamento.Frequencia);
            Assert.Equal(Medicamento.Qtd_dias, retornoMedicamento.Qtd_dias);

        }
    }
}
