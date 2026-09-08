using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Domain.Interfaces
{
    public interface IenderecoTutorService
    {
        Task<EnderecoTutor> CreateEnderecoTutorAsync(EnderecoTutor enderecoTutor);
        Task<EnderecoTutor> UpdateEnderecoTutorAsync(int id_endereco, EnderecoTutor enderecoTutor);
    }
}
