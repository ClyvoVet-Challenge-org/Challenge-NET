using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Domain.Interfaces
{
    public interface IClinicaService
    {
        Task<Clinica> CreateadAsync(Clinica clinica);
        Task<Clinica> UpdateClinicaAsync(int id_clinica, Clinica clinica);


        Task<Clinica> GetClinicaIdAsync(int id_Clinica);
        Task<Clinica> DeleteClinicaAsync(int id_Clinica);
    }
}
