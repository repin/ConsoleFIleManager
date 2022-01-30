using System;
using System.IO;


namespace ConsoleFIleManager
{
    class Program
    {

        static void Main(string[] args)
        {
/*            string r = @"C:\test\target";
            string k = "Kakaka";
            string f = Path.Combine(r, k);
            Directory.CreateDirectory(f);
            return;*/

            FO foo = new FO();
            while (foo.exit == false)
            {
                string command = Console.ReadLine();
                foo.CommandRead(command);
            }
        }
    }
}
