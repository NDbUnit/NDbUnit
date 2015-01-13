using System.Data;
using System.Diagnostics;
using System.Linq;
using KellermanSoftware.CompareNetObjects;

namespace NDbUnit.Core
{
    public static class DataSetComparer
    {
        //useful to increase this during debug/testing, default to 1 so that the first non-matching value will abort comparison and return a FALSE
        public static int MAX_COMPARE_ERRORS = 1;

        public static bool HasTheSameDataAs(this DataSet left, DataSet right)
        {
            //if schemas don't match, no point in proceeding to test any data/content so just bail out early...
            if (!left.HasTheSameSchemaAs(right))
                return false;

            foreach (var table in left.Tables.Cast<DataTable>())
            {
                if (!right.Tables.Contains(table.TableName))
                    return false;

                if (!HaveTheSameData(table, right.Tables[table.TableName]))
                    return false;
            }

            return true;
        }

        public static bool HasTheSameSchemaAs(this DataSet left, DataSet right)
        {
            //if the count of tables fails to match, no point in proceeding
            if (left.Tables.Count != right.Tables.Count)
                return false;

            //consider tables
            foreach (var table in left.Tables.Cast<DataTable>())
            {
                if (!right.Tables.Contains(table.TableName))
                    return false;

                if (!HaveTheSameSchema(table, right.Tables[table.TableName]))
                    return false;
            }

            //consider relatioships
            foreach (var relationship in left.Relations.Cast<DataRelation>())
            {
                if (!HaveTheSameSchema(relationship, right.Relations[relationship.RelationName]))
                    return false;
            }

            return true;
        }

        private static bool HaveTheSameSchema(DataTable left, DataTable right)
        {
            //for some reason the CompareNETObjects comparer refuses to respect the config directive to ignore the Rows property
            // so we have to clone the DataTable(s) and then clear the .Rows collection on both before comparing them
            var leftClone = left.Clone();
            var rightClone = right.Clone();

            leftClone.Rows.Clear();
            rightClone.Rows.Clear();

            var config = new ComparisonConfig { IgnoreCollectionOrder = true, CompareChildren = false, MaxDifferences = MAX_COMPARE_ERRORS };

            //this line *should* make the comparer ignore the Rows collection, but it doesn't appear to work
            // so we'll leave it in here just in case this functionality should be resolved at some future point
            config.MembersToIgnore.Add("Rows");

            var comparer = new CompareLogic(config);

            var result = comparer.Compare(leftClone, rightClone);

            if (!result.AreEqual)
            {
                Log(result.DifferencesString);
                return false;
            }

            //if the count of columns fails to match, no point in proceeding
            if (left.Columns.Count != right.Columns.Count)
                return false;

            foreach (var column in left.Columns.Cast<DataColumn>())
            {
                if (!right.Columns.Contains(column.ColumnName))
                    return false;

                if (!HaveTheSameSchema(column, right.Columns[column.ColumnName]))
                    return false;
            }

            return true;
        }

        private static bool HaveTheSameData(DataTable left, DataTable right)
        {
            //if the count of rows fails to match, no point in proceeding
            if (left.Rows.Count != right.Rows.Count)
                return false;

            var config = new ComparisonConfig { IgnoreCollectionOrder = true, MaxDifferences = MAX_COMPARE_ERRORS };
            config.MembersToIgnore.Add("Rows");

            var comparer = new CompareLogic(config);

            var result = comparer.Compare(left, right);

            if (!result.AreEqual)
                Log(string.Format("Expected DataTable: {0}, Actual DataTable: {1}\n{2}", left.TableName,
                    right.TableName, result.DifferencesString));

            return result.AreEqual;
        }

        private static bool HaveTheSameSchema(DataColumn left, DataColumn right)
        {
            var config = new ComparisonConfig { IgnoreCollectionOrder = true, CompareChildren = false, MaxDifferences = MAX_COMPARE_ERRORS };
            var comparer = new CompareLogic(config);

            var result = comparer.Compare(left, right);

            if (!result.AreEqual)
                Log(string.Format("Expected DataColumn: {0}, Actual DataColumn: {1}\n{2}", left.ColumnName,
                    right.ColumnName, result.DifferencesString));

            return result.AreEqual;
        }

        private static bool HaveTheSameSchema(DataRelation left, DataRelation right)
        {
            var config = new ComparisonConfig { IgnoreCollectionOrder = true, CompareChildren = false, MaxDifferences = MAX_COMPARE_ERRORS };
            var comparer = new CompareLogic(config);

            var result = comparer.Compare(left, right);

            if (!result.AreEqual)
                Log(string.Format("Expected DataRelation: {0}, Actual DataRelation: {1}\n{2}", left.RelationName,
                    right.RelationName, result.DifferencesString));

            return result.AreEqual;
        }

        //TODO: wire up Common.Logging here...
        private static void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}