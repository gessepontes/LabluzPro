using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using LabluzPro.Data.Mappings;

namespace LabluzPro.Data.Repositories.Common
{
    public static class RegisterMappings
    {
        public static void Register()
        {
            FluentMapper.Initialize(c =>
            {
                c.AddMap(new DocumentoMap());
                c.AddMap(new ContratoMap());
                c.AddMap(new CertificadoMap());
                c.AddMap(new TipoMap());
                c.AddMap(new UsuarioMap());
                c.AddMap(new UsuarioPaginaMap());
                c.ForDommel();
            });
        }
    }
}