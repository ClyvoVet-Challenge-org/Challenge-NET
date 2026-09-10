using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Domain.Interfaces
{
    public interface IenderecoClinicaService
    {
        Task<EnderecoClinica> CreateEnderecoClinicaAsync(EnderecoClinica enderecoClinica);
        Task<EnderecoClinica> UpdateEnderecoClinicaAsync(int id_endereco_clinica, EnderecoClinica enderecoClinica);

        Task<EnderecoClinica> GetEnderecoClinicaIdAsync(int id_EnderecoClinica);
        Task<EnderecoClinica> DeleteEnderecoClinicaAsync(int id_EnderecoClinica);
    }
}
