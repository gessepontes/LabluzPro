using Dapper.FluentMap.Dommel.Mapping;
using LabluzPro.Domain.Entities;

namespace LabluzPro.Data.Mappings
{
    public class ContratoMap : DommelEntityMap<Contrato>
    {
        public ContratoMap()
        {
            ToTable("Contrato");
            Map(p => p.ID).IsKey().IsIdentity();
        }
    }
}
