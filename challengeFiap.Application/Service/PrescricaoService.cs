using challengeFiap.Application.Diagnostics;
using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;

namespace challengeFiap.Application.Service
{
    public class PrescricaoService : IprescricaoService
    {
        private readonly ILogger<PrescricaoService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _prescricaoCreateCounter;


        public PrescricaoService(ILogger<PrescricaoService> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _prescricaoCreateCounter = meter.CreateCounter<int>("prescricao.create", description: "Total criado");
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
    }
}