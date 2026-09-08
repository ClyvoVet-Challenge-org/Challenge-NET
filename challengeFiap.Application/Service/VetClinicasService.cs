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
    public class VetClinicasService : IVetClinica
    {
        private readonly ILogger<VetClinicasService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _vetClinicasCreateCounter;


        public VetClinicasService(ILogger<VetClinicasService> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _vetClinicasCreateCounter = meter.CreateCounter<int>("vetclinicas.create", description: "Total criado");
        }

        public async Task<VetClinica> CreateVetClinicaAsync(VetClinica VetClinica)
        {
            using var activity = ActivitySource.StartActivity("CreateVetClinicaAsync");
            activity?.SetTag("vetclinica.id", VetClinica.Id_clinica_vet);
            activity?.SetTag("vetclinica.id_vet", VetClinica.Id_vet);
            activity?.SetTag("vetclinica.id_clinica", VetClinica.Id_clinica);

            await Task.Delay(100);

            var createdVetClinica = new VetClinica
            {
                Id_clinica_vet = VetClinica.Id_clinica_vet,
                Id_vet = VetClinica.Id_vet,
                Id_clinica = VetClinica.Id_clinica
            };

            _logger.LogInformation("Relação entre veterinário e clínica criada com sucesso: {VetClinicaId}", createdVetClinica.Id_clinica_vet);

            _vetClinicasCreateCounter.Add(1, new KeyValuePair<string, object?>("vetclinica.id", createdVetClinica.Id_clinica_vet), new KeyValuePair<string, object?>("vetclinica.id_vet", createdVetClinica.Id_vet), new KeyValuePair<string, object?>("vetclinica.id_clinica", createdVetClinica.Id_clinica));

            return createdVetClinica;
        }

        public async Task<VetClinica> UpdateVetClinicaAsync(int id_VetClinica, VetClinica VetClinica)
        {
            using var activity = ActivitySource.StartActivity("UpdateVetClinicaAsync");
            activity?.SetTag("vetclinica.id", id_VetClinica);

            var updatedVetClinica = new VetClinica
            {
                Id_clinica_vet = id_VetClinica,
                Id_vet = VetClinica.Id_vet,
                Id_clinica = VetClinica.Id_clinica
            };

            _logger.LogInformation("Relação entre veterinário e clínica atualizada com sucesso: {VetClinicaId}", id_VetClinica);

            _vetClinicasCreateCounter.Add(1, new KeyValuePair<string, object?>("vetclinica.id", updatedVetClinica.Id_clinica_vet), new KeyValuePair<string, object?>("vetclinica.id_vet", updatedVetClinica.Id_vet), new KeyValuePair<string, object?>("vetclinica.id_clinica", updatedVetClinica.Id_clinica));

            return updatedVetClinica;
        }
    }
}