using Dapper;
using LabluzPro.Data.Repositories.Common;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LabluzPro.Data.Repositories
{
    public class DocumentoRepository : RepositoryBase<Documento>, IDocumentoRepository
    {
        public override IEnumerable<Documento> GetAll() =>
            conn.Query<Documento, Tipo, Documento>(
            @"SELECT * FROM Documento C INNER JOIN Tipo T ON C.idTipo = T.ID",
            map: (documento, tipo) =>
            {
                documento.Tipo = tipo;
                return documento;
            });

        public override Documento GetById(int? id) =>
            conn.Query<Documento, Tipo, Documento>(
            @"SELECT TOP(1) * FROM Documento C INNER JOIN Tipo T ON C.idTipo = T.ID WHERE C.ID = @id",
            map: (documento, tipo) =>
            {
                documento.Tipo = tipo;
                return documento;
            },
            param: new { id }).FirstOrDefault();

        public override void Update(Documento obj)
        {
            string sql = "";
            string parametros = "";

            if (obj.sImagem != null) parametros = parametros + ",sImagem=@sImagem";

            sql = "UPDATE Documento SET sNumero = @sNumero,sNome = @sNome,dVencimento = @dVencimento,IdTipo =@IdTipo,iCodUsuarioMovimentacao=@iCodUsuarioMovimentacao,dCadastro=@dCadastro " + parametros + " WHERE ID = @ID; ";

            conn.Execute(sql, new { obj.sNumero, obj.sNome, obj.dVencimento, obj.sImagem, obj.IdTipo, obj.iCodUsuarioMovimentacao, obj.dCadastro, obj.ID });

        }

        public IEnumerable<Documento> GetAllVencidos()
        {
            var builder = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json").Build();

            string Dias = builder.GetSection(key: "Dias")["Dias"];

            return conn.Query<Documento, Tipo, Documento>(
            @"SELECT * FROM Documento C INNER JOIN Tipo T ON C.idTipo = T.ID WHERE C.dVencimento <= DATEADD(Day, " + Dias + ", GETDATE())",
            map: (documento, tipo) =>
            {
                documento.Tipo = tipo;
                return documento;
            });



        }
    }
}
