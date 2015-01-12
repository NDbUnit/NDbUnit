using System.Data;
using System.IO;
using NUnit.Framework;

namespace NDbUnit.Test.DataSetComparer
{
    [TestFixture]
    public class When_Comparing_Matching_DataSets
    {

        [Test]
        public void MyMethod()
        {
            DataSet preOperation = new DataSet();
            preOperation.ReadXmlSchema(ReadOnlyStreamFromFilename(@"Xml\DataSetComparer\"));
            preOperation.ReadXml(ReadOnlyStreamFromFilename(@""));
        }



        private FileStream ReadOnlyStreamFromFilename(string filename)
        {
            return new FileStream(filename, FileMode.Open, FileAccess.Read);
        }    
    }
}