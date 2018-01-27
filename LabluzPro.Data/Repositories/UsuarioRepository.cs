using Dapper;
using LabluzPro.Data.Repositories.Common;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using System.Linq;

namespace LabluzPro.Data.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public override void Add(Usuario obj)
        {
            string sql = "INSERT INTO Usuario(sNome,sEmail,sSenha,bAtivo,sImagem,sTelefone,iCodUsuarioMovimentacao,dCadastro) ";
            sql = sql + "values(@sNome,@sEmail,@sSenha,@bAtivo,@sImagem,@sTelefone,@iCodUsuarioMovimentacao,@dCadastro); SELECT CAST(SCOPE_IDENTITY() as int)";

            if (obj.sImagem == "") obj.sImagem = "user.png";

            var returnId = conn.Query<int>(sql, new { obj.sNome, obj.sEmail, obj.sSenha, obj.bAtivo, obj.sImagem, obj.sTelefone, obj.iCodUsuarioMovimentacao, obj.dCadastro }).SingleOrDefault();

            if (returnId != 0 && obj.PaginaSelecionado.Count > 0)
            {
                foreach (var idPagina in obj.PaginaSelecionado)
                {
                    conn.Execute(@"INSERT UsuarioPagina(idUsuario,idPagina) values(@idUsuario,@idPagina)", new { idUsuario = returnId, idPagina });
                }

            }
        }

        public override void Update(Usuario obj)
        {
            string sql = "";
            string parametros = "";

            if (obj.sSenha != null) parametros = ",sSenha=@sSenha";
            if (obj.sImagem != "") parametros = parametros + ",sImagem=@sImagem";

            sql = "UPDATE Usuario SET sNome=@sNome,sEmail=@sEmail,bAtivo=@bAtivo,sTelefone=@sTelefone,iCodUsuarioMovimentacao=@iCodUsuarioMovimentacao,dCadastro=@dCadastro " + parametros + " WHERE ID = @ID; ";

            conn.Execute(sql, new { obj.sNome, obj.sEmail, obj.sSenha, obj.bAtivo, obj.sImagem, obj.iCodUsuarioMovimentacao, obj.dCadastro, obj.sTelefone, obj.ID });

            conn.Execute("DELETE FROM UsuarioPagina WHERE idUsuario = @ID; ", new { obj.ID });

            foreach (var idPagina in obj.PaginaSelecionado)
            {
                conn.Execute(@"INSERT UsuarioPagina(idUsuario,idPagina) values(@idUsuario,@idPagina)", new { idUsuario = obj.ID, idPagina });
            }
        }

        public void UpdateUser(Usuario obj)
        {
            string sql = "";
            string parametros = "";

            if (obj.sSenha != null) parametros = ",sSenha=@sSenha";
            if (obj.sImagem != null) parametros = parametros + ",sImagem=@sImagem";

            sql = "UPDATE Usuario SET sNome=@sNome,sTelefone=@sTelefone,iCodUsuarioMovimentacao=@iCodUsuarioMovimentacao,dCadastro=@dCadastro " + parametros + " WHERE ID = @ID; ";

            conn.Execute(sql, new { obj.sNome, obj.sEmail, obj.sSenha, obj.bAtivo, obj.sImagem, obj.iCodUsuarioMovimentacao, obj.dCadastro, obj.sTelefone, obj.ID });
        }

        public Usuario GetByIdUsuarioPerfil(int? id)
        {
            Usuario p = GetById(id);
            p.UsuarioPagina = conn.Query<UsuarioPagina>("SELECT * FROM dbo.UsuarioPagina WHERE idUsuario = " + id).ToList();
            return p;
        }

        public Usuario Login(Usuario obj) => conn.Query<Usuario>("SELECT * FROM Usuario WHERE sEmail =@sEmail AND sSenha = @sSenha ", new { obj.sEmail, obj.sSenha }).FirstOrDefault();
    }
}
