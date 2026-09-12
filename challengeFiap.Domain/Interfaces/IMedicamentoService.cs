using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace challengeFiap.Domain.Interfaces
{
    public interface IMedicamentoService
    {
        Task<Medicamento> CreateMedicamentoAsync(Medicamento medicamento);
        Task<Medicamento> UpdateMedicamentoAsync(int id_medicamento, Medicamento medicamento);

        Task<Medicamento> GetMedicamentoIdAsync(int id_medicamento);
        Task<Medicamento> DeleteMedicamentoAsync(int id_medicamento);

        Task<IEnumerable<Medicamento>> GetAllMedicamentoAsync();
    }
}
