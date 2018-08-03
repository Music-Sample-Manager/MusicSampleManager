using System;
using System.IO.Abstractions;

namespace Domain
{
    public class LocalProject
    {
        private readonly IFileSystem FileSystem;
        public readonly string RootFolder;

        public LocalProject(IFileSystem fileSystem, string rootFolder)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            if (rootFolder == null)
            {
                throw new ArgumentNullException(nameof(rootFolder));
            }

            if (fileSystem.DirectoryInfo.FromDirectoryName(rootFolder) == null)
            {
                throw new Exception($"{rootFolder} does not exist. Invalid project root folder.");
            }

            RootFolder = rootFolder;
        }
    }
}