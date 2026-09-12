using challengeFiap.Application.Diagnostics;
using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using challengeFiap.Infrastruture.Data;
using Microsoft.EntityFrameworkCore;

namespace challengeFiap.Application.Service
{
    public class VeterinariosService : IVeterinarioService
    {
        private readonly ILogger<VeterinariosService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _veterinarioTaxaErro;

        private readonly AppDbContext _context;

        public VeterinariosService(ILogger<VeterinariosService> logger, IMeterFactory meterFactory, AppDbContext context)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _context = context;
            _veterinarioTaxaErro = meter.CreateCounter<int>("veterinario.error");
        }

        public async Task<Veterinario> CreateVeterinarioAsync(Veterinario veterinario)
        {
            try
            {
                using var activity = ActivitySource.StartActivity("CreateVeterinarioAsync");
                activity?.SetTag("veterinario.id", veterinario.Id_vet);
                activity?.SetTag("veterinario.nome", veterinario.Nm_vet);
                activity?.SetTag("veterinario.cpf", veterinario.Cpf_vet);
                activity?.SetTag("veterinario.crmv", veterinario.Crmv_vet);
                activity?.SetTag("veterinario.email", veterinario.Email_vet);
                activity?.SetTag("veterinario.senha", veterinario.Senha_vet);

                await Task.Delay(100);

                var createdVeterinario = new Veterinario
                {
                    Id_vet = veterinario.Id_vet,
                    Nm_vet = veterinario.Nm_vet,
                    Cpf_vet = veterinario.Cpf_vet,
                    Crmv_vet = veterinario.Crmv_vet,
                    Email_vet = veterinario.Email_vet,
                    Senha_vet = veterinario.Senha_vet
                };

                _logger.LogInformation("Veterinário criado com sucesso: {VeterinarioId}", createdVeterinario.Id_vet);

                return createdVeterinario;
            }
            catch (Exception ex)
            {
                _veterinarioTaxaErro.Add(1);
                 _logger.LogError("Erro ao criar veterinario: {erro}", ex);
                throw;
            }

        }

        public async Task<Veterinario> UpdateVeterinarioAsync(int id_veterinario, Veterinario veterinario)
        {
            try
            {
                using var activity = ActivitySource.StartActivity("UpdateVeterinarioAsync");
                activity?.SetTag("veterinario.id", id_veterinario);

                var updatedVeterinario = new Veterinario
                {
                    Id_vet = id_veterinario,
                    Nm_vet = veterinario.Nm_vet,
                    Cpf_vet = veterinario.Cpf_vet,
                    Crmv_vet = veterinario.Crmv_vet,
                    Email_vet = veterinario.Email_vet,
                    Senha_vet = veterinario.Senha_vet
                };

                _logger.LogInformation("Veterinário atualizado com sucesso: {VeterinarioId}", id_veterinario);

                return updatedVeterinario;
            }
            catch (Exception ex)
            {
                _veterinarioTaxaErro.Add(1);
                _logger.LogError("Erro ao criar veterinario: {erro}", ex);
                throw;
            }

        }

        public async Task<Veterinario> GetVeterinarioIdAsync(int id_veterinario)
        {
            var veterinario = await _context.Veterinarios.FindAsync(id_veterinario);

            if (veterinario == null)
            {
                _logger.LogWarning("Id não existe: {Id}", id_veterinario);

                _veterinarioTaxaErro.Add(1);


                throw new Exception("Id não foi encontrada");
            }

            return veterinario;
        }

        public async Task<Veterinario> DeleteVeterinarioAsync(int id_veterinario)
        {
            var veterinario = await _context.Veterinarios.FindAsync(id_veterinario);

            if (veterinario == null)
            {
                _logger.LogWarning("veterinario não encontrada para exclusão.");

                _veterinarioTaxaErro.Add(1);

                throw new Exception("Nao foi inserido");
            }
            else
            {
                _context.Veterinarios.Remove(veterinario);

                _logger.LogInformation("Veterinario encontrado para exclusão");

                return veterinario;
            }
        }

        public async Task<IEnumerable<Veterinario>> GetAllVeterinarioAsync()
        {
            var veterinarios = await _context.Veterinarios.ToListAsync();

            return veterinarios;
        }
    }
}
