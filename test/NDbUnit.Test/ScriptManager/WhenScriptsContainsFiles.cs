using System.IO;
using System.Linq;
using NDbUnit.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace NDbUnit.Test.ScriptManager
{
    [TestFixture]
    public class WhenScriptsContainsFiles
    {
        private const string FIRSTFILE = "file1.sql";

        private const string SECONDFILE = "file2.sql";

        private const string THIRDFILE = "file3.sql";

        private Core.ScriptManager _manager;

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

            _manager = new Core.ScriptManager(fileService);

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
}