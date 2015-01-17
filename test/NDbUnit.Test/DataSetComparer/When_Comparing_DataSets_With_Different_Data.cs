using System.Data;
using NDbUnit.Core;
using NUnit.Framework;

namespace NDbUnit.Test.DataSetComparer
{
    public class When_Comparing_DataSets_With_Different_Data : DataSetComparerTestBase
    {
        [Test]
        public void CanReportNoMatch()
        {
            var firstDataSet = BuildDataSet(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd", @"Xml\DataSetComparer\FirstDataToCompare.xml");
            var secondDataSet = BuildDataSet(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd", @"Xml\DataSetComparer\DifferingDataToCompare.xml");

            Assert.That(firstDataSet.HasTheSameDataAs(secondDataSet), Is.False);
        }
    }
}