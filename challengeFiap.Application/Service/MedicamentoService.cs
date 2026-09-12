using challengeFiap.Application.Diagnostics;
using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using challengeFiap.Infrastruture.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace challengeFiap.Application.Service
{
    public class MedicamentoService : IMedicamentoService
    {

        private readonly ILogger<MedicamentoService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);
        private readonly Counter<int> _medicamentoTaxaErro;

        private readonly AppDbContext _context;


        public MedicamentoService(ILogger<MedicamentoService> logger, IMeterFactory meterFactory, AppDbContext context)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _medicamentoTaxaErro = meter.CreateCounter<int>("medicamento.error");
            _context = context;
        }

        public async Task<Medicamento> CreateMedicamentoAsync(Medicamento medicamento)
        {
            try
            {
                using var activity = ActivitySource.StartActivity("CreateMedicamentoAsync");
                activity?.SetTag("medicamento.id", medicamento.Id_medicamento);
                activity?.SetTag("medicamento.id_prescricao", medicamento.Id_prescricao);
                activity?.SetTag("medicamento.nome", medicamento.Nm_medicamento);
                activity?.SetTag("medicamento.dosagem", medicamento.Dosagem_medicamento);
                activity?.SetTag("medicamento.frequencia", medicamento.Frequencia);
                activity?.SetTag("medicamento.qtd_dias", medicamento.Qtd_dias);

                await Task.Delay(100);

                var createdMedicamento = new Medicamento
                {
                    Id_medicamento = medicamento.Id_medicamento,
                    Id_prescricao = medicamento.Id_prescricao,
                    Nm_medicamento = medicamento.Nm_medicamento,
                    Dosagem_medicamento = medicamento.Dosagem_medicamento,
                    Frequencia = medicamento.Frequencia,
                    Qtd_dias = medicamento.Qtd_dias
                };

                _logger.LogInformation("Medicamento criado com sucesso: {MedicamentoId}", createdMedicamento.Id_medicamento);


                return createdMedicamento;
            }
            catch (Exception ex)
            {
                _medicamentoTaxaErro.Add(1);
                _logger.LogError("Erro ao criar medicamento: {erro}", ex);
                throw;
            }
        }

        public async Task<Medicamento> UpdateMedicamentoAsync(int id_medicamento, Medicamento medicamento)
        {
            try
            {
                using var activity = ActivitySource.StartActivity("UpdateMedicamentoAsync");
                activity?.SetTag("medicamento.id", id_medicamento);

                var updatedMedicamento = new Medicamento
                {
                    Id_medicamento = id_medicamento,
                    Id_prescricao = medicamento.Id_prescricao,
                    Nm_medicamento = medicamento.Nm_medicamento,
                    Dosagem_medicamento = medicamento.Dosagem_medicamento,
                    Frequencia = medicamento.Frequencia,
                    Qtd_dias = medicamento.Qtd_dias
                };

                _logger.LogInformation("Medicamento atualizado com sucesso: {MedicamentoId}", id_medicamento);

                return updatedMedicamento;
            }
            catch (Exception ex)
            {
                _medicamentoTaxaErro.Add(1);
                _logger.LogError("Erro ao atualizar medicamento: {erro}", ex);
                throw;
            }
        }
        public async Task<Medicamento> DeleteMedicamentoAsync(int id_Medicamento)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id_Medicamento);

            if (medicamento == null)
            {
                _logger.LogWarning("medicamento não encontrada para exclusão.");
                throw new Exception("Nao foi inserido");
            }
            else
            {
                _context.Medicamentos.Remove(medicamento);
                _logger.LogInformation("Medicamento encontrado para exclusão");

                return medicamento;
            }
        }

        public async Task<Medicamento> GetMedicamentoIdAsync(int id_medicamento)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id_medicamento);
            if (medicamento == null)
            {
                _logger.LogWarning("Id não existe: {Id}", id_medicamento);
                throw new Exception("Id não foi encontrada");
            }

            return medicamento;
        }

        public async Task<IEnumerable<Medicamento>> GetAllMedicamentoAsync()
        {
            var medicamentos = await _context.Medicamentos.ToListAsync();
            return medicamentos;
        }
    }
}
