using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Domain.Interfaces
{
    public interface IAnimalService
    {
        Task<Animal> CreateAnimalAsync(Animal animal);
        Task<Animal> UpdateAnimalAsync(int id_animal, Animal animal);
    }
}
