using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Domain.Interfaces
{
    public interface IprescricaoService
    {
        Task<Prescricao> CreatePrescricaoAsync(Prescricao prescricao);
        Task<Prescricao> UpdatePrescricaoAsync(int id_prescricao, Prescricao prescricao);

        Task<Prescricao> GetPrescricaoIdAsync(int id_Prescricao);
        Task<Prescricao> DeletePrescricaoAsync(int id_Prescricao);
    }
}
