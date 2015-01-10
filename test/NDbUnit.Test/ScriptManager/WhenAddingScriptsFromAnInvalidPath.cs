using System.IO;
using NDbUnit.Core;
using NUnit.Framework;

namespace NDbUnit.Test.ScriptManager
{
    [TestFixture]
    public class WhenAddingScriptsFromAnInvalidPath
    {
        [Test]
        public void Can_Bubble_Underlying_Exception_Up_To_Caller()
        {
            Core.ScriptManager manager = new Core.ScriptManager(new FileSystemService());
            Assert.Throws<DirectoryNotFoundException>(() => manager.AddWithWildcard("somefolderthatdoesntexist", "*.sql"));
        }
    }
}