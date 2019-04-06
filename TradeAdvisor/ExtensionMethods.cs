using System.IO;

namespace TradeAdvisor
{
    public static class ExtensionMethods
    {
        public static FileInfo GetFile(this DirectoryInfo directory, string fileName) =>
            new FileInfo(Path.Combine(directory.FullName, fileName));

        public static DirectoryInfo GetDirectory(this DirectoryInfo directory, string childName) =>
            new DirectoryInfo(Path.Combine(directory.FullName, childName));
    }
}
