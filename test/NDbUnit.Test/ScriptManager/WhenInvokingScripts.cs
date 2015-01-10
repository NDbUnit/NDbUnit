using NUnit.Framework;

namespace NDbUnit.Test.ScriptManager
{
    [TestFixture]
    public class WhenInvokingScripts
    {
        [Test]
        public void Can_Invoke_Scripts_Successfully()
        {
            //you cannot connect to the DB you're planning to create until AFTER you create it, so you must first connect to
            //something 'safe' (like 'master') when executing any DB-create or DB drop scripts!
            var db = new NDbUnit.Core.SqlClient.SqlDbUnitTest(DbConnection.SqlScriptTestsConnectionString);
            db.Scripts.AddSingle(@"TestScripts\sqlserver-drop-create-testdb.sql");
            db.ExecuteScripts();

            //once the DB has been created, its possible to connect to the new DB and invoke any other scripts as needed
            db = new NDbUnit.Core.SqlClient.SqlDbUnitTest(DbConnection.SqlConnectionString);
            db.Scripts.AddWithWildcard(@"TestScripts\", "sqlserver-testdb*.sql");
            db.ExecuteScripts();

        }
    }
}