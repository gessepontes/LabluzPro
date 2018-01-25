using LabluzPro.Data.Mappings;
using LabluzPro.Domain.Interfaces.Repositories.Common;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dommel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace LabluzPro.Data.Repositories.Common
{
    public class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly SqlConnection conn;

        public RepositoryBase()
        {
            if (FluentMapper.EntityMaps.IsEmpty)
            {
                FluentMapper.Initialize(c =>
                {
                    c.AddMap(new DocumentoMap());
                    c.AddMap(new ContratoMap());
                    c.AddMap(new CertificadoMap());
                    c.AddMap(new TipoMap());
                    c.AddMap(new UsuarioMap());

                    c.ForDommel();
                });
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();


            conn = new SqlConnection(config.GetSection(key: "ConnectionStrings")["DefaultConnection"]);

        }

        public virtual void Add(TEntity obj) => conn.Insert(obj);

        public virtual IEnumerable<TEntity> GetAll() => conn.GetAll<TEntity>();

        public virtual TEntity GetById(int? id) => conn.Get<TEntity>(id);

        public virtual void Remove(TEntity obj) => conn.Delete(obj);

        public virtual void Update(TEntity obj) => conn.Update(obj);

        public void Dispose()
        {
            conn.Close();
            conn.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
