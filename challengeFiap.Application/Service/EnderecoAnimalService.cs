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
    public class EnderecoAnimalService : IenderecoAnimalService
    {
        private readonly ILogger<EnderecoAnimalService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _enderecoAnimalTaxaErro;

        private readonly AppDbContext _context;


        public EnderecoAnimalService(ILogger<EnderecoAnimalService> logger, IMeterFactory meterFactory, AppDbContext context)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _enderecoAnimalTaxaErro = meter.CreateCounter<int>("endereco_animal.error");
            _context = context;
        }

        public async Task<EnderecoAnimal> CreateEnderecoAnimalAsync(EnderecoAnimal enderecoAnimal)
        {
            try
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

                return createdEnderecoAnimal;
            }
            catch (Exception ex)
            {
                _enderecoAnimalTaxaErro.Add(1);
                _logger.LogError("Erro ao criar endereco animal: {erro}", ex);
                throw;
            }
        }
        public async Task<EnderecoAnimal> UpdateEnderecoAnimalAsync(
            int id_endereco,
            EnderecoAnimal enderecoAnimal)
        {
            try
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

                return updatedEnderecoAnimal;
            }
            catch (Exception ex)
            {
                _enderecoAnimalTaxaErro.Add(1);
                _logger.LogError("Erro ao atualizar endereco animal: {erro}", ex);
                throw;
            }
        }

        public async Task<EnderecoAnimal> DeleteEnderecoAnimalAsync(int id_EnderecoAnimal)
        {
            var enderecoAnimal = await _context.EnderecoAnimals.FindAsync(id_EnderecoAnimal);

            if (enderecoAnimal == null)
            {
                _logger.LogWarning("endereco animal não encontrada para exclusão.");
                _enderecoAnimalTaxaErro.Add(1);
                throw new Exception("Nao foi inserido");
            }
            else
            {
                _context.EnderecoAnimals.Remove(enderecoAnimal);
                _logger.LogInformation("EnderecoAnimal encontrado para exclusão");

                return enderecoAnimal;
            }
        }

        public async Task<EnderecoAnimal> GetEnderecoAnimalIdAsync(int id_endereco)
        {
            var endereco = await _context.EnderecoAnimals.FindAsync(id_endereco);
            if (endereco == null)
            {
                _logger.LogWarning("Id não existe: {Id}", id_endereco);
                _enderecoAnimalTaxaErro.Add(1);
                throw new Exception("Id não foi encontrada");
            }

            return endereco;
        }

        public async Task<IEnumerable<EnderecoAnimal>> GetAllEnderecoAnimalAsync()
        {
            var enderecos = await _context.EnderecoAnimals.ToListAsync();
            return enderecos;
        }
    }
}
