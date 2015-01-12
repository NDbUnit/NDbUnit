using System.Data;
using NDbUnit.Core;
using NUnit.Framework;

namespace NDbUnit.Test.DataSetComparer
{
    public class When_Comparing_Matching_DataSets : DataSetComparerTestBase
    {
        [Test]
        public void CanReportMatch()
        {
            var firstDataSet = BuildDataSet(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd",@"Xml\DataSetComparer\FirstDataToCompare.xml");
            var secondDataSet = BuildDataSet(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd", @"Xml\DataSetComparer\FirstDataToCompare.xml");

            Assert.That(firstDataSet.HasSameSchemaAs(secondDataSet));
            Assert.That(firstDataSet.HasSameDataAs(secondDataSet));
        }
    }
}