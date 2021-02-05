using System;
using System.IO;
using System.Linq;

namespace OldFilesChecker {

    class Program {

        /// <summary>
        /// Deletes the oldest <paramref name="oldFiles"/> number of files of extension <paramref name="extension"/>
        /// if there are at least <paramref name="minimumFiles"/> number of files in the directory with path <paramref name="folderPath"/>.
        /// </summary>
        /// <param name="folderPath">Path of the directory that contains the files to check.</param>
        /// <param name="minimumFiles">Minimum number of files that must exist in the directory.</param>
        /// <param name="oldFiles">Number of files to delete.</param>
        /// <param name="extension">
        /// Extension of the files to check, e.g. <c>txt</c>.
        /// If it's not specified then all the files will be checked.
        /// </param>
        static void Main(string folderPath, int minimumFiles, int oldFiles, string extension = "*") {
            var files = new DirectoryInfo(folderPath)
                .GetFiles($"*.{extension}")
                .OrderBy(f => f.LastWriteTime)
                .ToArray();

            if (files.Length < minimumFiles) {
                Console.WriteLine($"The directory {folderPath} only contains {files.Length} files. " +
                    "Verify the extension and minimum files arguments.");
                return;
            }

            foreach (var file in files[..oldFiles]) {
                file.Delete();
            }
        }
    }
}
