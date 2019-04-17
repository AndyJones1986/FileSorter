using System;
using System.Collections.Generic;
using System.Text;

namespace FileSorterStorageLib.Models
{
    public class FileGrouping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Extensions { get; set; }
        public FileGrouping()
        {
            Extensions = new List<string>();
        }
    }
}
