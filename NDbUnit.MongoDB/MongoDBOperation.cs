using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NDbUnit.Core.MongoDB
{
    public class MongoDBOperation : DbOperation
    {
        protected override IDbCommand CreateDbCommand(string cmdText)
        {
            return new MongoDBCommand(cmdText);
        }

        protected override IDbDataAdapter CreateDbDataAdapter()
        {
            return new MongoDBDataAdapter();
        }
    }
}
