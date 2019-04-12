using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool finish = false;

            while (finish == false)
            {
                Console.WriteLine("Select Task");
                Console.WriteLine();
                Console.WriteLine("1: hunt files, 2: list files, 3: Empty DB, 0: Exit");
                var input = Console.ReadKey();
                Console.Clear();
                switch (input.KeyChar)
                {
                    case '1':
                        Console.WriteLine("Searching Files...");
                        HuntFiles();
                        Console.Clear();
                        Console.WriteLine("Done!");
                        Console.WriteLine();
                        Console.WriteLine();
                        break;
                    case '2':
                        ListFiles();
                        break;
                    case '3':
                        ClearFiles();
                        break;
                    case '0':
                        finish = true;
                        break;
                }
            }

        }

        public static void ListFiles()
        {
            using (var bus = new FileSorterLib.FileHunter())
            {
                var paths = bus.GetFilePaths();
                Console.WriteLine(paths.Count().ToString());
                Console.WriteLine("-------------------------------------");
                paths.ToList().ForEach(p => Console.WriteLine(p));

            }
        }

        public static void ClearFiles()
        {
            using (var bus = new FileSorterLib.FileHunter())
            {
                bus.EmptyDB();
            }
        }

        public static void HuntFiles()
        {
            using (var bus = new FileSorterLib.FileHunter())
            {
                bus.HuntFiles(@"\\APLFS115\Users\andy.jones\My Documents\#SortedMyDocuments");

            }
        }
    }
}

