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

    }
}
