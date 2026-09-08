using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Domain.Interfaces
{
    public interface IVeterinarioService
    {
        Task<Veterinario> CreateVeterinarioAsync(Veterinario veterinario);
        Task<Veterinario> UpdateVeterinarioAsync(int id_veterinario, Veterinario veterinario);
    }
}
