using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using System.Linq;
using NDbUnit.Core;

namespace NDbUnit.Test
{
    public class ScriptManagerTest
    {

        [TestFixture]
        public class When_Clearing_the_Scripts
        {
            [Test]
            public void All_Scripts_Are_Removed()
            {
                const string FIRSTFILE = "file1.sql";
                const string SECONDFILE = "file2.sql";

                var mocks = new MockRepository();

                var fileService = mocks.Stub<IFileSystemService>();
                SetupResult.For(fileService.GetSpecificFile(FIRSTFILE)).Return(new FileInfo(FIRSTFILE));
                SetupResult.For(fileService.GetSpecificFile(SECONDFILE)).Return(new FileInfo(SECONDFILE));

                mocks.ReplayAll();

                var manager = new ScriptManager(fileService);

                manager.AddSingle(FIRSTFILE);
                manager.AddSingle(SECONDFILE);

                Assert.IsNotEmpty(manager.Scripts);

                manager.ClearAll();

                Assert.IsEmpty(manager.Scripts);

            }
        }

        [TestFixture]
        public class When_Scripts_Contains_Files
        {
            private const string FIRSTFILE = "file1.sql";

            private const string SECONDFILE = "file2.sql";

            private const string THIRDFILE = "file3.sql";

            private ScriptManager _manager;

            private MockRepository _mocks;

            [SetUp]
            public void _Setup()
            {
                _mocks = new MockRepository();

                var fileService = _mocks.Stub<IFileSystemService>();
                SetupResult.For(fileService.GetSpecificFile(FIRSTFILE)).Return(new FileInfo(FIRSTFILE));
                SetupResult.For(fileService.GetSpecificFile(SECONDFILE)).Return(new FileInfo(SECONDFILE));
                SetupResult.For(fileService.GetSpecificFile(THIRDFILE)).Return(new FileInfo(THIRDFILE));

                _mocks.ReplayAll();

                _manager = new ScriptManager(fileService);

                _manager.AddSingle(FIRSTFILE);
                _manager.AddSingle(SECONDFILE);
                _manager.AddSingle(THIRDFILE);
            }

            [Test]
            public void Can_Return_Correct_Count_of_Script_Files()
            {
                Assert.AreEqual(3, _manager.Count());
            }

            [Test]
            public void Can_Return_List_In_Order_Added()
            {
                Assert.AreEqual(FIRSTFILE, _manager.Scripts.ElementAt(0).Name);
                Assert.AreEqual(SECONDFILE, _manager.Scripts.ElementAt(1).Name);
                Assert.AreEqual(THIRDFILE, _manager.Scripts.ElementAt(2).Name);

            }

        }

        [TestFixture]
        public class When_Adding_Multiple_Files_At_Once
        {
            [Test]
            public void Can_Add_In_Order_Sorted_By_Name()
            {
                const string FIRSTFILE = "file1.sql";
                const string SECONDFILE = "file2.sql";
                const string THIRDFILE = "file3.sql";

                var mocks = new MockRepository();

                var fileService = mocks.Stub<IFileSystemService>();
                SetupResult.For(fileService.GetFilesInSpecificDirectory(".", "*.*")).Return(new FileInfo[] { new FileInfo(SECONDFILE), new FileInfo(THIRDFILE), new FileInfo(FIRSTFILE) });

                mocks.ReplayAll();

                var manager = new ScriptManager(fileService);

                manager.AddWithWildcard(".", "*.*");

                Assert.AreEqual(FIRSTFILE, manager.Scripts.ElementAt(0).Name);
                Assert.AreEqual(SECONDFILE, manager.Scripts.ElementAt(1).Name);
                Assert.AreEqual(THIRDFILE, manager.Scripts.ElementAt(2).Name);
            }
        }

        [TestFixture]
        public class When_Adding_Scripts_from_an_Invalid_Path
        {
            [Test]
            public void Can_Bubble_Underlying_Exception_Up_To_Caller()
            {
                ScriptManager manager = new ScriptManager(new FileSystemService());
                Assert.Throws<DirectoryNotFoundException>(() => manager.AddWithWildcard("somefolderthatdoesntexist", "*.sql"));
            }
        }

        [TestFixture]
        public class When_Accessing_Script_Contents
        {
            [Test]
            public void Can_Iterate_Through_File_Contents()
            {
                ScriptManager manager = new ScriptManager(new FileSystemService());

                manager.AddWithWildcard("..\\..\\TestScripts", "testscript*.sql");

                Assert.AreEqual(3, manager.ScriptContents.Count());

                Assert.AreEqual("sql script01", manager.ScriptContents.ElementAt(0));
                Assert.AreEqual("sql script02", manager.ScriptContents.ElementAt(1));
                Assert.AreEqual("sql script03", manager.ScriptContents.ElementAt(2));

            }
        }

        [TestFixture]
        public class When_Invoking_Scripts
        {
            [Test]
            public void Can_Invoke_Scripts_Successfully()
            {
                //you cannot connect to the DB you're planning to create until AFTER you create it, so you must first connect to
                //something 'safe' (like 'master') when executing any DB-create or DB drop scripts!
                var db = new NDbUnit.Core.SqlClient.SqlDbUnitTest(DbConnection.SqlScriptTestsConnectionString);
                db.Scripts.AddSingle("..\\..\\TestScripts\\sqlserver-drop-create-testdb.sql");
                db.ExecuteScripts();

                //once the DB has been created, its possible to connect to the new DB and invoke any other scripts as needed
                db = new NDbUnit.Core.SqlClient.SqlDbUnitTest(DbConnection.SqlConnectionString);
                db.Scripts.AddWithWildcard("..\\..\\TestScripts\\", "sqlserver-testdb*.sql");
                db.ExecuteScripts();

            }
        }

        [TestFixture]
        public class When_Adding_a_Specific_FileSpec
        {
            private ScriptManager _manager;

            private string _theFileSpec;

            private MockRepository _mocks;

            [SetUp]
            public void _Setup()
            {
                _theFileSpec = "file1.sql";

                _mocks = new MockRepository();

                var fileService = _mocks.Stub<IFileSystemService>();
                SetupResult.For(fileService.GetSpecificFile(_theFileSpec)).Return(new FileInfo(_theFileSpec));

                _mocks.ReplayAll();

                _manager = new ScriptManager(fileService);
                _manager.AddSingle(_theFileSpec);
            }

            [Test]
            public void Collection_Contains_FileSpec_if_File_Exists()
            {
                bool found = false;

                foreach (FileInfo script in _manager.Scripts)
                {
                    if (script.Name == _theFileSpec)
                    {
                        found = true;
                        break;
                    }
                }

                Assert.IsTrue(found);

            }

            [Test]
            public void Collection_Doesnt_Contain_FileSpec_if_File_Doesnt_Exist()
            {
                const string NONEXISTENTFILE = "somefilethatdoesntexist.sql";
                _manager.AddSingle(NONEXISTENTFILE);

                bool found = false;

                foreach (FileInfo script in _manager.Scripts)
                {
                    if (script.Name == NONEXISTENTFILE)
                    {
                        found = true;
                        break;
                    }
                }

                Assert.IsFalse(found);

            }

            [TearDown]
            public void _TearDown()
            {
                _mocks.VerifyAll();
            }

        }
    }
}
