using System.Data;
using System.Data.SQLite;

namespace NDbUnit.Core.SqlLite
{
    public class SqlLiteUnitTest : NDbUnitTest
    {
        #region Private Fields

        SqlLiteDbCommandBuilder _sqlLiteDbCommandBuilder = null;
        SqlLiteDbOperation _sqlLiteDbOperation = null;        

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlLiteUnitTest"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string 
        /// used to open the database.
        /// <seealso cref="System.Data.IDbConnection.ConnectionString"/></param>
        public SqlLiteUnitTest(string connectionString)
        {
            _sqlLiteDbCommandBuilder = new SqlLiteDbCommandBuilder(connectionString);
            _sqlLiteDbOperation = new SqlLiteDbOperation();
        }

        #endregion

        #region Protected Overrides

        protected override IDbCommandBuilder GetDbCommandBuilder()
        {
            return _sqlLiteDbCommandBuilder;
        }

        protected override IDbOperation GetDbOperation()
        {
            return _sqlLiteDbOperation;
        }

        protected override void OnGetDataSetFromDb(string tableName, ref System.Data.DataSet dsToFill, System.Data.IDbConnection dbConnection)
        {
            SQLiteCommand selectCommand = _sqlLiteDbCommandBuilder.GetSelectCommand(tableName);
            selectCommand.Connection = dbConnection as SQLiteConnection;
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectCommand);
            adapter.Fill(dsToFill, tableName);
        }

        #endregion
    }
}
