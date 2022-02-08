using System;
using System.IO;


namespace ConsoleFIleManager
{
    class Program
    {

        static void Main(string[] args)
        {
            string pathFile = "FOCurrentPath.ini";
            bool k = File.Exists(pathFile);
            FO foo = new FO();
            if (k)
            {
                string lastUsePath = File.ReadAllText(pathFile);
                foo.currentDir = lastUsePath;
            }
            foo.CommandRead($"ls {foo.currentDir}");
            UIO ui = new UIO(foo);

            while (foo.exit == false)
            {

                string command = Console.ReadLine();
                foo.CommandRead(command);
                ui.Update();
            }
            File.WriteAllText(pathFile, foo.currentDir);
        }
    }
}

