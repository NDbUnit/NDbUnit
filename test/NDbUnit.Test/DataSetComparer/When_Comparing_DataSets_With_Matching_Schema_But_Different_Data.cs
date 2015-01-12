using System.Data;
using NDbUnit.Core;
using NUnit.Framework;

namespace NDbUnit.Test.DataSetComparer
{
    public class When_Comparing_DataSets_With_Matching_Schema_But_Different_Data
    {
        public void CanReportNoMatch()
        {
            var firstDataSet = new DataSet();
            firstDataSet.ReadXmlSchema(StreamHelper.ReadOnlyStreamFromFilename(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd"));
            firstDataSet.ReadXml(StreamHelper.ReadOnlyStreamFromFilename(@"Xml\DataSetComparer\FirstDataToCompare.xml"));

            var secondDataSet = new DataSet();
            secondDataSet.ReadXmlSchema(StreamHelper.ReadOnlyStreamFromFilename(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd"));
            secondDataSet.ReadXml(StreamHelper.ReadOnlyStreamFromFilename(@"Xml\DataSetComparer\DifferingDataToCompare.xml"));

            Assert.That(firstDataSet.HasSameSchemaAs(secondDataSet));
            Assert.That(firstDataSet.HasSameDataAs(secondDataSet), Is.False);
        }
    }
}