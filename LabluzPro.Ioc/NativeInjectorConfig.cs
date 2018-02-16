using Microsoft.Extensions.DependencyInjection;
using LabluzPro.Data.Repositories;
using LabluzPro.Domain.Interfaces;

namespace LabluzPro.Ioc
{
    public static class NativeInjectorConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IDocumentoRepository, DocumentoRepository>();
            services.AddScoped<IContratoRepository, ContratoRepository>();
            services.AddScoped<ICertificadoRepository, CertificadoRepository>();
            services.AddScoped<ITipoRepository, TipoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioPaginaRepository, UsuarioPaginaRepository>();
        }
    }
}
