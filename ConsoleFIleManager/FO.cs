using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleFIleManager
{
    /*
     * Вывод дерева файловой системы с условием “пейджинга”
        ls C:\Source -p 2
        Копирование каталога
        cp C:\Source D:\Target
        Копирование файла
        cp C:\source.txt D:\target.txt
        Удаление каталога рекурсивно
        rm C:\Source
        Удаление файла
        rm C:\source.txt
        Вывод информации
        file C:\source.txt
     */
    class FO
    {
        private string cu;
        private string[] listDirectory;

        public string[] lstDir
        {
            get { return listDirectory; }
            set { listDirectory = value; }
        }

        public string currentDir
        {
            get { return cu; }
            set { cu = value; }
        }
        private int nListDirectories;

        public int nLstDir
        {
            get { return nListDirectories; }
            set { nListDirectories = value; }
        }

        private string[] listFiles;

        public string[] lstFiles
        {
            get { return listFiles; }
            set { listFiles = value; }
        }

        private string ConsoleWrite;

        public string cSLWRT
        {
            get { return ConsoleWrite; }
            set { ConsoleWrite = value; }
        }

        private int nListDirectoryNow;

        public int nListDirNow
        {
            get { return nListDirectoryNow; }
            set { nListDirectoryNow = value; }
        }

        public bool exit { get; set; }

        public FO()
        {
            exit = false;
            GetTreePath(Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Разбор команды, запуск действия
        /// </summary>
        /// <param name="command">команда из основной программы</param>
        /// <returns>
        /// -1 - произошла ошибка
        /// 1 - выполнение произошло успешно
        /// </returns>
        public int CommandRead(string command)
        {
            int result;
            string[] comSplit = command.Split(" ");
            if (comSplit.Length == 1 || comSplit.Length > 4)
            {
                ConsoleError(-1);
                return -1;
            }
            else
            {
                switch (comSplit[0])
                {
                    case "ls":
                        if (comSplit.Length == 2)
                        {
                            result = GetTreePath(comSplit[1]);
                            return result;
                        }
                        else if (comSplit.Length == 4 && comSplit[2] == "-p")
                        {
                            result = GetTreePath(comSplit[1], comSplit[3]);
                            return result;
                        }
                        return -1;


                    case "rm":
                        result = Delete(comSplit[1]);
                        return result;
                        break;
                    case "file":
                        result = FileInfo(comSplit[1]);
                        return result;
                        break;
                    case "cp":
                        result = Copy(comSplit[1], comSplit[2]);
                        return result;
                    case "exit":
                        exit = true;
                        return 0;
                        break;

                    default:
                        ConsoleError(-1);
                        return -1;
                }
            }

        }
        /// <summary>
        /// Выводит 10 папок и файлов по пути, при наличии. Поддерживает листинг папок. Листинг файлов не производится.
        /// </summary>
        /// <param name="path">Путь</param>
        /// <param name="nList">Номер страницы листа</param>
        /// <returns></returns>
        private int GetTreePath(string path, string nList = "1")
        {
            int n = int.Parse(nList);
            string[] pathlist = Directory.GetDirectories(path);
            string[] fileList = Directory.GetFiles(path);
            if (pathlist.Length < n * 10 - 10)
            {
                lstDir = new string[10];
                ConsoleError(-2);
                return -1;
            }
            string[] newPathList = new string[10];
            int k = 0;
            for (int i = n - 1; i < pathlist.Length && i < n + 9; i++)
            {
                newPathList[k] = pathlist[i];
                k++;
            }
            string[] newFilelist = new string[10];
            if (fileList.Length > 0)
            {
                for (int i = 0; i < fileList.Length; i++)
                {
                    newFilelist[i] = fileList[i];
                }
            }
            lstDir = newPathList;
            lstFiles = newFilelist;
            currentDir = path;
            nListDirNow = n;
            nListDirectories = pathlist.Length / 10+1;
            return 0;
        }

        private int FileInfo(string path)
        {
            int k = FileOrDirectory(path);
            if (k == 1)
            {
                FileInfo file = new FileInfo(path);
                string fileInfo = $"Информация о файле {Path.GetFileName(path)}" + Environment.NewLine;
                fileInfo = string.Concat(fileInfo, $"Размер файла {file.Length} байт" + Environment.NewLine);
                fileInfo = string.Concat(fileInfo, $"Запрет на редактирование: {file.IsReadOnly}" + Environment.NewLine);
                fileInfo = string.Concat(fileInfo, $"Дата создания: {file.CreationTime}" + Environment.NewLine);
                fileInfo = string.Concat(fileInfo, $"Дата последнего изменения: {file.LastWriteTime}" + Environment.NewLine);
                fileInfo = string.Concat(fileInfo, $"Дата последнего открытия: {file.LastAccessTime}" + Environment.NewLine);
                Console.WriteLine(fileInfo);
                return 0;
            }
            else
            {
                return -1;
            }

        }
        /// <summary>
        /// Определяет необходимо удалить файл или папку, запускает соответствующий метод.
        /// </summary>
        /// <param name="path">путь к файлу или папке</param>
        /// <returns></returns>
        private int Delete(string path)
        {
            int t = FileOrDirectory(path);
            if (t == -1)
            {
                return -1;
            }
            else if (t == 1)
            {
                DelFile(path);
            }
            else
            {
                DelDir(path);
            }
            return 0;
        }

        /// <summary>
        /// Удаляет директорию
        /// </summary>
        /// <param name="path">путь к директории</param>
        private void DelDir(string path)
        {
            var pathsInDir = Directory.EnumerateFileSystemEntries(path);
            foreach (string anyPath in pathsInDir)
            {
                int k = FileOrDirectory(anyPath);
                if (k == 1)
                {
                    DelFile(anyPath);
                }
                else
                {
                    DelDir(anyPath);
                }
            }
            Directory.Delete(path);
            return;
        }

        /// <summary>
        /// Удаляет файл
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        private void DelFile(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        /// Определяет что необходимо скопировать файл или папку, запускает соответствующий метод.
        /// </summary>
        /// <param name="path">начальный путь</param>
        /// <param name="pathTarget">конечный путь</param>
        /// <returns></returns>
        private int Copy(string path, string pathTarget)
        {
            int t = FileOrDirectory(path);
            if (t == -1)
            {
                return -1;
            }
            else if (t == 1)
            {
                CopyFile(path, pathTarget);
            }
            else
            {
                CopyDir(path, pathTarget);
            }

            return 0;
        }

        /// <summary>
        /// Копирование директории и всего, что есть в директории.
        /// </summary>
        /// <param name="path">Путь к директории.</param>
        /// <param name="pathTarget">Путь к итоговой папке.</param>
        /// <returns></returns>
        private int CopyDir(string path, string pathTarget)
        {
            string directoryName = Path.GetFileName(path);
            string newNameDirectory = Path.Combine(pathTarget, directoryName);
            try
            {
                Directory.CreateDirectory(newNameDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            var pathInDir = Directory.EnumerateFileSystemEntries(path);
            foreach (string pathsInDirectory in pathInDir)
            {
                if (FileOrDirectory(pathsInDirectory) == 1)
                {
                    string nameFile = Path.GetFileName(pathsInDirectory);
                    nameFile = Path.Combine(newNameDirectory, nameFile);
                    CopyFile(pathsInDirectory, nameFile);
                }
                else
                {
                    CopyDir(pathsInDirectory, newNameDirectory);
                }
            }


            return 1;
        }

        /// <summary>
        /// Копирует файл.
        /// </summary>
        /// <param name="pathFile">Текущий путь файла.</param>
        /// <param name="pathTarget">Путь файла после копирования.</param>
        /// <returns></returns>
        private int CopyFile(string pathFile, string pathTarget)
        {
            try
            {
                File.Copy(pathFile, pathTarget);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Метод возвращает файл или директория направленны пользователем.
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>
        /// -1 - направлен неверный путь, директории или файла по этому пути не существует,
        /// 1 - путь указывает на файл,
        /// 2 - путьуказывает на директорию.
        /// </returns>
        private int FileOrDirectory(string path)
        {
            bool t = File.Exists(path);
            bool l = Directory.Exists(path);
            if (t == false && l == false)
            {
                return -1;
            }
            else if (t == true)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        /// <summary>
        /// Выводит в поле cSLWRT сообщение об ошибке
        /// </summary>
        /// <param name="error"></param>
        private void ConsoleError(int error)
        {
            switch (error)
            {
                case -1:
                    cSLWRT = "Неверная команда или количество аргументов, воспользуйтесь командой HELP для просмотра списка команд";
                    break;
                case -2:
                    cSLWRT = "На данном листе списка папок ничего нет, на каждом листе отображается 10 папок";
                    break;
                default:
                    cSLWRT = "Неизвестная ошибка";
                    break;
            }
        }

    }
}
