using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace challengeFiap.Domain.Interfaces
{
    public interface IenderecoAnimalService
    {
        Task<EnderecoAnimal> CreateEnderecoAnimalAsync(EnderecoAnimal enderecoAnimal);
        Task<EnderecoAnimal> UpdateEnderecoAnimalAsync(int id_endereco, EnderecoAnimal enderecoAnimal);

        Task<EnderecoAnimal> GetEnderecoAnimalIdAsync(int id_endereco);
        Task<EnderecoAnimal> DeleteEnderecoAnimalAsync(int id_endereco);

        Task<IEnumerable<EnderecoAnimal>> GetAllEnderecoAnimalAsync();
    }
}
