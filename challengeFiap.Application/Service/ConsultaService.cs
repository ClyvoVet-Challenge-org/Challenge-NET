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
using System.Text;

namespace challengeFiap.Application.Service
{
    public class ConsultaService : IConsultaService
    {
        private readonly ILogger<ConsultaService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _consultaCreateCounter;

        private readonly List<Consulta> _GetConsulta = new();

        private readonly AppDbContext _context;

        public ConsultaService(ILogger<ConsultaService> logger, IMeterFactory meterFactory, AppDbContext context)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _consultaCreateCounter = meter.CreateCounter<int>("consulta.create", description: "Total criado");
            _context = context;
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

        public async Task<Consulta> DeleteConsultaAsync(int id_Consulta)
        {
            var consulta = await _context.Consultas.FindAsync(id_Consulta);

            if (consulta == null)
            {
                _logger.LogWarning("consulta não encontrada para exclusão.");
                throw new Exception("Nao foi inserido");
            }
            else
            {
                _context.Consultas.Remove(consulta);

                _logger.LogInformation("Consulta encontrado para exclusão");

                return consulta;
            }
        }

        public async Task<Consulta> GetConsultaIdAsync(int id_consulta)
        {
            var consulta = await _context.Consultas.FindAsync(id_consulta);
            if (consulta == null)
            {
                _logger.LogWarning("Id não existe: {Id}", id_consulta);
                throw new Exception("Id não foi encontrada");
            }

            return consulta;
        }

        public async Task<IEnumerable<Consulta>> GetAllConsultaAsync()
        {
            var consultas = await _context.Consultas.ToListAsync();
            return consultas;
        }
    }
}
