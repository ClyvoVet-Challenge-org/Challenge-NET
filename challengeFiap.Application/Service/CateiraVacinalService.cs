using challengeFiap.Application.Diagnostics;
using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using Microsoft.Extensions.Logging;

using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace challengeFiap.Application.Service
{
    public class CateiraVacinalService : ICarteiraVacinalService
    {

        private readonly ILogger<CateiraVacinalService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _CarteiraVacinalCreateCounter;


        public CateiraVacinalService(ILogger<CateiraVacinalService> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _CarteiraVacinalCreateCounter = meter.CreateCounter<int>("carteira_vacinal.create", description: "Total criado");

        }
        public async Task<CarteiraVacinal> CreateCarteiraVacinalAsync(CarteiraVacinal carteiraVacinal)
        {
            using var activity = ActivitySource.StartActivity("CreateCarteiraVacinalAsync");
            activity?.SetTag("carteira_vacinal.id", carteiraVacinal.Id_carteiraVacinal);
            activity?.SetTag("carteira_vacinal.name", carteiraVacinal.Nm_vacina);



            await Task.Delay(100);

            var createdCarteiraVacinal = new CarteiraVacinal
            {
                Id_carteiraVacinal = carteiraVacinal.Id_carteiraVacinal,
                Nm_vacina = carteiraVacinal.Nm_vacina,
                Dt_vacina_prevista = carteiraVacinal.Dt_vacina_prevista,
                Dt_vacina_efetuada = carteiraVacinal.Dt_vacina_efetuada,
                St_vacina = carteiraVacinal.St_vacina,
                Id_animal = carteiraVacinal.Id_animal,
            };


            _logger.LogInformation("CarteiraVacinal criar: {CarteiraVacinalId} - {CarteiraVacinalName}", createdCarteiraVacinal.Id_carteiraVacinal, createdCarteiraVacinal.Nm_vacina);

            _CarteiraVacinalCreateCounter.Add(1, new KeyValuePair<string, object?>("carteira_vacinal.id", createdCarteiraVacinal.Id_carteiraVacinal), new KeyValuePair<string, object?>("carteira_vacinal.name", createdCarteiraVacinal.Nm_vacina));

            return createdCarteiraVacinal;

        }

        public async Task<CarteiraVacinal> UpdateCarteiraVacinalAsync(int id_carteira, CarteiraVacinal carteiraVacinal)
        {

            using var activity = ActivitySource.StartActivity("UpdateCarteiraVacinalAsync");
            activity?.SetTag("carteira_vacinal.id", id_carteira);

            var updatedCarteiraVacinal = new CarteiraVacinal
            {
                Id_carteiraVacinal = id_carteira,
                Nm_vacina = carteiraVacinal.Nm_vacina,
                Dt_vacina_prevista = carteiraVacinal.Dt_vacina_prevista,
                Dt_vacina_efetuada = carteiraVacinal.Dt_vacina_efetuada,
                St_vacina = carteiraVacinal.St_vacina,
                Id_animal = carteiraVacinal.Id_animal,
            };

            _logger.LogInformation("Carteira vacinal atualizar: {CarteiraVacinalId} - {CarteiraVacinalName}", updatedCarteiraVacinal.Id_carteiraVacinal, updatedCarteiraVacinal.Nm_vacina);

            _CarteiraVacinalCreateCounter.Add(1, new KeyValuePair<string, object?>("carteira_vacinal.id", updatedCarteiraVacinal.Id_carteiraVacinal), new KeyValuePair<string, object?>("carteira_vacinal.name", updatedCarteiraVacinal.Nm_vacina));

            return updatedCarteiraVacinal;
        }

        public async Task<CarteiraVacinal> GetCarteiraVacinalIdAsync(int id_CarteiraVacinal)
        {
        
        }

        public async Task<CarteiraVacinal> DeleteCarteiraVacinalAsync(int id_CarteiraVacinal)
        {
        
        }

    }
}

