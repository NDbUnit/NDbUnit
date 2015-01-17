using System.Data;
using NUnit.Framework;

namespace NDbUnit.Test.DataSetComparer
{
    [TestFixture]
    public abstract class DataSetComparerTestBase
    {
        public DataSet BuildDataSet(string schemaFile, string dataFile = null)
        {
            var dataSet = new DataSet();
            dataSet.ReadXmlSchema(StreamHelper.ReadOnlyStreamFromFilename(schemaFile));

            if (dataFile != null)
            {
                dataSet.ReadXml(StreamHelper.ReadOnlyStreamFromFilename(dataFile));
            }

            return dataSet;
        }
    }
}