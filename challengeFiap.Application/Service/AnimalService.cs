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
using System.Threading.Tasks;

namespace challengeFiap.Application.Service
{
    public class AnimalService : IAnimalService
    {
        private readonly ILogger<AnimalService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _animalTaxaErro;

        private readonly AppDbContext _context;


        public AnimalService(ILogger<AnimalService> logger, IMeterFactory meterFactory, AppDbContext context)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _animalTaxaErro = meter.CreateCounter<int>("animal.error");
            _context = context;
        }

        public async Task<Animal> CreateAnimalAsync(Animal animal)
        {
            try
            {
                using var activity = ActivitySource.StartActivity("CreateAnimalAsync");
                activity?.SetTag("animal.id", animal.Id_animal);
                activity?.SetTag("animal.name", animal.Nm_animal);
                activity?.SetTag("animal.especies", animal.Especie_animal);
                activity?.SetTag("animal.raca", animal.Raca_animal);

                await Task.Delay(100);

                var createdAnimal = new Animal
                {
                    Id_animal = animal.Id_animal,
                    Rg_animal = animal.Rg_animal,
                    Nr_microchip_animal = animal.Nr_microchip_animal,
                    Nm_animal = animal.Nm_animal,
                    Dt_nascimento_animal = animal.Dt_nascimento_animal,
                    Peso_animal = animal.Peso_animal,
                    Especie_animal = animal.Especie_animal,
                    Raca_animal = animal.Raca_animal,
                    Id_tutor = animal.Id_tutor,
                };

                _logger.LogInformation("Animal criado: id_animal -> {animal.id} nm_animal -> {animal.name}", createdAnimal.Id_animal, createdAnimal.Nm_animal);

                return createdAnimal;
            }
            catch (Exception ex)
            {
                _animalTaxaErro.Add(1);
                _logger.LogError("Erro ao criar animal: {erro}", ex);
                throw;
            }
        }

        public async Task<Animal> UpdateAnimalAsync(int id_animal, Animal animal)
        {
            try
            {
                using var activity = ActivitySource.StartActivity("UpdateAnimalAsync");
                activity?.SetTag("animal.id", id_animal);

                var updatedAnimal = new Animal
                {
                    Id_animal = id_animal,
                    Rg_animal = animal.Rg_animal,
                    Nr_microchip_animal = animal.Nr_microchip_animal,
                    Nm_animal = animal.Nm_animal,
                    Dt_nascimento_animal = animal.Dt_nascimento_animal,
                    Peso_animal = animal.Peso_animal,
                    Especie_animal = animal.Especie_animal,
                    Raca_animal = animal.Raca_animal,
                    Id_tutor = animal.Id_tutor,
                };

                _logger.LogInformation("Animal atualizando com sucesso: {Animal} - {AnimalName}", updatedAnimal.Id_animal, updatedAnimal.Nm_animal);

                return updatedAnimal;
            }
            catch (Exception ex)
            {
                _animalTaxaErro.Add(1);
                _logger.LogError("Erro ao atualizar animal: {erro}", ex);
                throw;
            }
        }

        public async Task<Animal> DeleteAnimalAsync(int id_animal)
        {
            var animal = await _context.Animals.FindAsync(id_animal);

            if (animal == null)
            {
                _logger.LogWarning("animal não encontrada para exclusão: {Id}", id_animal);
                _animalTaxaErro.Add(1);
                throw new Exception("Nao foi inserido");
            }
            else
            {
                _context.Animals.Remove(animal);
                _logger.LogInformation("Animal encontrado para exclusão: {Id}", id_animal);
                return animal;
            }
        }

        public async Task<Animal> GetAnimalAsync(int id_animal)
        {
            var animal = await _context.Animals.FindAsync(id_animal);

            if (animal == null)
            {
                _logger.LogWarning("Id não existe: {Id}", id_animal);
                _animalTaxaErro.Add(1);
                throw new Exception("Id não foi encontrada");
            }

            return animal;
        }

        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            var animals = await _context.Animals.ToListAsync();

            return animals;
        }
    }
}
