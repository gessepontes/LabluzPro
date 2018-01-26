using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces.Repositories.Common;

namespace LabluzPro.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        void UpdateUser(Usuario obj);
        Usuario GetByIdUsuarioPerfil(int? id);
        bool Login(Usuario obj);
    }
}
