using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FileSorterLib
{
    public class FileGroupingConfiguration : DisposableDataLayerBase
    {
        public IEnumerable<FileSorterStorageLib.Models.FileGrouping> GetGroupings(string name)
        {
            return base._db.GetGroupingConfig(name);
        }

        public IEnumerable<FileSorterStorageLib.Models.FileGrouping> GetGroupings()
        {
            return base._db.GetGroupingConfig();
        }

        public void AddGrouping(string name, List<string> extensions)
        {
            base._db.InsertGrouping(name, extensions.GroupBy(g => g)
                .Select(ext => ext.Key.ToUpper().Replace(".", "")).ToList());
        }
    }
}
