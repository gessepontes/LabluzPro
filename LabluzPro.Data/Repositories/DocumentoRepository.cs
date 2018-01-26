using Dapper;
using LabluzPro.Data.Repositories.Common;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using System.Collections.Generic;
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
    }
}
