using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TradeAdvisor.Utils
{
    public class FilePath
    {
        public readonly string Absolute;

        public FilePath(string path) => 
            Absolute = Path.GetFullPath(path);

        public bool Exists => File.Exists(Absolute);

        public string Name => Path.GetFileName(Absolute);

        public override string ToString() => Absolute;

        public IEnumerable<string> ReadLines() => Exists ? File.ReadLines(Absolute) : new string[0];
        public Task WriteLines(IEnumerable<string> lines) => File.WriteAllLinesAsync(Absolute, lines);
    }
}
