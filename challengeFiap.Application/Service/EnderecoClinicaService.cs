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
    public class EnderecoClinicaService : IenderecoClinicaService
    {
        private readonly ILogger<EnderecoClinicaService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _enderecoClinicaCreateCounter;

        private readonly List<EnderecoClinica> _GetEndereco = new();


        public EnderecoClinicaService(ILogger<EnderecoClinicaService> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _enderecoClinicaCreateCounter = meter.CreateCounter<int>("endereco_clinica.create", description: "Total criado");
        }

        public async Task<EnderecoClinica> CreateEnderecoClinicaAsync(EnderecoClinica enderecoClinica)
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

            _enderecoClinicaCreateCounter.Add(1, new KeyValuePair<string, object?>("endereco_clinica.id", createdEnderecoClinica.Id_endereco_clinica), new KeyValuePair<string, object?>("endereco_clinica.id_clinica", createdEnderecoClinica.Id_clinica));

            return createdEnderecoClinica;
        }

        public async Task<EnderecoClinica> UpdateEnderecoClinicaAsync(int id_endereco_clinica, EnderecoClinica enderecoClinica)
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

            _enderecoClinicaCreateCounter.Add(1, new KeyValuePair<string, object?>("endereco_clinica.id", updatedEnderecoClinica.Id_endereco_clinica), new KeyValuePair<string, object?>("endereco_clinica.id_clinica", updatedEnderecoClinica.Id_clinica));

            return updatedEnderecoClinica;
        }

        public async Task<EnderecoClinica> DeleteEnderecoClinicaAsync(int id_EnderecoClinica)
        {
            var enderecoClinicaDelete = _GetEndereco.FirstOrDefault(c => c.Id_endereco_clinica == id_EnderecoClinica);
            if (enderecoClinicaDelete != null)
            {
                _GetEndereco.Remove(enderecoClinicaDelete);
                _logger.LogInformation("Deletado");

                return enderecoClinicaDelete;
            }
            else
            {
                _logger.LogWarning("Id de endereco clinica não foi encontrado. ");

                throw new Exception("Id não existente presente");
            }
        }
    }
}

