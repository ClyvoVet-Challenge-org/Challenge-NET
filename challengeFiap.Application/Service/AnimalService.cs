using challengeFiap.Application.Diagnostics;
using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;



namespace challengeFiap.Application.Service
{
    public class AnimalService : IAnimalService
    {
        private readonly ILogger<AnimalService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _animalCreateCounter;
        private readonly Counter<int> _animalUpdateCounter;
        private readonly List<Animal> _AnimalGet= new();


        public AnimalService(ILogger<AnimalService> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _animalCreateCounter = meter.CreateCounter<int>("animal.create", description: "Total criado");
            _animalUpdateCounter = meter.CreateCounter<int>("animal.update", description: "Total de animais atualizados");

        }

        public async Task<Animal> CreateAnimalAsync(Animal animal)
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

            _animalCreateCounter.Add(1, new KeyValuePair<string, object?>("animal.id", createdAnimal.Id_animal), new KeyValuePair<string, object?>("animal.name", createdAnimal.Nm_animal));

            return createdAnimal;

        }

        public async Task<Animal> UpdateAnimalAsync(int id_animal, Animal animal)
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

            _animalUpdateCounter.Add(1,new KeyValuePair<string, object?>("animal.id", updatedAnimal.Id_animal));
            return updatedAnimal;
        }

        public async Task<Animal> GetAnimalIdAsync(int id_animal)
        {
            var animalget = _AnimalGet.FirstOrDefault(a => a.Id_animal == id_animal);
            if (animalget == null)
            {
                _logger.LogWarning("Id não existe");

                throw new Exception("Id  não foi encontrada");
            }

            return animalget;
        }
        public Task<Animal> DeleteAnimalAsync(int id_animal)
        {
            var animalDelete = _AnimalGet.FirstOrDefault(a => a.Id_animal == id_animal);
            if (animalDelete != null) {

                _AnimalGet.Remove(animalDelete);
                _logger.LogInformation("Deletado");

                return Task.FromResult(animalDelete);

            } else
            {
                _logger.LogWarning("Id de animal  não foi encontrado. ");

                throw new Exception("Id não existente presente");
            }
        }
    }
}
