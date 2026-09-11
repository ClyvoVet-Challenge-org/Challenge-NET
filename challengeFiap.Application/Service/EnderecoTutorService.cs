
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
    public class EnderecoTutorService : IenderecoTutorService
    {
        private readonly ILogger<EnderecoTutorService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _enderecoTutorCreateCounter;

        private readonly List<EnderecoTutor> _GetEnderecoTutor = new();


        public EnderecoTutorService(ILogger<EnderecoTutorService> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _enderecoTutorCreateCounter = meter.CreateCounter<int>("endereco_tutor.create", description: "Total criado");
        }

        public async Task<EnderecoTutor> CreateEnderecoTutorAsync(EnderecoTutor enderecoTutor)
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

            _enderecoTutorCreateCounter.Add(1, new KeyValuePair<string, object?>("endereco_tutor.id", createdEnderecoTutor.Id_endereco_tutor), new KeyValuePair<string, object?>("endereco_tutor.id_tutor", createdEnderecoTutor.Id_tutor));

            return createdEnderecoTutor;
        }

        public async Task<EnderecoTutor> UpdateEnderecoTutorAsync(int id_endereco, EnderecoTutor enderecoTutor)
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

            _enderecoTutorCreateCounter.Add(1, new KeyValuePair<string, object?>("endereco_tutor.id", updatedEnderecoTutor.Id_endereco_tutor), new KeyValuePair<string, object?>("endereco_tutor.id_tutor", updatedEnderecoTutor.Id_tutor));

            return updatedEnderecoTutor;
        }

        public async Task<EnderecoTutor> DeleteEnderecoTutorAsync(int id_EnderecoTutor)
        {
            var enderecoTutorDelete = _GetEnderecoTutor.FirstOrDefault(c => c.Id_endereco_tutor == id_EnderecoTutor);
            if (enderecoTutorDelete != null)
            {
                _GetEnderecoTutor.Remove(enderecoTutorDelete);
                _logger.LogInformation("Deletado");

                return enderecoTutorDelete;
            }
            else
            {
                _logger.LogWarning("Id de endereco tutor não foi encontrado. ");

                throw new Exception("Id não existente presente");
            }
        }
    }
}

