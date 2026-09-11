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
    public class ClinicaService : IClinicaService
    {
        private readonly ILogger<ClinicaService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _clinicaCreateCounter;

        private readonly List<Clinica> _GetClinica = new();


        public ClinicaService(ILogger<ClinicaService> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _clinicaCreateCounter = meter.CreateCounter<int>("clinica.create", description: "Total criado");
        }

        public async Task<Clinica> CreateadAsync(Clinica clinica)
        {
            using var activity = ActivitySource.StartActivity("CreateClinicaAsync");
            activity?.SetTag("clinica.id", clinica.Id_clinica);
            activity?.SetTag("clinica.nome", clinica.Nm_clinica);

            await Task.Delay(100);

            var createdClinica = new Clinica
            {
                Id_clinica = clinica.Id_clinica,
                Cnpj_clinica = clinica.Cnpj_clinica,
                Nm_clinica = clinica.Nm_clinica
            };
            
            _clinicaCreateCounter.Add(1, new KeyValuePair<string, object?>("clinica.id", createdClinica.Id_clinica), new KeyValuePair<string, object?>("clinica.nome", createdClinica.Nm_clinica));

            return createdClinica;
        }

        public async Task<Clinica> UpdateClinicaAsync(int id_clinica, Clinica clinica)
        {
            using var activity = ActivitySource.StartActivity("UpdateClinicaAsync");
            activity.SetTag("clinica.id", id_clinica);

            var updatedClinica = new Clinica
            {
                Id_clinica = id_clinica,
                Cnpj_clinica = clinica.Cnpj_clinica,
                Nm_clinica = clinica.Nm_clinica
            };

            _logger.LogInformation("Clínica atualizada com sucesso: {ClinicaId}", id_clinica);

            _clinicaCreateCounter.Add(1, new KeyValuePair<string, object?>("clinica.id", updatedClinica.Id_clinica), new KeyValuePair<string, object?>("clinica.nome", updatedClinica.Nm_clinica));
        
            return updatedClinica;
        }
        public async Task<Clinica> GetClinicaIdAsync(int id_Clinica)
        {
            var GetClinica = _GetClinica.FirstOrDefault(c => c.Id_clinica == id_Clinica);
            if (GetClinica == null)
            {
                _logger.LogWarning("Id não existe");

                throw new Exception("Id  não foi encontrada");
            }
            return GetClinica;
        }
        public async Task<Clinica> DeleteClinicaAsync(int id_Clinica)
        {
            var clinicaDelete = _GetClinica.FirstOrDefault(c => c.Id_clinica == id_Clinica);
            if (clinicaDelete != null)
            {
                _GetClinica.Remove(clinicaDelete);
                _logger.LogInformation("Deletado");

                return clinicaDelete;
            }
            else
            {
                _logger.LogWarning("Id de clinica não foi encontrado. ");

                throw new Exception("Id não existente presente");
            }
        }

    }
}
