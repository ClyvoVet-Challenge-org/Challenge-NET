using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Domain.Interfaces
{
    public interface IConsultaService
    {
        Task<Consulta> CreateConsultaAsync(Consulta consulta);
        Task<Consulta> UpdateConsultaAsync(int id_consulta, Consulta consulta);


        Task<Consulta> GetConsultaIdAsync(int id_Consulta);
        Task<Consulta> DeleteConsultaAsync(int id_Consulta);
    }
}
