using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Domain.Interfaces
{
    public interface ItutorService
    {
        Task<Tutor> CreateTutorAsync(Tutor tutor);
        Task<Tutor> UpdateTutorAsync(int id_tutor, Tutor tutor);
    }
}
