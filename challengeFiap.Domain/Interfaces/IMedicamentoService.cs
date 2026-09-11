using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Domain.Interfaces
{
    public interface IMedicamentoService
    {
        Task<Medicamento> CreateMedicamentoAsync(Medicamento medicamento);
        Task<Medicamento> UpdateMedicamentoAsync(int id_medicamento, Medicamento medicamento);

    }
}
