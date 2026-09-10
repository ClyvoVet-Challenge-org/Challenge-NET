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
    public class VeterinariosService : IVeterinarioService
    {
        private readonly ILogger<VeterinariosService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _veterinarioCreateCounter;


        public VeterinariosService(ILogger<VeterinariosService> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _veterinarioCreateCounter = meter.CreateCounter<int>("veterinario.create", description: "Total criado");
        }

        public async Task<Veterinario> CreateVeterinarioAsync(Veterinario veterinario)
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

            _veterinarioCreateCounter.Add(1, new KeyValuePair<string, object?>("veterinario.id", createdVeterinario.Id_vet));

            return createdVeterinario;
        }

        public async Task<Veterinario> UpdateVeterinarioAsync(int id_veterinario, Veterinario veterinario)
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

            _veterinarioCreateCounter.Add(1, new KeyValuePair<string, object?>("veterinario.id", updatedVeterinario.Id_vet));

            return updatedVeterinario;
        }
        
        public async Task<Veterinario> GetVeterinarioIdAsync(int id_Veterinario)
        {
        
        }

        public async Task<Veterinario> DeleteVeterinarioAsync(int id_Veterinario)
        {
        
        }


    }
}
