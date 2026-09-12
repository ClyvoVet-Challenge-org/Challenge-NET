using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace challengeFiap.Domain.Interfaces
{
    public interface ItutorService
    {
        Task<Tutor> CreateTutorAsync(Tutor tutor);
        Task<Tutor> UpdateTutorAsync(int id_tutor, Tutor tutor);

        Task<Tutor> GetTutorIdAsync(int id_tutor);
        Task<Tutor> DeleteTutorAsync(int id_tutor);

        Task<IEnumerable<Tutor>> GetAllTutorAsync();
    }
}
