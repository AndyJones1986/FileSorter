using System;
using System.Collections.Generic;
using System.Text;

namespace FileSorterStorageLib
{
    public class Common
    {
        public static string StoragePath { get { return System.IO.Path.Combine(StorageDirectory, "FileIndex.db"); } }
        public static string StorageDirectory { get { return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FileSorter"); } }

    }
}
