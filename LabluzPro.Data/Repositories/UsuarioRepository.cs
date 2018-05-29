using Dapper;
using LabluzPro.Data.Repositories.Common;
using LabluzPro.Domain.Diversos;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
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

            if (returnId != 0 && obj.PaginaSelecionada.Count > 0)
            {
                foreach (var item in obj.PaginaSelecionada)
                {
                    conn.Execute(@"INSERT UsuarioPagina(idUsuario,idPagina) values(@idUsuario,@idPagina)", new { idUsuario = returnId, idPagina = item });
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

            foreach (var item in obj.PaginaSelecionada)
            {
                conn.Execute(@"INSERT UsuarioPagina(idUsuario,idPagina) values(@idUsuario,@idPagina)", new { idUsuario = obj.ID, idPagina = item });
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

        public void UpdateSenha(string SECURITYSTAMP, string sSenha)
        {
            sSenha = Diverso.GenerateMD5(sSenha);

            conn.Execute("UPDATE Usuario SET sSenha=@sSenha WHERE SECURITYSTAMP=@SECURITYSTAMP; ", new {sSenha, SECURITYSTAMP });
        }

        public Usuario GetByIdUsuarioPerfil(int? id)
        {
            Usuario p = GetById(id);

            p.UsuarioPagina = conn.Query<UsuarioPagina, Pagina, UsuarioPagina>(
                @"SELECT * FROM dbo.UsuarioPagina up INNER JOIN Pagina p ON up.idPagina = p.ID WHERE idUsuario = @ID",
                    map: (usuarioPagina, pagina) =>
                    {
                        usuarioPagina.Pagina = pagina;
                        return usuarioPagina;
                    },
                    param: new { p.ID }).ToList();

            return p;
        }

        public Usuario Login(Usuario obj)
        {
            Usuario p = conn.Query<Usuario>("SELECT TOP(1) * FROM Usuario WHERE sEmail =@sEmail AND sSenha = @sSenha ", new { obj.sEmail, obj.sSenha }).FirstOrDefault();

            if (p != null)
            {
                p.UsuarioPagina = conn.Query<UsuarioPagina, Pagina, UsuarioPagina>(
                    @"SELECT * FROM dbo.UsuarioPagina up INNER JOIN Pagina p ON up.idPagina = p.ID WHERE idUsuario = @ID",
                        map: (usuarioPagina, pagina) =>
                        {
                            usuarioPagina.Pagina = pagina;
                            return usuarioPagina;
                        },
                        param: new { p.ID }).ToList();
            }

            return p;
        }

        public void SendEmail(string sEmail)
        {
            string _body, strBody, conteudo;
            string sTitulo = "";
            int Count = 0;

            CertificadoRepository certificadoRepository = new CertificadoRepository();
            ContratoRepository contratoRepository = new ContratoRepository();
            DocumentoRepository documentoRepository = new DocumentoRepository();

            conteudo = "";

            foreach (Certificado item in certificadoRepository.GetAllVencidos())
            {
                conteudo += "<tr><td font-weight:bold'><b>Certificado: </b>" + item.sNome + "</td></tr>";
                Count = Count + 1;
            }

            foreach (Contrato item in contratoRepository.GetAllVencidos())
            {
                conteudo += "<tr><td font-weight:bold'><b>Contrato: </b>" + item.sNome + "</td></tr>";
                Count = Count + 1;
            }

            foreach (Documento item in documentoRepository.GetAllVencidos())
            {
                conteudo += "<tr><td font-weight:bold'><b>Documeto: </b>" + item.sNome + "</td></tr>";
                Count = Count + 1;
            }


            _body = "<table style='border - collapse:collapse; border - spacing:0; Margin - left:auto; Margin - right:auto; width: 600px; background - color:#ffffff;font-size:14px;table-layout:fixed'><tbody><tr><td style='padding:0;vertical-align:top;text-align:left'><div><div style='font-size:32px;line-height:32px'>&nbsp;</div></div>";
            _body += "<table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><h1 style='font - style:normal; font - weight:700; Margin - bottom:18px; Margin - top:0; font - size:36px; line - height:44px; font - family:Ubuntu,sans - serif; color:#565656;text-align:center'><strong style='font-weight:bold'>Aviso de Vencimento</strong></h1>";
            _body += "</td></tr></tbody></table><table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><div style='min - height:20px'>&nbsp;</div>" + conteudo + "</td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div><u></u></div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div style='min-height:14px'>&nbsp;</div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><p style='font-style:normal;font-weight:400;Margin-bottom:0;Margin-top:0;line-height:24px;font-family:Ubuntu,sans-serif;color:#787778;font-size:16px'><em>Equipe </em>LabLuz agradece sua preferência.</p></td></tr></tbody></table><div style='font-size:32px;line-height:32px'>&nbsp;</div></td></tr></tbody></table>";


            strBody = "";
            strBody = strBody + "<html>";
            strBody = strBody + "<head>";
            strBody = strBody + "<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>";
            strBody = strBody + "<title>Untitled Document</title>";
            strBody = strBody + "</head>";
            strBody = strBody + "<body>";

            strBody = strBody + "<table style='font-family: verdana; font-size: 11px; color: #000000;' width='100%'>";
            strBody = strBody + "<tr align=center><td colspan=2><img src='cid:Imagem1' /></td></tr>";
            strBody = strBody + "<tr align=center><td colspan=2></td></tr>";

            strBody = strBody + "<tr><td font-weight:bold'><p><p></td></tr> ";
            strBody = strBody + _body;
            strBody = strBody + "</table> ";
            strBody = strBody + "<br><br>";
            strBody = strBody + "Esta é uma  mensagem automática enviada pelo sistema. Não precisa responder.";
            strBody = strBody + "</body>";
            strBody = strBody + "</html>";

            sTitulo = "Alerta de vencimentos";

            if (Count > 0)
            {
                Diverso.SendEmail(sEmail, sTitulo, strBody, null);
            }

        }

        public void Forgot(string sEmail)
        {
            string _body = "";
            string strBody = "";
            string sTitulo = "";
            string sToken = Guid.NewGuid().ToString("D");

            Usuario p = conn.Query<Usuario>("SELECT TOP(1) * FROM Usuario WHERE sEmail =@sEmail ", new { sEmail }).FirstOrDefault();

            conn.Execute("UPDATE Usuario SET SECURITYSTAMP=@SECURITYSTAMP  WHERE ID = @ID;", new { p.ID, SECURITYSTAMP = sToken });

            var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json").Build();

            string sSite = builder.GetSection(key: "AppConfiguration")["Url"];

            _body += "<table style='border - collapse:collapse; border - spacing:0; Margin - left:auto; Margin - right:auto; width: 600px; background - color:#ffffff;font-size:14px;table-layout:fixed'><tbody><tr><td style='padding:0;vertical-align:top;text-align:left'><div><div style='font-size:32px;line-height:32px'>&nbsp;</div></div>";
            _body += "<table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><h1 style='font - style:normal; font - weight:700; Margin - bottom:18px; Margin - top:0; font - size:36px; line - height:44px; font - family:Ubuntu,sans - serif; color:#565656;text-align:center'><strong style='font-weight:bold'>Mudança de Senha</strong></h1>";
            _body += "</td></tr></tbody></table><table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><div style='min - height:20px'>&nbsp;</div></td></tr></tbody></table><table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><p style='font - style:normal; font - weight:400; Margin - bottom:24px; Margin - top:0; line - height:24px; font - family:Ubuntu,sans - serif; color:#787778;font-size:16px'>Segue o link para mudança de sua senha.</p></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div><u></u><a style='border-radius:3px;display:inline-block;font-size:14px;font-weight:700;line-height:24px;padding:13px 35px 12px 35px;text-align:center;text-decoration:none!important;color:#fff;font-family:Ubuntu,sans-serif;background-color:#a42532' href=\"{1}\" target='_blank'>Clique aqui</a><u></u></div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div style='min-height:14px'>&nbsp;</div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><p style='font-style:normal;font-weight:400;Margin-bottom:0;Margin-top:0;line-height:24px;font-family:Ubuntu,sans-serif;color:#787778;font-size:16px'><em>Equipe </em>LabLuz agradece sua preferência.</p></td></tr></tbody></table><div style='font-size:32px;line-height:32px'>&nbsp;</div></td></tr></tbody></table>";

            strBody = "";
            strBody = strBody + "<html>";
            strBody = strBody + "<head>";
            strBody = strBody + "<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>";
            strBody = strBody + "<title>Untitled Document</title>";
            strBody = strBody + "</head>";
            strBody = strBody + "<body>";

            strBody = strBody + "<table style='font-family: verdana; font-size: 11px; color: #000000;' width='100%'>";
            strBody = strBody + "<tr align=center><td colspan=2><img src='cid:Imagem1' /></td></tr>";
            strBody = strBody + "<tr align=center><td colspan=2></td></tr>";

            strBody = strBody + "<tr><td font-weight:bold'><p><p></td></tr> ";
            strBody = strBody + string.Format(_body, p.sNome, sSite + @"Login/ResetPassword?Token=" + sToken + "&Email=" + p.sEmail);
            strBody = strBody + "</table> ";
            strBody = strBody + "<br><br>";
            strBody = strBody + "Esta é uma  mensagem automática enviada pelo sistema. Não precisa responder.";
            strBody = strBody + "</body>";
            strBody = strBody + "</html>";

            sTitulo = "Esqueci minha senha";

            Diverso.SendEmail(sEmail, sTitulo, strBody, null);

        }

        public Usuario GetByIdTokenSenha(Usuario obj) => conn.Query<Usuario>("SELECT TOP(1) * FROM Usuario WHERE sEmail =@sEmail AND SECURITYSTAMP=@SECURITYSTAMP ", new { obj.sEmail, obj.SECURITYSTAMP }).FirstOrDefault();
    }
}
