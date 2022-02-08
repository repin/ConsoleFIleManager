using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleFIleManager
{
    class UIO
    {
        private FO fo { get; set; }



        public UIO(FO foo)
        {
            fo = foo;
            Console.WindowWidth = 150;
            Console.WindowHeight = 30;
            Update();
        }

        public void ConstrConsole()
        {

        }

        internal void Update()
        {
            Console.Clear();
            Console.WriteLine($"==================Список директорий, лист {fo.nListDirNow} из {fo.nLstDir}=============================");
            DisplData(fo.lstDir);
            Console.WriteLine("=================================Список файлов================================");
            DisplData(fo.lstFiles);
            Console.WriteLine("=============================Отчет по операции================================");
            Console.WriteLine(fo.cSLWRT);
            Console.Write($"{fo.currentDir}>");


        }

        private void DisplData(string[] lstDir)
        {
            int kLst = 0;

            foreach (string direct in lstDir)
            {
                Console.WriteLine(direct);
            }
            int lenghtLst = lstDir.Length;
            if (lenghtLst < 10)
            {
                kLst = 10 - lenghtLst;
            }
            for (int i = 0; i < kLst; i++)
            {
                Console.WriteLine();
            }
        }
    }
}
