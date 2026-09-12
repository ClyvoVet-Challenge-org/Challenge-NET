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
    public class VetClinicasService : IVetClinica
    {
        private readonly ILogger<VetClinicasService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _vetClinicasTaxaErro;

        private readonly AppDbContext _context;

        public VetClinicasService(ILogger<VetClinicasService> logger, IMeterFactory meterFactory, AppDbContext context)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _vetClinicasTaxaErro = meter.CreateCounter<int>("vetclinicas.error");
            _context = context;
        }

        public async Task<VetClinica> CreateVetClinicaAsync(VetClinica VetClinica)
        {
            try
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

                return createdVetClinica;
            }
            catch (Exception ex)
            {
                _vetClinicasTaxaErro.Add(1);
                _logger.LogError("Erro ao criar vetclinica: {erro}", ex);
                throw;
            }
        }

        public async Task<VetClinica> UpdateVetClinicaAsync(int id_VetClinica, VetClinica VetClinica)
        {
            try
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

                return updatedVetClinica;
            }
            catch (Exception ex)
            {
                _vetClinicasTaxaErro.Add(1);
                _logger.LogError("Erro ao atualizar vetclinica: {erro}", ex);
                throw;
            }
        }

        public async Task<VetClinica> GetVetClinicaIdAsync(int id_VetClinica)
        {
            var vetClinica = await _context.VetClinicas.FindAsync(id_VetClinica);
            if (vetClinica == null)
            {
                _logger.LogWarning("Id não existe: {Id}", id_VetClinica);
                _vetClinicasTaxaErro.Add(1);
                throw new Exception("Id não foi encontrada");
            }

            return vetClinica;
        }

        public async Task<VetClinica> DeleteVetClinicaAsync(int id_VetClinica)
        {
            var vetClinica = await _context.VetClinicas.FindAsync(id_VetClinica);

            if (vetClinica == null)
            {
                _logger.LogWarning("vetclinica não encontrada para exclusão.");
                _vetClinicasTaxaErro.Add(1);
                throw new Exception("Nao foi inserido");
            }
            else
            {
                _context.VetClinicas.Remove(vetClinica);

                _logger.LogInformation("VetClinica encontrado para exclusão");

                return vetClinica;
            }
        }

        public async Task<IEnumerable<VetClinica>> GetAllVetClinicaAsync()
        {
            var vetClinicas = await _context.VetClinicas.ToListAsync();
            return vetClinicas;
        }
    }
}


