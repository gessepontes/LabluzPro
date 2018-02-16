using Dapper.FluentMap.Dommel.Mapping;
using LabluzPro.Domain.Entities;

namespace LabluzPro.Data.Mappings
{
    public class UsuarioPaginaMap : DommelEntityMap<UsuarioPagina>
    {
        public UsuarioPaginaMap()
        {
            ToTable("UsuarioPagina");
            Map(p => p.ID).IsKey().IsIdentity();
        }
    }
}
