using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Domain.Interfaces
{
    public interface ICarteiraVacinalService
    {
        Task<CarteiraVacinal> CreateCarteiraVacinalAsync(CarteiraVacinal carteiraVacinal);
        Task<CarteiraVacinal> UpdateCarteiraVacinalAsync(int id_carteira, CarteiraVacinal carteiraVacinal);

        Task<CarteiraVacinal> GetCarteiraVacinalIdAsync(int id_CarteiraVacinal);
        Task<CarteiraVacinal> DeleteCarteiraVacinalAsync(int id_CarteiraVacinal);
    }
}
