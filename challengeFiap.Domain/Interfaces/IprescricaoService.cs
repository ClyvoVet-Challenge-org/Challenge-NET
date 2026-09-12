using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace challengeFiap.Domain.Interfaces
{
    public interface IprescricaoService
    {
        Task<Prescricao> CreatePrescricaoAsync(Prescricao prescricao);
        Task<Prescricao> UpdatePrescricaoAsync(int id_prescricao, Prescricao prescricao);

        Task<Prescricao> GetPrescricaoIdAsync(int id_prescricao);
        Task<Prescricao> DeletePrescricaoAsync(int id_prescricao);

        Task<IEnumerable<Prescricao>> GetAllPrescricaoAsync();
    }
}
