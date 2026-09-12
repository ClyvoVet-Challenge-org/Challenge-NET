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
    public class EnderecoClinicaService : IenderecoClinicaService
    {
        private readonly ILogger<EnderecoClinicaService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _enderecoClinicaTaxaErro;

        private readonly AppDbContext _context;


        public EnderecoClinicaService(ILogger<EnderecoClinicaService> logger, IMeterFactory meterFactory, AppDbContext context)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _enderecoClinicaTaxaErro = meter.CreateCounter<int>("endereco_clinica.error");
            _context = context;
        }

        public async Task<EnderecoClinica> CreateEnderecoClinicaAsync(EnderecoClinica enderecoClinica)
        {
            try
            {
                using var activity = ActivitySource.StartActivity("CreateEnderecoClinicaAsync");
                activity?.SetTag("endereco_clinica.id", enderecoClinica.Id_endereco_clinica);
                activity?.SetTag("endereco_clinica.pais", enderecoClinica.Pais);
                activity?.SetTag("endereco_clinica.estado", enderecoClinica.Estado);
                activity?.SetTag("endereco_clinica.cidade", enderecoClinica.Cidade);
                activity?.SetTag("endereco_clinica.bairro", enderecoClinica.Bairro);
                activity?.SetTag("endereco_clinica.logradouro", enderecoClinica.Logradouro_rua);
                activity?.SetTag("endereco_clinica.nr_rua", enderecoClinica.Nr_rua);
                activity?.SetTag("endereco_clinica.complemento", enderecoClinica.Complemento);
                activity?.SetTag("endereco_clinica.cep", enderecoClinica.Cep);
                activity?.SetTag("endereco_clinica.id_clinica", enderecoClinica.Id_clinica);

                await Task.Delay(100);

                var createdEnderecoClinica = new EnderecoClinica
                {
                    Id_endereco_clinica = enderecoClinica.Id_endereco_clinica,
                    Pais = enderecoClinica.Pais,
                    Estado = enderecoClinica.Estado,
                    Cidade = enderecoClinica.Cidade,
                    Bairro = enderecoClinica.Bairro,
                    Logradouro_rua = enderecoClinica.Logradouro_rua,
                    Nr_rua = enderecoClinica.Nr_rua,
                    Complemento = enderecoClinica.Complemento,
                    Cep = enderecoClinica.Cep,
                    Id_clinica = enderecoClinica.Id_clinica
                };


                _logger.LogInformation("Endereço da clínica criado com sucesso: {EnderecoClinicaId}", createdEnderecoClinica.Id_endereco_clinica);

                return createdEnderecoClinica;
            }
            catch (Exception ex)
            {
                _enderecoClinicaTaxaErro.Add(1);
                _logger.LogError("Erro ao criar endereco clinica: {erro}", ex);
                throw;
            }
        }

        public async Task<EnderecoClinica> UpdateEnderecoClinicaAsync(int id_endereco_clinica, EnderecoClinica enderecoClinica)
        {
            try
            {
                using var activity = ActivitySource.StartActivity("UpdateEnderecoClinicaAsync");
                activity?.SetTag("endereco_clinica.id", id_endereco_clinica);

                var updatedEnderecoClinica = new EnderecoClinica
                {
                    Id_endereco_clinica = id_endereco_clinica,
                    Pais = enderecoClinica.Pais,
                    Estado = enderecoClinica.Estado,
                    Cidade = enderecoClinica.Cidade,
                    Bairro = enderecoClinica.Bairro,
                    Logradouro_rua = enderecoClinica.Logradouro_rua,
                    Nr_rua = enderecoClinica.Nr_rua,
                    Complemento = enderecoClinica.Complemento,
                    Cep = enderecoClinica.Cep,
                    Id_clinica = enderecoClinica.Id_clinica
                };

                _logger.LogInformation("Endereço da clínica atualizado com sucesso: {EnderecoClinicaId}", id_endereco_clinica);

                return updatedEnderecoClinica;
            }
            catch (Exception ex)
            {
                _enderecoClinicaTaxaErro.Add(1);
                _logger.LogError("Erro ao atualizar endereco clinica: {erro}", ex);
                throw;
            }
        }

        public async Task<EnderecoClinica> DeleteEnderecoClinicaAsync(int id_EnderecoClinica)
        {
            var enderecoClinica = await _context.EnderecoClinicas.FindAsync(id_EnderecoClinica);

            if (enderecoClinica == null)
            {
                _logger.LogWarning("endereco clinica não encontrada para exclusão.");
                _enderecoClinicaTaxaErro.Add(1);
                throw new Exception("Nao foi inserido");
            }
            else
            {
                _context.EnderecoClinicas.Remove(enderecoClinica);

                _logger.LogInformation("EnderecoClinica encontrado para exclusão");

                return enderecoClinica;
            }
        }

        public async Task<EnderecoClinica> GetEnderecoClinicaIdAsync(int id_endereco_clinica)
        {
            var endereco = await _context.EnderecoClinicas.FindAsync(id_endereco_clinica);
            if (endereco == null)
            {
                _logger.LogWarning("Id não existe: {Id}", id_endereco_clinica);
                _enderecoClinicaTaxaErro.Add(1);
                throw new Exception("Id não foi encontrada");
            }

            return endereco;
        }

        public async Task<IEnumerable<EnderecoClinica>> GetAllEnderecoClinicaAsync()
        {
            var enderecos = await _context.EnderecoClinicas.ToListAsync();
            return enderecos;
        }
    }
}

