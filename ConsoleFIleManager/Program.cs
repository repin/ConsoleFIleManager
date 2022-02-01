using System;
using System.IO;


namespace ConsoleFIleManager
{
    class Program
    {

        static void Main(string[] args)
        {
/*            string r = @"C:\test\source\Новый_текстовый_документ.txt";
            string k = "Kakaka";
            FileInfo st = new FileInfo(r);
            string t = st.IsReadOnly.ToString() + Environment.NewLine;
            t = string.Concat(t, st.Attributes.ToString() + Environment.NewLine);
            t = string.Concat(t, st.CreationTime.ToString() + Environment.NewLine);
            t = string.Concat(t, st.Extension.ToString() + Environment.NewLine);
            t = string.Concat(t, st.LastAccessTime.ToString() + Environment.NewLine);
            t = string.Concat(t, st.LastWriteTime.ToString() + Environment.NewLine);
            t = string.Concat(t, st.Length.ToString() + Environment.NewLine);
            Console.WriteLine(t);
            return;*/

            FO foo = new FO();
            UIO ui = new UIO();
            while (foo.exit == false)
            {
                string command = Console.ReadLine();
                foo.CommandRead(command);
                foreach (string p in foo.lstDir)
                {
                    Console.WriteLine(p);
                }
            }
        }
    }
}
