using NDbUnit.Core;
using NUnit.Framework;

namespace NDbUnit.Test.DataSetComparer
{
    [TestFixture]
    public class When_Comparing_DataSets_With_Different_Schemas : DataSetComparerTestBase
    {
        [Test]
        public void CanReportNoMatchOnMissingFK()
        {
            var firstDataSet = BuildDataSet(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd");
            var secondDataSet = BuildDataSet(@"Xml\DataSetComparer\DifferingDataSetWithMissingFKToCompare.xsd");

            Assert.That(firstDataSet.HasTheSameSchemaAs(secondDataSet), Is.False);
        }

        [Test]
        public void CanReportNoMatchOnDifferntDataTypes()
        {
            var firstDataSet = BuildDataSet(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd");
            var secondDataSet = BuildDataSet(@"Xml\DataSetComparer\DifferingDataSetWithDiffColumnTypeToCompare.xsd");

            Assert.That(firstDataSet.HasTheSameSchemaAs(secondDataSet), Is.False);
        }

        [Test]
        public void CanReportNoMatchOnDifferentColumnNames()
        {
            var firstDataSet = BuildDataSet(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd");
            var secondDataSet = BuildDataSet(@"Xml\DataSetComparer\DifferingDataSetWithDiffColumnNameToCompare.xsd");

            Assert.That(firstDataSet.HasTheSameSchemaAs(secondDataSet), Is.False);
        }

        [Test]
        public void CanReportNoMatchOnDifferentTables()
        {
            var firstDataSet = BuildDataSet(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd");
            var secondDataSet = BuildDataSet(@"Xml\DataSetComparer\DifferingDataSetWithDiffTablesToCompare.xsd");

            Assert.That(firstDataSet.HasTheSameSchemaAs(secondDataSet), Is.False);
        }
    }
}