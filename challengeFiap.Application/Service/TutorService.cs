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
    public class TutorService : ItutorService
    {
        private readonly ILogger<TutorService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _tutorCreateCounter;

        private readonly List<Tutor> _GetTutor = new();

        private readonly AppDbContext _context;

        public TutorService(ILogger<TutorService> logger, IMeterFactory meterFactory, AppDbContext context)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _tutorCreateCounter = meter.CreateCounter<int>("tutor.create", description: "Total criado");
            _context = context;
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

        public async Task<Tutor> GetTutorIdAsync(int id_tutor)
        {
            var tutor = await _context.Tutor.FindAsync(id_tutor);

            if (tutor == null)
            {
                _logger.LogWarning("Id não existe: {Id}", id_tutor);
                throw new Exception("Id não foi encontrada");
            }

            return tutor;
        }

        public async Task<Tutor> DeleteTutorAsync(int id_tutor)
        {
            var tutor = await _context.Tutor.FindAsync(id_tutor);

            if (tutor == null)
            {
                _logger.LogWarning("tutor não encontrada para exclusão.");
                throw new Exception("Nao foi inserido");
            }
            else
            {
                _context.Tutor.Remove(tutor);

                _logger.LogInformation("Tutor encontrado para exclusão");

                return tutor;
            }
        }

        public async Task<IEnumerable<Tutor>> GetAllTutorAsync()
        {
            var tutors = await _context.Tutor.ToListAsync();
            return tutors;
        }
    }
}
