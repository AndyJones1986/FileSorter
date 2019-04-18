using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using FileSorterStorageLib;
using FileSorterStorageLib.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace FileSorterLib
{
    public abstract class DisposableDataLayerBase : IDisposable
    {
        protected Database _db { get; set; }

        protected DisposableDataLayerBase()
        {
            this._db = new Database();
        }

        public virtual void EmptyDB()
        {
            _db.ClearFiles();
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
