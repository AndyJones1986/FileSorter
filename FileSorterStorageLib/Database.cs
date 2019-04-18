using System;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSorterStorageLib
{
    public class Database : IDisposable
    {
        private LiteDatabase _db { get; set; }
        private LiteCollection<Models.FileData> _files { get { return _db.GetCollection<Models.FileData>("Files"); } }
        private LiteCollection<Models.FileGrouping> _groupingConfig { get { return _db.GetCollection<Models.FileGrouping>("Grouping"); } }

        public Database()
        {
            if (!System.IO.Directory.Exists(Common.StorageDirectory))
            {
                System.IO.Directory.CreateDirectory(Common.StorageDirectory);
            }
            _db = new LiteDatabase(Common.StoragePath);
        }

        public IEnumerable<Models.FileData> GetFiles()
        {
            return _files.FindAll();
        }

        public IEnumerable<Models.FileData> GetFiles(Query predicate)
        {
            return _files.Find(predicate);
        }

        public void ClearFiles()
        {
            _files.Delete(x => true);
        }

        public void InsertFile(Models.FileData file)
        {
            _files.Insert(file);
        }

        public void UpdateFile(Models.FileData file)
        {
            file.LastSeen = DateTime.Now;
            _files.Update(file);
        }

        public void InsertGrouping(string name, List<string> extensions)
        {
            IEnumerable<Models.FileGrouping> existingGroups = GetGroupingConfig(name);
            if (existingGroups.Count() > 0)
            {
                foreach (var group in existingGroups)
                {
                    group.Extensions.AddRange(extensions.Distinct().Where(ext => !group.Extensions.Contains(ext)));
                    UpdateGrouping(group);
                }
            }
            else
            {
                Models.FileGrouping group = new Models.FileGrouping();
                group.Name = name;
                group.Extensions.AddRange(extensions);
                _groupingConfig.Insert(group);
            }

        }

        public void UpdateGrouping(Models.FileGrouping group)
        {
            _groupingConfig.Update(group);
        }

        public IEnumerable<Models.FileGrouping> GetGroupingConfig()
        {
            return _groupingConfig.FindAll();
        }

        public IEnumerable<Models.FileGrouping> GetGroupingConfig(string name)
        {
            return _groupingConfig.Find(Query.EQ("Name", name));
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
