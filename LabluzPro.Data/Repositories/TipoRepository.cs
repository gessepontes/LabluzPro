using Dapper;
using LabluzPro.Data.Repositories.Common;
using LabluzPro.Domain.Entities;
using LabluzPro.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LabluzPro.Data.Repositories
{
    public class TipoRepository : RepositoryBase<Tipo>, ITipoRepository
    {
        public IEnumerable<Tipo> GetAllTipoDrop(int IdSolicitacao)
        {
            return conn.Query<Tipo>("SELECT ID,sNome FROM Tipo WHERE IdSolicitacao =@IdSolicitacao ORDER BY sNome", new { IdSolicitacao }).ToList();
        }
    }
}
