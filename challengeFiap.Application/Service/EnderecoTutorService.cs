
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
    public class EnderecoTutorService : IenderecoTutorService
    {
        private readonly ILogger<EnderecoTutorService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _enderecoTutorTaxaErro;

        private readonly AppDbContext _context;


        public EnderecoTutorService(ILogger<EnderecoTutorService> logger, IMeterFactory meterFactory, AppDbContext context)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _enderecoTutorTaxaErro = meter.CreateCounter<int>("endereco_tutor.error");
            _context = context;
        }

        public async Task<EnderecoTutor> CreateEnderecoTutorAsync(EnderecoTutor enderecoTutor)
        {
            try
            {
                using var activity = ActivitySource.StartActivity("CreateEnderecoTutorAsync");
                activity?.SetTag("endereco_tutor.id", enderecoTutor.Id_endereco_tutor);
                activity?.SetTag("endereco_tutor.pais", enderecoTutor.Pais);
                activity?.SetTag("endereco_tutor.estado", enderecoTutor.Estado);
                activity?.SetTag("endereco_tutor.cidade", enderecoTutor.Cidade);
                activity?.SetTag("endereco_tutor.bairro", enderecoTutor.Bairro);
                activity?.SetTag("endereco_tutor.logradouro", enderecoTutor.Logradouro_rua);
                activity?.SetTag("endereco_tutor.nr_rua", enderecoTutor.Nr_rua);
                activity?.SetTag("endereco_tutor.complemento", enderecoTutor.Complemento);
                activity?.SetTag("endereco_tutor.cep", enderecoTutor.Cep);
                activity?.SetTag("endereco_tutor.id_tutor", enderecoTutor.Id_tutor);

                await Task.Delay(100);

                var createdEnderecoTutor = new EnderecoTutor
                {
                    Id_endereco_tutor = enderecoTutor.Id_endereco_tutor,
                    Pais = enderecoTutor.Pais,
                    Estado = enderecoTutor.Estado,
                    Cidade = enderecoTutor.Cidade,
                    Bairro = enderecoTutor.Bairro,
                    Logradouro_rua = enderecoTutor.Logradouro_rua,
                    Nr_rua = enderecoTutor.Nr_rua,
                    Complemento = enderecoTutor.Complemento,
                    Cep = enderecoTutor.Cep,
                    Id_tutor = enderecoTutor.Id_tutor
                };

                _logger.LogInformation("Endereço do tutor criado com sucesso: {EnderecoTutorId}", createdEnderecoTutor.Id_endereco_tutor);

                return createdEnderecoTutor;
            }
            catch (Exception ex)
            {
                _enderecoTutorTaxaErro.Add(1);
                _logger.LogError("Erro ao criar endereco tutor: {erro}", ex);
                throw;
            }
        }

        public async Task<EnderecoTutor> UpdateEnderecoTutorAsync(int id_endereco, EnderecoTutor enderecoTutor)
        {
            try
            {
                using var activity = ActivitySource.StartActivity("UpdateEnderecoTutorAsync");
                activity?.SetTag("endereco_tutor.id", id_endereco);

                var updatedEnderecoTutor = new EnderecoTutor
                {
                    Id_endereco_tutor = id_endereco,
                    Pais = enderecoTutor.Pais,
                    Estado = enderecoTutor.Estado,
                    Cidade = enderecoTutor.Cidade,
                    Bairro = enderecoTutor.Bairro,
                    Logradouro_rua = enderecoTutor.Logradouro_rua,
                    Nr_rua = enderecoTutor.Nr_rua,
                    Complemento = enderecoTutor.Complemento,
                    Cep = enderecoTutor.Cep,
                    Id_tutor = enderecoTutor.Id_tutor
                };

                _logger.LogInformation("Endereço do tutor atualizado com sucesso: {EnderecoTutorId}", id_endereco);

                return updatedEnderecoTutor;
            }
            catch (Exception ex)
            {
                _enderecoTutorTaxaErro.Add(1);
                _logger.LogError("Erro ao atualizar endereco tutor: {erro}", ex);
                throw;
            }
        }

        public async Task<EnderecoTutor> DeleteEnderecoTutorAsync(int id_EnderecoTutor)
        {
            var enderecoTutor = await _context.EnderecoTutors.FindAsync(id_EnderecoTutor);

            if (enderecoTutor == null)
            {
                _logger.LogWarning("endereco tutor não encontrada para exclusão.");
                _enderecoTutorTaxaErro.Add(1);
                throw new Exception("Nao foi inserido");
            }
            else
            {
                _context.EnderecoTutors.Remove(enderecoTutor);
                _logger.LogInformation("EnderecoTutor encontrado para exclusão");

                return enderecoTutor;
            }
        }

        public async Task<EnderecoTutor> GetEnderecoTutorIdAsync(int id_endereco)
        {
            var endereco = await _context.EnderecoTutors.FindAsync(id_endereco);
            if (endereco == null)
            {
                _logger.LogWarning("Id não existe: {Id}", id_endereco);
                _enderecoTutorTaxaErro.Add(1);
                throw new Exception("Id não foi encontrada");
            }

            return endereco;
        }

        public async Task<IEnumerable<EnderecoTutor>> GetAllEnderecoTutorAsync()
        {
            var enderecos = await _context.EnderecoTutors.ToListAsync();
            return enderecos;
        }
    }
}

