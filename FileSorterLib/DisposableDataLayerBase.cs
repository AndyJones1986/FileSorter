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

        public DisposableDataLayerBase()
        {
            this._db = new Database();
        }

        public virtual void EmptyDB()
        {
            _db.ClearFiles();
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
