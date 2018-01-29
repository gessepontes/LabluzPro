using Dapper;
using LabluzPro.Data.Repositories.Common;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LabluzPro.Data.Repositories
{
    public class CertificadoRepository : RepositoryBase<Certificado>, ICertificadoRepository
    {

        public override IEnumerable<Certificado> GetAll() =>
            conn.Query<Certificado, Tipo, Certificado>(
                @"SELECT * FROM Certificado C INNER JOIN Tipo T ON C.idTipo = T.ID",
                    map: (certificado, tipo) =>
                    {
                        certificado.Tipo = tipo;
                        return certificado;
                    });

        public override Certificado GetById(int? id) =>
            conn.Query<Certificado, Tipo, Certificado>(
            @"SELECT TOP(1) * FROM Certificado C INNER JOIN Tipo T ON C.idTipo = T.ID WHERE C.ID = @id",
                map: (certificado, tipo) =>
                {
                    certificado.Tipo = tipo;
                    return certificado;
                },
                param: new { id }).FirstOrDefault();

        public override void Update(Certificado obj)
        {
            string sql = "";
            string parametros = "";

            if (obj.sImagem != null) parametros = parametros + ",sImagem=@sImagem";

            sql = "UPDATE Certificado SET sNumero = @sNumero,sNome = @sNome,dVencimento = @dVencimento,dServico = @dServico,IdTipo =@IdTipo,iCodUsuarioMovimentacao=@iCodUsuarioMovimentacao,dCadastro=@dCadastro " + parametros + " WHERE ID = @ID; ";

            conn.Execute(sql, new { obj.sNumero, obj.sNome, obj.dVencimento, obj.dServico, obj.sImagem,obj.IdTipo, obj.iCodUsuarioMovimentacao, obj.dCadastro, obj.ID });

        }
    }
}
