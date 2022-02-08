using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleFIleManager
{
    class FO
    {
        private string cu;
        private string[] listDirectory;

        /// <summary>
        /// Список директорий
        /// </summary>
        public string[] lstDir
        {
            get { return listDirectory; }
            set { listDirectory = value; }
        }

        /// <summary>
        /// Текущая выбранная директория
        /// </summary>
        public string currentDir
        {
            get { return cu; }
            set { cu = value; }
        }

        /// <summary>
        /// Количество страниц в списке директорий
        /// </summary>
        private int nListDirectories;

        /// <summary>
        /// Количество страниц в списке директорий
        /// </summary>
        public int nLstDir
        {
            get { return nListDirectories; }
            set { nListDirectories = value; }
        }

        /// <summary>
        /// Список файлов
        /// </summary>
        private string[] listFiles;

        /// <summary>
        /// Список файлов
        /// </summary>
        public string[] lstFiles
        {
            get { return listFiles; }
            set { listFiles = value; }
        }

        /// <summary>
        /// Вывод информации в консоль
        /// </summary>
        private string ConsoleWrite;


        /// <summary>
        /// Вывод информации в консоль
        /// </summary>
        public string cSLWRT
        {
            get { return ConsoleWrite; }
            set { ConsoleWrite = value; }
        }

        /// <summary>
        /// Номер текущей страницы в листинге директорий
        /// </summary>
        private int nListDirectoryNow;

        /// <summary>
        /// Номер текущей страницы в листинге директорий
        /// </summary>
        public int nListDirNow
        {
            get { return nListDirectoryNow; }
            set { nListDirectoryNow = value; }
        }

        /// <summary>
        /// Выход
        /// </summary>
        public bool exit { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
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
        public void CommandRead(string command)
        {
            int result;
            string[] comSplit = CommandSplit(command);
            if (comSplit.Length == 1 || comSplit == null)
            {
                ConsoleError(-1);
                return;
            }
            else
            {
                switch (comSplit[0])
                {
                    case "ls":
                        if (comSplit[3] == null)
                        {
                            result = GetTreePath(comSplit[1]);
                            ConsoleError(result);
                            break;
                        }
                        else if (comSplit[2] == "-p")
                        {
                            result = GetTreePath(comSplit[1], comSplit[3]);
                            ConsoleError(result);
                            break;
                        }
                        ConsoleError(-1);
                        break;

                    case "rm":
                        result = Delete(comSplit[1]);
                        ConsoleError(result);
                        break;
                    case "file":
                        result = FileInfo(comSplit[1]);
                        ConsoleError(result);
                        break;
                    case "cp":
                        result = Copy(comSplit[1], comSplit[2]);
                        ConsoleError(result);
                        break;
                    case "exit":
                        exit = true;
                        break;

                    default:
                        ConsoleError(-1);
                        break;
                }
            }

        }

        /// <summary>
        /// Разрезает строку на комманды
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private string[] CommandSplit(string command)
        {
            string[] array = command.Split("\"");
            if (array.Length == 1)
                array = command.Split(" ");
            else
            {
                array[0] = array[0].Split(" ")[0];
            }
            string[] newArray = new string[4];
            int k = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (string.IsNullOrEmpty(array[i]) || array[i] == " ")
                {
                    continue;
                }
                if (k > 4) return null;
                newArray[k] = array[i];
                k++;
            }
            return newArray;
        }


        /// <summary>
        /// Выводит 10 папок и файлов по пути, при наличии. Поддерживает листинг папок. Листинг файлов не производится.
        /// </summary>
        /// <param name="path">Путь</param>
        /// <param name="nList">Номер страницы листа</param>
        /// <returns></returns>
        /// 

        private int GetTreePath(string path, string nList = "1")
        {
            int n = int.Parse(nList);
            int wtf = FileOrDirectory(path);
            if (wtf!=2)
            {
                return -1;
            }
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
            /*            if (fileList.Length > 0)
                        {
                            for (int i = 0; i < fileList.Length; i++)
                            {
                                newFilelist[i] = fileList[i];
                            }
                        }
            */
            for (int m = 0; m < fileList.Length; m++)
            {
                newFilelist[m % 10] = string.Concat(newFilelist[m % 10]," ", Path.GetFileName(fileList[m]));
            }


            lstDir = newPathList;
            lstFiles = newFilelist;
            currentDir = path;
            nListDirNow = n;
            nListDirectories = pathlist.Length / 10 + 1;
            return 0;
        }

/*        private string[] AddArray(string[] array, string add)
        {
            string[] newArray = new string[array.Length + 1];
            for (int k = 0; k < array.Length; k++)
            {
                newArray[k] = array[k];
            }
            newArray[array.Length] = add;
            return newArray;
        }*/

        /// <summary>
        /// Получает информацию о файле.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private int FileInfo(string path)
        {
            int k = FileOrDirectory(path);
            if (k == 1)
            {
                FileInfo file = new FileInfo(path);
                string fileInfo = $"Информация о файле {Path.GetFileName(path)}: ";
                fileInfo = string.Concat(fileInfo, $"Размер файла {file.Length} байт | ");
                fileInfo = string.Concat(fileInfo, $"Запрет на редактирование: {file.IsReadOnly} | ");
                fileInfo = string.Concat(fileInfo, $"Дата создания: {file.CreationTime} | ");
                fileInfo = string.Concat(fileInfo, $"Дата последнего изменения: {file.LastWriteTime} | ");
                fileInfo = string.Concat(fileInfo, $"Дата последнего открытия: {file.LastAccessTime} | ");
                ConsoleError(fileInfo);
                return -4;
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
            try
            {
                Directory.Delete(path);
            }
            catch (Exception ex)
            {
                ConsoleError(ex.ToString());
                ConsoleError(-3);
            }
            return;
        }

        /// <summary>
        /// Удаляет файл
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        private void DelFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                ConsoleError(ex.ToString());
                ConsoleError(-3);
            }

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
                int result = CopyFile(path, pathTarget);
                return result;
            }
            else
            {
                int result = CopyDir(path, pathTarget);
                return result;
            }
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
                ConsoleError(ex.ToString());
                return -3;
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
                ConsoleError(ex.ToString());
                return -3;
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
                    cSLWRT = "Неверная команда или количество аргументовW";
                    break;
                case -2:
                    cSLWRT = "На данном листе списка папок ничего нет, на каждом листе отображается 10 папок";
                    break;
                case 0:
                    cSLWRT = "Операция прошла успешно";
                    break;
                case -3: //ошибка направлена в перегрузку со строкой ex
                    SaveFileError();
                    break;
                case -4: //не ошибка, вывод информации в консоль
                    break;
                default:
                    cSLWRT = "Неизвестная ошибка";
                    break;
            }
        }
        /// <summary>
        /// Выводит в поле cSLWRT сооющение
        /// </summary>
        /// <param name="error"></param>
        private void ConsoleError(string error)
        {
            cSLWRT = error;

        }

        /// <summary>
        /// Сохраняет ошибки в файл errors\\random_name_exception.txt
        /// </summary>
        private void SaveFileError()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "errors\\random_name_exception.txt");
            bool fileExist = File.Exists(path);
            if (fileExist)
            {
                File.AppendAllText(path, cSLWRT);
            }
            else
            {
                Directory.CreateDirectory("errors");
                File.WriteAllText(path, cSLWRT + Environment.NewLine);
            }
        }

    }
}
