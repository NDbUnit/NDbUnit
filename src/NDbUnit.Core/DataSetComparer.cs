using System.Data;
using System.Linq;

namespace NDbUnit.Core
{
    public static class DataSetComparer
    {
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

            foreach (var table in leftTables.Cast<DataTable>())
            {
                if (!rightTables.Contains(table.TableName))
                    return false;

                if (!AreTheSame(table, rightTables[table.TableName]))
                    return false;
            }


            return false;
        }

        private static bool AreTheSame(DataTable left, DataTable right)
        {
            return false;
        }

        private static bool AreTheSame(DataRow left, DataRow right)
        {
            return false;
        }

        private static bool AreTheSame(DataColumn left, DataColumn right)
        {
            return false;
        }
    }
}