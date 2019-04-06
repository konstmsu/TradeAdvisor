using System.IO;

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
    }
}
