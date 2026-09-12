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
using System.Threading.Tasks;

namespace challengeFiap.Application.Service
{
    public class ClinicaService : IClinicaService
    {
        private readonly ILogger<ClinicaService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);
        private readonly Counter<int> _clinicaTaxaErro;

        private readonly AppDbContext _context;


        public ClinicaService(ILogger<ClinicaService> logger, IMeterFactory meterFactory, AppDbContext context)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _clinicaTaxaErro = meter.CreateCounter<int>("clinica.error");
            _context = context;
        }

        public async Task<Clinica> CreateadAsync(Clinica clinica)
        {
            try
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

                return createdClinica;
            }
            catch (Exception ex)
            {
                _clinicaTaxaErro.Add(1);
                _logger.LogError("Erro ao criar clinica: {erro}", ex);
                throw;
            }
        }

        public async Task<Clinica> UpdateClinicaAsync(int id_clinica, Clinica clinica)
        {
            try
            {
                using var activity = ActivitySource.StartActivity("UpdateClinicaAsync");
                activity?.SetTag("clinica.id", id_clinica);

                var updatedClinica = new Clinica
                {
                    Id_clinica = id_clinica,
                    Cnpj_clinica = clinica.Cnpj_clinica,
                    Nm_clinica = clinica.Nm_clinica
                };

                _logger.LogInformation("Clínica atualizada com sucesso: {ClinicaId}", id_clinica);

                return updatedClinica;
            }
            catch (Exception ex)
            {
                _clinicaTaxaErro.Add(1);
                _logger.LogError("Erro ao atualizar clinica: {erro}", ex);
                throw;
            }
        }

        public async Task<Clinica> DeleteClinicaAsync(int id_Clinica)
        {
            var clinica = await _context.Clinicas.FindAsync(id_Clinica);

            if (clinica == null)
            {
                _logger.LogWarning("clinica não encontrada para exclusão.");
                _clinicaTaxaErro.Add(1);
                throw new Exception("Nao foi inserido");
            }
            else
            {
                _context.Clinicas.Remove(clinica);

                _logger.LogInformation("Clinica encontrado para exclusão");

                return clinica;
            }
        }

        public async Task<Clinica> GetClinicaIdAsync(int id_Clinica)
        {
            var clinica = await _context.Clinicas.FindAsync(id_Clinica);
            if (clinica == null)
            {
                _logger.LogWarning("Id não existe: {Id}", id_Clinica);
                _clinicaTaxaErro.Add(1);
                throw new Exception("Id não foi encontrada");
            }

            return clinica;
        }

        public async Task<IEnumerable<Clinica>> GetAllClinicaAsync()
        {
            var clinicas = await _context.Clinicas.ToListAsync();
            return clinicas;
        }
    }
}
