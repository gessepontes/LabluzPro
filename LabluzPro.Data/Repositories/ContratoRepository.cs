using Dapper;
using LabluzPro.Data.Repositories.Common;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LabluzPro.Data.Repositories
{
    public class ContratoRepository : RepositoryBase<Contrato>, IContratoRepository
    {
        public override IEnumerable<Contrato> GetAll() =>
            conn.Query<Contrato, Tipo, Contrato>(
            @"SELECT * FROM Contrato C INNER JOIN Tipo T ON C.idTipo = T.ID",
            map: (contrato, tipo) =>
            {
                contrato.Tipo = tipo;
                return contrato;
            });

        public override Contrato GetById(int? id) =>
            conn.Query<Contrato, Tipo, Contrato>(
            @"SELECT TOP(1) * FROM Contrato C INNER JOIN Tipo T ON C.idTipo = T.ID WHERE C.ID = @id",
            map: (contrato, tipo) =>
            {
                contrato.Tipo = tipo;
                return contrato;
            },
            param: new { id }).FirstOrDefault();
    }
}
