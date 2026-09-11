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
    public class TutorService : ItutorService
    {
        private readonly ILogger<TutorService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _tutorCreateCounter;

        private readonly List<Tutor> _GetTutor = new();

        public TutorService(ILogger<TutorService> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _tutorCreateCounter = meter.CreateCounter<int>("tutor.create", description: "Total criado");
        }

        public async Task<Tutor> CreateTutorAsync(Tutor tutor)
        {
            using var activity = ActivitySource.StartActivity("CreateTutorAsync");
            activity?.SetTag("tutor.id", tutor.Id_tutor);
            activity?.SetTag("tutor.cpf", tutor.Cpf_tutor);
            activity?.SetTag("tutor.nome", tutor.Nm_tutor);
            activity?.SetTag("tutor.telefone", tutor.Nr_telefone_tutor);

            await Task.Delay(100);

            var createdTutor = new Tutor
            {
                Id_tutor = tutor.Id_tutor,
                Cpf_tutor = tutor.Cpf_tutor,
                Nm_tutor = tutor.Nm_tutor,
                Nr_telefone_tutor = tutor.Nr_telefone_tutor
            };

            _logger.LogInformation("Tutor criado com sucesso: {TutorId}", createdTutor.Id_tutor);

            _tutorCreateCounter.Add(1, new KeyValuePair<string, object?>("tutor.id", createdTutor.Id_tutor));

            return createdTutor;
        }

        public async Task<Tutor> UpdateTutorAsync(int id_tutor, Tutor tutor)
        {
            using var activity = ActivitySource.StartActivity("UpdateTutorAsync");
            activity?.SetTag("tutor.id", id_tutor);

            var updatedTutor = new Tutor
            {
                Id_tutor = id_tutor,
                Cpf_tutor = tutor.Cpf_tutor,
                Nm_tutor = tutor.Nm_tutor,
                Nr_telefone_tutor = tutor.Nr_telefone_tutor
            };

            _logger.LogInformation("Tutor atualizado com sucesso: {TutorId}", id_tutor);

            _tutorCreateCounter.Add(1, new KeyValuePair<string, object?>("tutor.id", updatedTutor.Id_tutor));

            return updatedTutor;
        }

    }
}