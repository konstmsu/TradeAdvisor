using System;
using System.IO;
using TradeAdvisor.Utils;

namespace TradeAdvisor.Test
{
    public static class Temporary
    {
        public static Scoped<DirectoryPath> GetEmptyDirectory()
        {
            var directory = new DirectoryPath(Path.GetRandomFileName());
            directory.Create();
            return Scoped.Create(directory, () => directory.Delete());
        }
    }
}
