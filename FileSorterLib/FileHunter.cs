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
    public class FileHunter : IDisposable
    {
        private ConcurrentBag<FileData> _files { get; set; }
        private Database _db { get; set; }

        public FileHunter()
        {
            _db = new Database();
        }

        public IEnumerable<String> GetFilePaths()
        {
            return _db.GetFiles().Select(file => file.CurrentLocation);
        }

        public void EmptyDB()
        {
            _db.ClearFiles();
        }

        public void HuntFiles(string directoryPath)
        {
            Directory.GetDirectories(directoryPath).ForEach(dir =>
            {
                string[] files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
                var batches = files.Batch(50).ToList();
                batches.ForEach(filesBatch =>
                {
                    _files = new ConcurrentBag<FileData>(_db.GetFiles().ToList());
                    List<Task<FileInfo>> fileTasks = filesBatch.Select(f => new Task<FileInfo>(() => GetFileInfo(f))).ToList();
                    try
                    {
                        fileTasks.AsParallel().ForAll(t => t.Start());
                        Task.WaitAll(fileTasks.ToArray());
                        ConcurrentBag<FileData> parsedFile = new ConcurrentBag<FileData>();
                        List<Task> ParseTasks = fileTasks.Where(ft => !ft.IsFaulted).Select(ft => new Task(() => parsedFile.Add(new FileData(ft.Result)))).ToList();
                        ParseTasks.AsParallel().ForAll(t => t.Start());
                        Task.WaitAll(ParseTasks.ToArray());
                        parsedFile.ForEach(x =>
                        {
                            if (_files.Any(f => f.CurrentLocation == x.CurrentLocation))
                            {
                                _db.UpdateFile(x);
                            }
                            else
                            {
                                _db.InsertFile(x);
                            }
                        });
                    }
                    catch (AggregateException ae)
                    {
                        StringBuilder exceptionSB = new StringBuilder();
                        foreach (var ex in ae.Flatten().InnerExceptions)
                        {
                            exceptionSB.AppendLine(ex.ToString());
                        }
                    }
                });
            });

        }


        private FileInfo GetFileInfo(string path)
        {
            return new FileInfo(path);
        }

        public void LoadAllFiles()
        {
            _db.GetFiles();
        }

        public void StoreFiles()
        {

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _files = null;
                    _db.Dispose();

                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
