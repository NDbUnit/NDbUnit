using System.IO;
using NDbUnit.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace NDbUnit.Test.ScriptManager
{
    [TestFixture]
    public class WhenAddingASpecificFileSpec
    {
        private Core.ScriptManager _manager;

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

            _manager = new Core.ScriptManager(fileService);
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