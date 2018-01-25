using Dapper.FluentMap.Dommel.Mapping;
using LabluzPro.Domain.Entities;

namespace LabluzPro.Data.Mappings
{
    public class UsuarioMap : DommelEntityMap<Usuario>
    {
        public UsuarioMap()
        {
            ToTable("Usuario");
            Map(p => p.ID).IsKey().IsIdentity();
        }
    }
}
