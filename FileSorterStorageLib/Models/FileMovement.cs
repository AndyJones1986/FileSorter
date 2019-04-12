using System;
using System.Collections.Generic;
using System.Text;

namespace FileSorterStorageLib.Models
{
    public class FileMovement
    {
        public DateTime Date { get; set; }
        public string OldLocation { get; set; }
        public string NewLocation { get; set; }
    }
}
