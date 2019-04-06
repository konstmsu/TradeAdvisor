using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TradeAdvisor.Utils
{
    public class DirectoryPath
    {
        readonly string Absolute;

        public DirectoryPath(string path) => Absolute = Path.GetFullPath(path);

        public static DirectoryPath CurrentWorkingDirectory =>
            new DirectoryPath(Environment.CurrentDirectory);

        public void Delete() => Directory.Delete(Absolute, recursive: true);

        public void Create() => Directory.CreateDirectory(Absolute);

        public DirectoryPath Parent => new DirectoryPath(Path.GetDirectoryName(Absolute));

        public FilePath GetFile(string path) => new FilePath(Path.Combine(Absolute, path));

        public DirectoryPath GetDirectory(string path) =>
            new DirectoryPath(Path.Combine(Absolute, path));

        public bool Exists => Directory.Exists(Absolute);

        public IEnumerable<FilePath> GetFiles(string template) =>
            Exists
            ? Directory.EnumerateFiles(Absolute, template).Select(f => new FilePath(f))
            : new FilePath[0];
    }
}
