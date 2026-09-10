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
    public class ConsultaService : IConsultaService
    {
        private readonly ILogger<ConsultaService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _consultaCreateCounter;

        public ConsultaService(ILogger<ConsultaService> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _consultaCreateCounter = meter.CreateCounter<int>("consulta.create", description: "Total criado");
        }


        public async Task<Consulta> CreateConsultaAsync(Consulta consulta)
        {
            using var activity = ActivitySource.StartActivity("CreateConsultaAsync");
            activity?.SetTag("consulta.id", consulta.Id_consulta);
            activity?.SetTag("consulta.historico", consulta.Historico_consulta);
            activity?.SetTag("consulta.status", consulta.St_consulta.ToString());
            activity?.SetTag("consulta.data", consulta.Dt_consulta);
            activity?.SetTag("consulta.id_vet", consulta.Id_vet);
            activity?.SetTag("consulta.id_animal", consulta.Id_animal);

            await Task.Delay(100);

            var createdConsulta = new Consulta
            {
                Id_consulta = consulta.Id_consulta,
                Historico_consulta = consulta.Historico_consulta,
                St_consulta = consulta.St_consulta,
                Dt_consulta = consulta.Dt_consulta,
                Id_vet = consulta.Id_vet,
                Id_animal = consulta.Id_animal
            };

            _logger.LogInformation("Consulta criada com sucesso: {ConsultaId}", createdConsulta.Id_consulta);

            _consultaCreateCounter.Add(1, new KeyValuePair<string, object?>("consulta.id", createdConsulta.Id_consulta), new KeyValuePair<string, object?>("consulta.id_vet", createdConsulta.Id_vet), new KeyValuePair<string, object?>("consulta.id_animal", createdConsulta.Id_animal));

            return createdConsulta;
        }

        public async Task<Consulta> UpdateConsultaAsync(int id_consulta, Consulta consulta)
        {
            using var activity = ActivitySource.StartActivity("UpdateConsultaAsync");
            activity?.SetTag("consulta.id", id_consulta);

            var updatedConsulta = new Consulta
            {
                Id_consulta = id_consulta,
                Historico_consulta = consulta.Historico_consulta,
                St_consulta = consulta.St_consulta,
                Dt_consulta = consulta.Dt_consulta,
                Id_vet = consulta.Id_vet,
                Id_animal = consulta.Id_animal
            };

            _logger.LogInformation("Consulta atualizada com sucesso: {ConsultaId}", id_consulta);

            _consultaCreateCounter.Add(1, new KeyValuePair<string, object?>("consulta.id", updatedConsulta.Id_consulta), new KeyValuePair<string, object?>("consulta.id_vet", updatedConsulta.Id_vet), new KeyValuePair<string, object?>("consulta.id_animal", updatedConsulta.Id_animal));

            return updatedConsulta;
        }
        public async Task<Consulta> GetConsultaIdAsync(int id_Consulta)
        {
        
        }
        public async Task<Consulta> DeleteConsultaAsync(int id_Consulta)
        {
        
        }


    }
}
