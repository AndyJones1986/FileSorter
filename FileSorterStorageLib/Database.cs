using System;
using LiteDB;
using System.Collections.Generic;
using System.Text;

namespace FileSorterStorageLib
{
    public class Database : IDisposable
    {
        private LiteDatabase _db { get; set; }
        private LiteCollection<Models.FileData> _files { get { return _db.GetCollection<Models.FileData>("Files"); } }
        private LiteCollection<Models.Configuration> _config { get { return _db.GetCollection<Models.Configuration>("Config"); } }

        public Database()
        {
            if (!System.IO.Directory.Exists(Common.StorageDirectory)) System.IO.Directory.CreateDirectory(Common.StorageDirectory);
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

        public IEnumerable<Models.Configuration> GetConfig()
        {
            return _config.FindAll();
        }

        public IEnumerable<Models.Configuration> GetConfig(Query predicate)
        {
            return _config.Find(predicate);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {

            if (!disposedValue)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
