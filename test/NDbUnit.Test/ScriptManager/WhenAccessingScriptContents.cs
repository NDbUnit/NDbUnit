using System.Linq;
using NDbUnit.Core;
using NUnit.Framework;

namespace NDbUnit.Test.ScriptManager
{
    [TestFixture]
    public class WhenAccessingScriptContents
    {
        [Test]
        public void Can_Iterate_Through_File_Contents()
        {
            Core.ScriptManager manager = new Core.ScriptManager(new FileSystemService());

            manager.AddWithWildcard("TestScripts", "testscript*.sql");

            Assert.AreEqual(3, manager.ScriptContents.Count());

            Assert.AreEqual("sql script01", manager.ScriptContents.ElementAt(0));
            Assert.AreEqual("sql script02", manager.ScriptContents.ElementAt(1));
            Assert.AreEqual("sql script03", manager.ScriptContents.ElementAt(2));

        }
    }
}