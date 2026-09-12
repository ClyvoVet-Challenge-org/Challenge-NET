using challengeFiap.Application.Diagnostics;
using challengeFiap.Domain.Entities;
using challengeFiap.Domain.Interfaces;
using challengeFiap.Infrastruture.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace challengeFiap.Application.Service
{
    public class CateiraVacinalService : ICarteiraVacinalService
    {

        private readonly ILogger<CateiraVacinalService> _logger;

        private static readonly ActivitySource ActivitySource = new(TelemetryConstants.ServiceName);

        private readonly Counter<int> _carteiraVacinalTaxaErro;
        private readonly AppDbContext _context;


        public CateiraVacinalService(ILogger<CateiraVacinalService> logger, IMeterFactory meterFactory, AppDbContext context)
        {
            _logger = logger;
            var meter = meterFactory.Create(TelemetryConstants.MeterName);
            _carteiraVacinalTaxaErro = meter.CreateCounter<int>("carteira_vacinal.error");
            _context = context;

        }
        public async Task<CarteiraVacinal> CreateCarteiraVacinalAsync(CarteiraVacinal carteiraVacinal)
        {
            try
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


                return createdCarteiraVacinal;
            }
            catch (Exception ex)
            {
                _carteiraVacinalTaxaErro.Add(1);
                _logger.LogError("Erro ao criar carteira vacinal: {erro}", ex);
                throw;
            }

        }

        public async Task<CarteiraVacinal> UpdateCarteiraVacinalAsync(int id_carteira, CarteiraVacinal carteiraVacinal)
        {
            try
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

                return updatedCarteiraVacinal;
            }
            catch (Exception ex)
            {
                _carteiraVacinalTaxaErro.Add(1);
                _logger.LogError("Erro ao atualizar carteira vacinal: {erro}", ex);
                throw;
            }
        }
        public async Task<CarteiraVacinal> DeleteCarteiraVacinalAsync(int id_CarteiraVacinal)
        {
            var carteiraVacinal = await _context.CarteiraVacinals.FindAsync(id_CarteiraVacinal);

            if (carteiraVacinal == null)
            {
                _logger.LogWarning("carteira vacinal não encontrada para exclusão.");
                _carteiraVacinalTaxaErro.Add(1);
                throw new Exception("Nao foi inserido");
            }
            else
            {
                _context.CarteiraVacinals.Remove(carteiraVacinal);

                _logger.LogInformation("CarteiraVacinal encontrado para exclusão");

                return carteiraVacinal;
            }
        }

        public async Task<CarteiraVacinal> GetCarteiraVacinalIdAsync(int id_CarteiraVacinal)
        {
            var carteira = await _context.CarteiraVacinals.FindAsync(id_CarteiraVacinal);
            if (carteira == null)
            {
                _logger.LogWarning("Id não existe: {Id}", id_CarteiraVacinal);
                _carteiraVacinalTaxaErro.Add(1);
                throw new Exception("Id não foi encontrada");
            }

            return carteira;
        }

        public async Task<IEnumerable<CarteiraVacinal>> GetAllCarteiraVacinalAsync()
        {
            var carteiras = await _context.CarteiraVacinals.ToListAsync();
            return carteiras;
        }
    }
}

