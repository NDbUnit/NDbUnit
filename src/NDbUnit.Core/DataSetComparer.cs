using System.Data;

namespace NDbUnit.Core
{
    public static class DataSetComparer
    {
        public static bool HasSameDataAs(this DataSet left, DataSet right)
        {
            return false;
        }
        
        public static bool HasSameSchemaAs(this DataSet left, DataSet right)
        {
            return false;
        }
    }
}