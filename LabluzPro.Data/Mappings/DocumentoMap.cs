using Dapper.FluentMap.Dommel.Mapping;
using LabluzPro.Domain.Entities;

namespace LabluzPro.Data.Mappings
{
    public class DocumentoMap : DommelEntityMap<Documento>
    {
        public DocumentoMap()
        {
            ToTable("Documento");
            Map(p => p.ID).IsKey().IsIdentity();
        }
    }
}
