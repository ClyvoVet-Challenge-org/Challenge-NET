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

        Task<Veterinario> GetVeterinarioIdAsync(int id_Veterinario);
        Task<Veterinario> DeleteVeterinarioAsync(int id_Veterinario);
    }
}
