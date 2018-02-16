using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces.Repositories.Common;
using System.Collections.Generic;

namespace LabluzPro.Domain.Interfaces
{
    public interface IUsuarioPaginaRepository : IRepositoryBase<UsuarioPagina>
    {
        IEnumerable<UsuarioPagina> Perfil(int idUsuario = 0);
    }
}
