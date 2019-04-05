using System;
using System.IO;

namespace TradeAdvisor
{
    public static class KnownPaths
    {
        public static DirectoryInfo SolutionRoot => solutionRoot.Value;
        static readonly Lazy<DirectoryInfo> solutionRoot = new Lazy<DirectoryInfo>(FindSolutionRoot);

        static DirectoryInfo FindSolutionRoot()
        {
            var current = new DirectoryInfo(Directory.GetCurrentDirectory());

            for (; ; )
            {
                if (current == null)
                    throw new Exception();

                if (current.GetFile("TradeAdvisor.sln").Exists)
                    return current;

                current = current.Parent;
            }
        }
    }
}
