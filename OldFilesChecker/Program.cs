using System;
using System.IO;
using System.Linq;

namespace OldFilesChecker {

    public class Program {

        /// <summary>
        /// Deletes the oldest number of folders older than some days, or files of a certain extension
        /// if there are at least some number of files in the specified directory with path.
        /// </summary>
        /// <param name="directoryPath">Path of the directory that contains the files to check</param>
        /// <param name="minimumFiles">Minimum number of files that must exist in the directory</param>
        /// <param name="oldFiles">Number of files to delete</param>
        /// <param name="daysOld">Delete all folders older than this number of days. If specified, then
        /// <paramref name="minimumFiles"/> and <paramref name="oldFiles"/> options are ignored</param>
        /// <param name="extension">Extension of the files to check, e.g. <c>txt</c>
        /// If it's not specified then all the files will be checked.</param>
        public static void Main(string directoryPath, int minimumFiles, int oldFiles, int daysOld = 0, string extension = "*") {
            var directory = new DirectoryInfo(directoryPath);

            if (daysOld > 0) {
                DeleteFolders(directory, daysOld);
            }
            else {
                DeleteFiles(directory, minimumFiles, oldFiles, extension);
            }
        }

        public static void DeleteFiles(DirectoryInfo directory, int minimum, int old, string extension) {
            var files = directory
                .GetFiles($"*.{extension}")
                .OrderBy(f => f.LastWriteTime)
                .ToArray();

            if (files.Length < minimum) {
                Console.WriteLine($"The directory {directory.FullName} only contains {files.Length} files. " +
                    "Verify the extension and minimum files arguments.");
                return;
            }

            foreach (var file in files[..old]) {
                Console.WriteLine($"Delete file '{file.Name}'.");
                file.Delete();
            }


        }

        public static void DeleteFolders(DirectoryInfo directory, int daysOld) {
            var folders = directory
                .GetDirectories();

            var oldestFolders = folders
                .Where(f => (DateTime.Now - f.LastWriteTime).TotalDays >= daysOld)
                .ToArray();
            foreach (var folder in oldestFolders) {
                Console.WriteLine($"Delete folder '{folder.Name}' ({folder.LastWriteTime}).");
                folder.Delete(true);
            }

            Console.WriteLine($"Total deleted folders: {oldestFolders.Length}");
        }
    }
}
