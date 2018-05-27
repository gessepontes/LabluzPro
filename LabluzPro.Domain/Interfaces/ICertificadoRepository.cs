using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces.Repositories.Common;
using System.Collections.Generic;

namespace LabluzPro.Domain.Interfaces
{
    public interface ICertificadoRepository : IRepositoryBase<Certificado>
    {
        IEnumerable<Certificado> GetAllVencidos();
    }
}
