using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace challengeFiap.Domain.Interfaces
{
    public interface IAnimalService
    {
        Task<Animal> CreateAnimalAsync(Animal animal);
        Task<Animal> UpdateAnimalAsync(int id_animal, Animal animal);
        Task<Animal> DeleteAnimalAsync(int id_animal);

        Task<Animal> GetAnimalAsync(int id_animal);

        Task<IEnumerable<Animal>> GetAllAnimalsAsync();
    }
}
