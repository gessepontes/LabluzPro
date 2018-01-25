using Dapper.FluentMap.Dommel.Mapping;
using LabluzPro.Domain.Entities;

namespace LabluzPro.Data.Mappings
{
    public class CertificadoMap : DommelEntityMap<Certificado>
    {
        public CertificadoMap()
        {
            ToTable("Certificado");
            Map(p => p.ID).IsKey().IsIdentity();
        }
    }
}
