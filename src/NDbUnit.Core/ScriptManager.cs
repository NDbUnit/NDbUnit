using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NDbUnit.Core
{
    public class ScriptManager
    {
        private IFileSystemService _fileSystem;

        private IList<FileInfo> _scripts = new List<FileInfo>();

        public ScriptManager(IFileSystemService fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IEnumerable<string> ScriptContents
        {
            get
            {
                return GetScriptContents();
            }
        }

        public IEnumerable<FileInfo> Scripts
        {
            get
            {
                return _scripts;
            }
        }

        public void AddWithWildcard(string pathSpec, string fileSpec)
        {
            IEnumerable<FileInfo> files = _fileSystem.GetFilesInSpecificDirectory(pathSpec, fileSpec);
            if (files == null)
                return;
            foreach (var file in files.OrderBy(f => f.Name))
                _scripts.Add(file);
        }

        public void AddSingle(string fileSpec)
        {
            var file = _fileSystem.GetSpecificFile(fileSpec);
            if (file != null)
                _scripts.Add(file);

        }

        public void ClearAll()
        {
            _scripts.Clear();
        }

        public int Count()
        {
            return _scripts.Count;
        }

        private IEnumerable<string> GetScriptContents()
        {
            IList<string> contents = new List<string>();

            foreach (FileInfo script in Scripts)
            {
                using (StreamReader reader = script.OpenText())
                {
                    contents.Add(reader.ReadToEnd());
                }
            }

            return contents;
        }

    }
}
