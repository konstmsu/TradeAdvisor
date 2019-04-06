using System;
using System.IO;
using TradeAdvisor.Utils;

namespace TradeAdvisor
{
    public static class KnownPaths
    {
        public static DirectoryPath SolutionRoot => solutionRoot.Value;
        static readonly Lazy<DirectoryPath> solutionRoot = new Lazy<DirectoryPath>(FindSolutionRoot);

        static DirectoryPath FindSolutionRoot()
        {
            var current = DirectoryPath.CurrentWorkingDirectory;

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
