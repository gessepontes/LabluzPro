using System.Collections.Generic;
using Dapper;
using LabluzPro.Data.Repositories.Common;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;

namespace LabluzPro.Data.Repositories
{
    public class UsuarioPaginaRepository : RepositoryBase<UsuarioPagina>, IUsuarioPaginaRepository
    {

        public IEnumerable<UsuarioPagina> Perfil(int idUsuario = 0) =>
            conn.Query<Pagina,UsuarioPagina, UsuarioPagina>(
                @"SELECT * FROM Pagina p LEFT JOIN UsuarioPagina up ON p.ID = up.idPagina AND up.idUsuario = @idUsuario",
                    map: (pagina,usuarioPagina) =>
                    {
                        if (usuarioPagina == null) {
                            UsuarioPagina up = new UsuarioPagina();
                            usuarioPagina = up;
                        }

                        usuarioPagina.Pagina = pagina;
                        return usuarioPagina;
                    }, param: new { idUsuario });

    }
}
