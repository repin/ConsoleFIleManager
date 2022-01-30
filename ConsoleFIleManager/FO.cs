﻿using System;
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

        public string currentDir
        {
            get { return cu; }
            set { cu = value; }
        }
        public string[] lstDir
        {
            get { return listDirectory; }
            set { listDirectory = value; }
        }
        public bool exit { get; set; }

        public FO()
        {
            exit = false;
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
            int result = 0;
            string[] comSplit = command.Split(" ");
            if (comSplit.Length == 1 && comSplit.Length > 4)
            {
                ConsoleError(-1);
                return -1;
            }
            else
            {
                switch (comSplit[0])
                {
                    case "ls":
                        result = GetTreePath(comSplit[1]); 
                        return result;

                    case "rm":

                        break;
                    case "file":

                        break;
                    case "cp":
                        result = Copy(comSplit[1], comSplit[2]);
                        return result;

                    default:
                        ConsoleError(-1);
                        return -1;
                }
            }
            return 0;

        }
        /// <summary>
        /// КОпирование файлов или папок из path в pathTarget
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pathTarget"></param>
        /// <returns></returns>
        private int Copy(string path, string pathTarget)
        {
            int t = FileOrDirectory(path);
            if(t==-1)
            {
                return -1;
            }
            else if (t == 1)
            {
                CopyFilePath(path,pathTarget);
            }
            else
            {
                CopyDirPath(path, pathTarget);
            }
           
            return 0;
        }

        private int CopyDirPath(string path, string pathTarget)
        {
            var pathTree = Directory.EnumerateFileSystemEntries(path, "", SearchOption.AllDirectories);
            string pathParentPath = Path.GetDirectoryName(path);
            int parentPathLenght = pathParentPath.Length;
            foreach (string h in pathTree)
            {
                try
                {
                    int lenghtSP = path.Length;
                    int t = FileOrDirectory(h);
                    if (t == 1)
                    {
                        string pathTargetFile = string.Concat(pathTarget, h.Remove(0, lenghtSP));
                        CopyFilePath(h, pathTargetFile);
                    }
                    else if (t == 2)
                    {
                        string directoryCreateName = string.Concat(pathTarget, path.Remove(0, lenghtSP));
                        Directory.CreateDirectory(directoryCreateName);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return -1;
                }

            }
            return 1;
        }

        private int CopyFilePath(string pathFile, string pathTarget)
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
        /// Метод возвращает в переменную listDirectory список папок по пути
        /// </summary>
        /// <param name="path">путь к папкам</param>
        /// <returns>
        /// -1 - путь не является директорией,
        /// 1 - добавление данных в переменную прошло успешно.
        /// </returns>
        private int GetTreePath(string path)
        {
            int WhatThis = FileOrDirectory(path);
            switch (WhatThis)
            {
                case -1:
                    return -1;
                case 1:
                    return -1;
                case 2:
                    try
                    {
                        lstDir = Directory.GetDirectories(path);
                        return 1;
                    }
                    catch
                    {
                        ConsoleError(-1);
                        return -1;
                    }
                default:
                    ConsoleError(-1);
                    return -1;
            }
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

        private string[] intDirectoryTree(string currentDir)
        {
            string[] none = new string[5];
            return none;
        }

        private int CopyDip (string currentDir)
        {
            return 0;
        }

        private int CopiFile(string file)
        {
            return 0;
        }

        private int FullDelDir(string dir)
        {
            return 0;
        }

        private int delFile(string file)
        {
            return 0;
        }

        private int FileInfo(string filePath)
        {
            return 0;
        }

        private int DirectoryInfo(string dirPath)
        {
            return 0;
        }
        private void ConsoleError(int error)
        {
            switch (error) 
            {
                case -1:
                    Console.WriteLine("Неверная команда или количество аргументов, воспользуйтесь командой HELP для просмотра списка команд");
                    break;
                default:
                    Console.WriteLine("Неизвестная ошибка");
                    break;
            }
        }

    }
}
