namespace Xaml.Tests.Resources
{
    using System.Collections.Generic;
    using System.Linq;
    using PCLStorage;

    public static class File
    {
        public static string LoadAsString(string path)
        {
            var file = FileSystem.Current.GetFileFromPathAsync(path).Result;
            var str = file.ReadAllTextAsync().Result;
            return str;
        }

        public static IEnumerable<string> GetFiles(string folder)
        {
            var f = FileSystem.Current.GetFolderFromPathAsync(folder).Result;
            var files = f.GetFilesAsync().Result;
            
            return files.Select(file => file.Path);
        }
    }
}