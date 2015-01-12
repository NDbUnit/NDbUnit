using System.Data;
using NDbUnit.Core;
using NUnit.Framework;

namespace NDbUnit.Test.DataSetComparer
{
    [TestFixture]
    public class When_Comparing_Matching_DataSets
    {
        [Test]
        public void CanReportMatch()
        {
            var firstDataSet = new DataSet();
            firstDataSet.ReadXmlSchema(StreamHelper.ReadOnlyStreamFromFilename(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd"));
            firstDataSet.ReadXml(StreamHelper.ReadOnlyStreamFromFilename(@"Xml\DataSetComparer\FirstDataToCompare.xml"));

            var secondDataSet = new DataSet();
            secondDataSet.ReadXmlSchema(StreamHelper.ReadOnlyStreamFromFilename(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd"));
            secondDataSet.ReadXml(StreamHelper.ReadOnlyStreamFromFilename(@"Xml\DataSetComparer\FirstDataToCompare.xml"));

            Assert.That(firstDataSet.HasSameSchemaAs(secondDataSet));
            Assert.That(firstDataSet.HasSameDataAs(secondDataSet));
        }
    }
}