using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;

namespace NDbUnit.Test.MongoDB
{
    
    [TestFixture]
    public class MongoDBCommandBuilderTest : NDbUnit.Test.Common.DbCommandBuilderTestBase
    {
        public override IList<string> ExpectedDataSetTableNames
        {
            get { throw new NotImplementedException(); }
        }

        public override IList<string> ExpectedDeleteAllCommands
        {
            get { throw new NotImplementedException(); }
        }

        public override IList<string> ExpectedDeleteCommands
        {
            get { throw new NotImplementedException(); }
        }

        public override IList<string> ExpectedInsertCommands
        {
            get { throw new NotImplementedException(); }
        }

        public override IList<string> ExpectedInsertIdentityCommands
        {
            get { throw new NotImplementedException(); }
        }

        public override IList<string> ExpectedSelectCommands
        {
            get { throw new NotImplementedException(); }
        }

        public override IList<string> ExpectedUpdateCommands
        {
            get { throw new NotImplementedException(); }
        }

        protected override Core.IDbCommandBuilder GetDbCommandBuilder()
        {
            throw new NotImplementedException();
        }

        protected override string GetXmlSchemaFilename()
        {
            throw new NotImplementedException();
        }
    }
}
