using System;
using System.IO.Abstractions;

namespace Domain
{
    public class LocalProject
    {
        private readonly IFileSystem FileSystem;
        public readonly DirectoryInfoBase RootFolder;

        public LocalProject(IFileSystem fileSystem, string rootFolder)
        {
            FileSystem = fileSystem;

            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            if (rootFolder == null)
            {
                throw new ArgumentNullException(nameof(rootFolder));
            }

            if (!FileSystem.Directory.Exists(rootFolder))
            {
                throw new ArgumentException($"{rootFolder} does not exist. Invalid project root folder.");
            }

            RootFolder = FileSystem.DirectoryInfo.FromDirectoryName(rootFolder);
        }
    }
}