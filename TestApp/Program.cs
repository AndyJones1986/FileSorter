using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            SetGrouping();
            bool finish = false;
            while (finish == false)
            {
                Console.WriteLine("Select Task");
                Console.WriteLine();
                Console.WriteLine("1: hunt files, 2: list files, 3: Empty DB, 4: Move Files, 0: Exit");
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
                    case '4':
                        MoveFiles();
                        break;
                    case '0':
                    default:
                        finish = true;
                        break;
                }
            }

        }

        public static void MoveFiles()
        {
            using (var bus1 = new FileSorterLib.FileHunter())
            using (var bus2 = new FileSorterLib.FileMover())
            {
                bus2.MoveFiles(bus1.GetFilePaths().ToList(), @"\\APLFS115\Users\andy.jones\My Documents\#SortedMyDocuments\AUTOSORTED");
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
            SetGrouping();
        }

        public static void SetGrouping()
        {
            using (var bus = new FileSorterLib.FileGroupingConfiguration())
            {
                List<string> imageExtensions = new List<string>();
                imageExtensions.Add("jpg");
                imageExtensions.Add("jpg_1");
                imageExtensions.Add("jpeg");
                imageExtensions.Add("png");
                imageExtensions.Add("png_1");
                imageExtensions.Add("gif");
                imageExtensions.Add("gif_1");
                bus.AddGrouping("1. Images", imageExtensions);

                List<string> rawImageExtensions = new List<string>();
                rawImageExtensions.Add("DNG");
                bus.AddGrouping("2. Raw Photos", rawImageExtensions);

                List<string> editFile = new List<string>();
                editFile.Add("AFDESIGN");
                editFile.Add("AFDESIGN_1");
                editFile.Add("AFPHOTO");
                editFile.Add("eps");
                editFile.Add("ai");
                bus.AddGrouping("3. Image Edit Files", editFile);

                List<string> sqlExtensions = new List<string>();
                sqlExtensions.Add("SQL");
                bus.AddGrouping("4. SQL", sqlExtensions);

                List<string> videoExtensions = new List<string>();
                videoExtensions.Add("AVI");
                videoExtensions.Add("MOV");
                videoExtensions.Add("wmv");
                bus.AddGrouping("5. Video", videoExtensions);

                List<string> audioExtensions = new List<string>();
                audioExtensions.Add("MP3");
                bus.AddGrouping("6. Audio", audioExtensions);

                List<string> documentExtensions = new List<string>();
                documentExtensions.Add("DOC");
                documentExtensions.Add("DOCX");
                documentExtensions.Add("DOT");
                documentExtensions.Add("DOTX");
                documentExtensions.Add("XLS");
                documentExtensions.Add("XLSX");
                documentExtensions.Add("XLTM");
                documentExtensions.Add("XLTX");
                documentExtensions.Add("TXT");
                bus.AddGrouping("7. Documents", documentExtensions);
            }
        }


        public static void HuntFiles()
        {
            using (var bus = new FileSorterLib.FileHunter())
            {
                Console.WriteLine("Enter Path:");
                var path = Console.ReadLine();
                bus.HuntFiles(path);
            }
        }
    }
}

