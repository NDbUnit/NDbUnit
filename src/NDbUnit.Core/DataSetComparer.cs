using System.Data;
using System.Linq;
using KellermanSoftware.CompareNetObjects;

namespace NDbUnit.Core
{
    public static class DataSetComparer
    {

        private static CompareLogic _comparer = new CompareLogic();

        public static bool HasSameDataAs(this DataSet left, DataSet right)
        {
            //if schemas don't match, no point in proceeding to test any data/content so just bail out early...
            if (!left.HasSameSchemaAs(right))
                return false;

            return false;
        }

        public static bool HasSameSchemaAs(this DataSet left, DataSet right)
        {
            var leftTables = left.Tables;
            var rightTables = right.Tables;

            //if the count of tables fails to match, no point in proceeding
            if (leftTables.Count != rightTables.Count)
                return false;

            //consider tables
            foreach (var table in leftTables.Cast<DataTable>())
            {
                if (!rightTables.Contains(table.TableName))
                    return false;

                if (!AreTheSameSchema(table, rightTables[table.TableName]))
                    return false;
            }


            return true;
        }

        private static bool AreTheSameSchema(DataTable left, DataTable right)
        {
            var config = new ComparisonConfig { IgnoreCollectionOrder = true, CompareChildren = false };
            config.MembersToIgnore.Add("Columns");
            config.MembersToIgnore.Add("Rows");

            _comparer.Config = config;

            var result = _comparer.Compare(left, right);

            if (!result.AreEqual)
                return false;

            //if the count of columns fails to match, no point in proceeding
            if (left.Columns.Count != right.Columns.Count)
                return false;

            foreach (var element in left.Columns)
            {
                var column = (DataColumn)element;

                if (!right.Columns.Contains(column.ColumnName))
                    return false;

                if (!AreTheSameSchema(column, right.Columns[column.ColumnName]))
                    return false;
            }

            return true;
        }

        private static bool AreTheSame(DataRow left, DataRow right)
        {
            return false;
        }

        private static bool AreTheSameSchema(DataColumn left, DataColumn right)
        {
            var config = new ComparisonConfig { IgnoreCollectionOrder = true, CompareChildren = false };
            _comparer.Config = config;

            var result = _comparer.Compare(left, right);

            return result.AreEqual;
        }
    }
}