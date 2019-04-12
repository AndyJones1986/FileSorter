using System;
using System.Collections.Generic;
using System.Text;

namespace FileSorterStorageLib.Models
{
    public class FileData
    {
        public int Id { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
        public DateTime FirstFound { get; set; }
        public DateTime LastSeen { get; set; }
        public List<FileMovement> Movements { get; set; }
        public string CurrentLocation { get; set; }

        public int Hash { get; set; }
        public FileData()
        {

        }

        public FileData(System.IO.FileInfo info)
        {
            CurrentLocation = info.FullName;
            Name = info.Name;
            FirstFound = DateTime.Now;
            Movements = new List<FileMovement>();
            Extension = info.Extension;
            Hash = info.GetHashCode();
        }
        public void AddMovement(string newLocation)
        {
            Movements.Add(new FileMovement() { Date = DateTime.Now, NewLocation = newLocation, OldLocation = CurrentLocation });
            CurrentLocation = newLocation;
        }
    }
}
