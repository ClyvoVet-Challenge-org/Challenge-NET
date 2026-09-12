using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace challengeFiap.Domain.Interfaces
{
    public interface IenderecoClinicaService
    {
        Task<EnderecoClinica> CreateEnderecoClinicaAsync(EnderecoClinica enderecoClinica);
        Task<EnderecoClinica> UpdateEnderecoClinicaAsync(int id_endereco_clinica, EnderecoClinica enderecoClinica);

        Task<EnderecoClinica> GetEnderecoClinicaIdAsync(int id_endereco_clinica);
        Task<EnderecoClinica> DeleteEnderecoClinicaAsync(int id_endereco_clinica);

        Task<IEnumerable<EnderecoClinica>> GetAllEnderecoClinicaAsync();
    }
}
