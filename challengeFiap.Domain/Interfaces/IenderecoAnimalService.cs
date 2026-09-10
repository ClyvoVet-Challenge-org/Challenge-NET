using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Domain.Interfaces
{
    public interface IenderecoAnimalService
    {
        Task<EnderecoAnimal> CreateEnderecoAnimalAsync(EnderecoAnimal enderecoAnimal);
        Task<EnderecoAnimal> UpdateEnderecoAnimalAsync(int id_endereco, EnderecoAnimal enderecoAnimal);


        Task<EnderecoAnimal> GetEnderecoAnimalIdAsync(int id_EnderecoAnimal);
        Task<EnderecoAnimal> DeleteEnderecoAnimalAsync(int id_EnderecoAnimal);
    }
}
