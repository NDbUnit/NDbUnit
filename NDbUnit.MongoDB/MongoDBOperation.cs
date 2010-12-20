using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NDbUnit.Core.MongoDB
{
    public class MongoDBOperation : IDbOperation
    {
        public void Insert(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction)
        {
            throw new NotImplementedException();
        }

        public void InsertIdentity(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction)
        {
            throw new NotImplementedException();
        }

        public void Delete(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction)
        {
            throw new NotImplementedException();
        }

        public void Update(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction)
        {
            throw new NotImplementedException();
        }

        public void Refresh(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction)
        {
            throw new NotImplementedException();
        }
    }
}
