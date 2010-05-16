using System;
using System.Collections.Generic;
using System.IO;


namespace NDbUnit.Core
{
    public interface IFileSystemService
    {
        IEnumerable<FileInfo> GetFilesInCurrentDirectory(string fileSpec);
        IEnumerable<FileInfo> GetFilesInSpecificDirectory(string pathSpec, string fileSpec);
        FileInfo GetSpecificFile(string fileSpec);
    }
}
