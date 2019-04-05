using System;
using System.IO;

namespace TradeAdvisor.Test
{

    public static class Temporary
    {
        public static Scoped<DirectoryInfo> GetEmptyDirectory()
        {
            var directory = new DirectoryInfo(Path.GetRandomFileName());
            directory.Create();
            return new Scoped<DirectoryInfo>(directory, () => directory.Delete(true));
        }

        public static (DirectoryInfo, IDisposable) GetEmptyDirectory2()
        {
            var directory = new DirectoryInfo(Path.GetRandomFileName());
            directory.Create();
            return (directory, new Disposable(() => directory.Delete(true)));
        }
    }
}
