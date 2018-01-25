using Dapper.FluentMap.Dommel.Mapping;
using LabluzPro.Domain.Entities;

namespace LabluzPro.Data.Mappings
{
    public class TipoMap : DommelEntityMap<Tipo>
    {
        public TipoMap()
        {
            ToTable("Tipo");
            Map(p => p.ID).IsKey().IsIdentity();
        }
    }
}
