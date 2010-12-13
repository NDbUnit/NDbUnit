using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NDbUnit.Core.MongoDB
{
    public class MongoDBDataAdapter : IDbDataAdapter
    {
        public DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
        {
            throw new NotImplementedException();
        }

        public int Fill(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public IDataParameter[] GetFillParameters()
        {
            throw new NotImplementedException();
        }

        public int Update(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public MissingMappingAction MissingMappingAction
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public MissingSchemaAction MissingSchemaAction
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ITableMappingCollection TableMappings
        {
            get { throw new NotImplementedException(); }
        }

        public IDbCommand SelectCommand
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IDbCommand InsertCommand
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IDbCommand UpdateCommand
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IDbCommand DeleteCommand
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
