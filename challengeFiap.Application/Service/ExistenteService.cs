using challengeFiap.Infrastruture.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Application.Service
{
    public class ExistenteService
    {
        private readonly AppDbContext _context;

        public ExistenteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AnimalExiste(int id_animal)
        {
            return await _context.Animals.FirstOrDefaultAsync(u => u.Id_animal == id_animal) != null; 
        }
        public async Task<bool> ClinicaExiste(int id_clinica)
        {
            return await _context.Clinicas.FirstOrDefaultAsync(u => u.Id_clinica == id_clinica) != null;
        }

    }
}
