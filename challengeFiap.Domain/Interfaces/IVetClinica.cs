using challengeFiap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace challengeFiap.Domain.Interfaces
{
    public interface IVetClinica
    {
        Task<VetClinica> CreateVetClinicaAsync(VetClinica VetClinica);
        Task<VetClinica> UpdateVetClinicaAsync(int id_VetClinica, VetClinica VetClinica);
    }
}
