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
    public class EnderecoAnimalService : IenderecoAnimalService
    {
        private readonly ILogger<EnderecoAnimalService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _enderecoAnimalCreateCounter;

        private readonly List<EnderecoAnimal> _GetEnderecoAnimal = new();


        public EnderecoAnimalService(ILogger<EnderecoAnimalService> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _enderecoAnimalCreateCounter = meter.CreateCounter<int>("endereco_animal.create", description: "Total criado");
        }

        public async Task<EnderecoAnimal> CreateEnderecoAnimalAsync(EnderecoAnimal enderecoAnimal)
        {
            using var activity = ActivitySource.StartActivity("CreateEnderecoAnimalAsync");
            activity?.SetTag("endereco_animal.id", enderecoAnimal.Id_endereco_animal);
            activity?.SetTag("endereco_animal.pais", enderecoAnimal.Pais);
            activity?.SetTag("endereco_animal.estado", enderecoAnimal.Estado);
            activity?.SetTag("endereco_animal.cidade", enderecoAnimal.Cidade);
            activity?.SetTag("endereco_animal.bairro", enderecoAnimal.Bairro);
            activity?.SetTag("endereco_animal.logradouro", enderecoAnimal.Logradouro_rua);
            activity?.SetTag("endereco_animal.nr_rua", enderecoAnimal.Nr_rua);
            activity?.SetTag("endereco_animal.complemento", enderecoAnimal.Complemento);
            activity?.SetTag("endereco_animal.cep", enderecoAnimal.Cep);
            activity?.SetTag("endereco_animal.id_animal", enderecoAnimal.Id_animal);

            await Task.Delay(100);

            var createdEnderecoAnimal = new EnderecoAnimal
            {
                Id_endereco_animal = enderecoAnimal.Id_endereco_animal,
                Pais = enderecoAnimal.Pais,
                Estado = enderecoAnimal.Estado,
                Cidade = enderecoAnimal.Cidade,
                Bairro = enderecoAnimal.Bairro,
                Logradouro_rua = enderecoAnimal.Logradouro_rua,
                Nr_rua = enderecoAnimal.Nr_rua,
                Complemento = enderecoAnimal.Complemento,
                Cep = enderecoAnimal.Cep,
                Id_animal = enderecoAnimal.Id_animal
            };

            _logger.LogInformation(
                "EnderecoAnimal created with ID: {Id_endereco_animal}",
                createdEnderecoAnimal.Id_endereco_animal);

            _enderecoAnimalCreateCounter.Add(1,new KeyValuePair<string, object>("status", "success"));

            return createdEnderecoAnimal;
        }
        public async Task<EnderecoAnimal> UpdateEnderecoAnimalAsync(
            int id_endereco,
            EnderecoAnimal enderecoAnimal)
        {
            using var activity = ActivitySource.StartActivity("UpdateEnderecoAnimalAsync");
            activity?.SetTag("endereco_animal.id", id_endereco);

            _logger.LogInformation(
                "Updating EnderecoAnimal with ID: {Id_endereco_animal}",
                id_endereco);

            var updatedEnderecoAnimal = new EnderecoAnimal
            {
                Id_endereco_animal = id_endereco,
                Pais = enderecoAnimal.Pais,
                Estado = enderecoAnimal.Estado,
                Cidade = enderecoAnimal.Cidade,
                Bairro = enderecoAnimal.Bairro,
                Logradouro_rua = enderecoAnimal.Logradouro_rua,
                Nr_rua = enderecoAnimal.Nr_rua,
                Complemento = enderecoAnimal.Complemento,
                Cep = enderecoAnimal.Cep,
                Id_animal = enderecoAnimal.Id_animal
            };

            _logger.LogInformation("EnderecoAnimal updated with ID: {Id_endereco_animal}",updatedEnderecoAnimal.Id_endereco_animal);

            _enderecoAnimalCreateCounter.Add(1,new KeyValuePair<string, object>("status", "success"));

            return updatedEnderecoAnimal;
        }

        public Task<EnderecoAnimal> DeleteEnderecoAnimalAsync(int id_EnderecoAnimal)
        {
            var enderecoAnimalDelete = _GetEnderecoAnimal.FirstOrDefault(c => c.Id_endereco_animal == id_EnderecoAnimal);

            if (enderecoAnimalDelete != null)
            {
                _GetEnderecoAnimal.Remove(enderecoAnimalDelete);
                _logger.LogInformation("Deletado");

                return Task.FromResult(enderecoAnimalDelete);
            }
            else
            {
                _logger.LogWarning("Id de Endereco animal não foi encontrado. ");

                throw new Exception("Id não existente presente");
            }
        }
    }
}
