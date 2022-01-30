using System;
using System.IO;


namespace ConsoleFIleManager
{
    class Program
    {

        static void Main(string[] args)
        {
            /*            var pathTree = Directory.EnumerateFileSystemEntries(@"C:\test\source", "", SearchOption.AllDirectories);
                        foreach (string k in pathTree)
                        {
                            Console.WriteLine(k);
                        }*/

            FO foo = new FO();
            while (foo.exit == false)
            {
                string command = Console.ReadLine();
                foo.CommandRead(command);
                foreach(string p in foo.lstDir)
                {
                    Console.WriteLine(p);
                }
            }
        }
    }
}
