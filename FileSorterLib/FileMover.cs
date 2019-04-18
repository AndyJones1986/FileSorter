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
using System.Globalization;

namespace FileSorterLib
{
    public class FileMover : DisposableDataLayerBase
    {
        public void MoveFiles(List<string> filePaths, string TargetRoot)
        {
            var groupings = base._db.GetGroupingConfig();
            filePaths.ForEach(file =>
            {
                FileData fileObject = base._db.GetFiles().Where(f => f.CurrentLocation.ToUpperInvariant() == file.ToUpperInvariant()).FirstOrDefault();
                if (fileObject != null)
                {
                    FileInfo fileInfo = fileObject.GetInfo();
                    string newFolder = string.Empty;
                    if (groupings.Any(g => g.Extensions.Contains(fileObject.Extension.Replace(".", "").ToUpper())))
                    {
                        var grpConfig = groupings.First(g => g.Extensions.Contains(fileObject.Extension.Replace(".", "").ToUpper()));
                        if (grpConfig.Extensions.Count == 1)
                        {
                            newFolder = Path.Combine(TargetRoot, grpConfig.Name, fileInfo.CreationTime.Year.ToString());
                        }
                        else
                        {
                            newFolder = Path.Combine(TargetRoot, grpConfig.Name, fileObject.Extension.Replace(".", "").ToUpper(), fileInfo.CreationTime.Year.ToString());
                        }
                    }
                    else
                    {
                        newFolder = Path.Combine(TargetRoot, fileObject.Extension.Replace(".", "").ToUpper(), fileInfo.CreationTime.Year.ToString());
                    }
                    string oldPath = fileObject.CurrentLocation;
                    string newPath = Path.Combine(newFolder, fileInfo.Name);
                    if (!Directory.Exists(newFolder)) { Directory.CreateDirectory(newFolder); }

                    string newName = fileInfo.Name;
                    bool checkingName = true;
                    int count = 0;

                    while (checkingName)
                    {
                        count++;
                        if (File.Exists(Path.Combine(Path.Combine(newFolder, newName))))
                        {
                            newName = string.Format("{0}_{1}", newName, count);
                        }
                        else
                        {
                            checkingName = false;
                        }
                    }


                    fileInfo.CopyTo(Path.Combine(newFolder, newName));
                    var newFile = new FileInfo(Path.Combine(newFolder, newName));
                    if (newFile.Exists)
                    {
                        File.SetCreationTime(Path.Combine(newFolder, newName), File.GetCreationTime(fileInfo.FullName));
                        File.SetCreationTimeUtc(Path.Combine(newFolder, newName), File.GetCreationTimeUtc(fileInfo.FullName));
                        File.Delete(oldPath);
                        fileObject.AddMovement(newPath);
                        base._db.UpdateFile(fileObject);
                    }
                }
            });
        }

    }
}
