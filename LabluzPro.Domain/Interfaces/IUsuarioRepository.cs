using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces.Repositories.Common;

namespace LabluzPro.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        void UpdateUser(Usuario obj);
        Usuario GetByIdUsuarioPerfil(int? id);
        Usuario Login(Usuario obj);
        Usuario GetByIdTokenSenha(Usuario obj);
        void UpdateSenha(string SECURITYSTAMP, string sSenha);
        void SendEmail(string sEmail);
        void Forgot(string sEmail);
    }
}
