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
    public class PrescricaoService : IprescricaoService
    {
        private readonly ILogger<PrescricaoService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _prescricaoCreateCounter;

        private readonly List<Prescricao> _GetPrescricao = new();

        private readonly AppDbContext _context;

        public PrescricaoService(ILogger<PrescricaoService> logger, IMeterFactory meterFactory, AppDbContext context)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _prescricaoCreateCounter = meter.CreateCounter<int>("prescricao.create", description: "Total criado");
            _context = context;
        }

        public async Task<Prescricao> CreatePrescricaoAsync(Prescricao prescricao)
        {
            using var activity = ActivitySource.StartActivity("CreatePrescricaoAsync");
            activity?.SetTag("prescricao.id", prescricao.Id_prescricao);
            activity?.SetTag("prescricao.dt_emissao", prescricao.Dt_emissao);
            activity?.SetTag("prescricao.dt_expiracao", prescricao.Dt_expiracao);
            activity?.SetTag("prescricao.id_consulta", prescricao.Id_consulta);
            activity?.SetTag("prescricao.observacoes_gerais", prescricao.Observacoes_gerais);

            await Task.Delay(100);

            var createdPrescricao = new Prescricao
            {
                Id_prescricao = prescricao.Id_prescricao,
                Dt_emissao = prescricao.Dt_emissao,
                Dt_expiracao = prescricao.Dt_expiracao,
                Id_consulta = prescricao.Id_consulta,
                Observacoes_gerais = prescricao.Observacoes_gerais
            };

            _logger.LogInformation("Prescrição criada com sucesso: {PrescricaoId}", createdPrescricao.Id_prescricao);

            _prescricaoCreateCounter.Add(1, new KeyValuePair<string, object?>("prescricao.id", createdPrescricao.Id_prescricao), new KeyValuePair<string, object?>("prescricao.id_consulta", createdPrescricao.Id_consulta));

            return createdPrescricao;
        }

        public async Task<Prescricao> UpdatePrescricaoAsync(int id_prescricao, Prescricao prescricao)
        {
            using var activity = ActivitySource.StartActivity("UpdatePrescricaoAsync");
            activity?.SetTag("prescricao.id", id_prescricao);

            var updatedPrescricao = new Prescricao
            {
                Id_prescricao = id_prescricao,
                Dt_emissao = prescricao.Dt_emissao,
                Dt_expiracao = prescricao.Dt_expiracao,
                Id_consulta = prescricao.Id_consulta,
                Observacoes_gerais = prescricao.Observacoes_gerais
            };

            _logger.LogInformation("Prescrição atualizada com sucesso: {PrescricaoId}", id_prescricao);

            _prescricaoCreateCounter.Add(1, new KeyValuePair<string, object?>("prescricao.id", updatedPrescricao.Id_prescricao), new KeyValuePair<string, object?>("prescricao.id_consulta", updatedPrescricao.Id_consulta));

            return updatedPrescricao;
        }

        public async Task<Prescricao> DeletePrescricaoAsync(int id_Prescricao)
        {
            var prescricao = await _context.Prescricaos.FindAsync(id_Prescricao);

            if (prescricao == null)
            {
                _logger.LogWarning("prescricao não encontrada para exclusão.");
                throw new Exception("Nao foi inserido");
            }
            else
            {
                _context.Prescricaos.Remove(prescricao);
                _logger.LogInformation("Prescricao encontrado para exclusão");

                return prescricao;
            }
        }

        public async Task<Prescricao> GetPrescricaoIdAsync(int id_prescricao)
        {
            var prescricao = await _context.Prescricaos.FindAsync(id_prescricao);
            if (prescricao == null)
            {
                _logger.LogWarning("Id não existe: {Id}", id_prescricao);
                throw new Exception("Id não foi encontrada");
            }

            return prescricao;
        }

        public async Task<IEnumerable<Prescricao>> GetAllPrescricaoAsync()
        {
            var prescricoes = await _context.Prescricaos.ToListAsync();
            return prescricoes;
        }
    }
}
