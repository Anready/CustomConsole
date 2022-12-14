using System;
using System.IO;
using System.Diagnostics;

namespace CustomConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("command.txt"))
            {
                using (StreamWriter writer = new StreamWriter("command.txt", true))
                {
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Type 'help' if your don't know command");
            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("$ - ");
                Console.ResetColor();
                String commandIn = Console.ReadLine();
                if (commandIn == "")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Enter command!");
                    continue;
                }
                if (commandIn.Contains("new"))
                {
                    commandIn = commandIn.Replace("new ", "");
                    var arrayOfStrings = commandIn.Split(',');
                    int i = 0;
                    using (StreamReader reader = new StreamReader("command.txt"))
                    {
                        string text = null;
                        while ((text = reader.ReadLine()) != null)
                        {
                            if (text.Contains(arrayOfStrings[1]) || text.Contains(arrayOfStrings[0]))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("This comand already create");
                                i = 1;
                            }
                        }
                    }
                    if(i == 1)
                    {
                        continue;
                    }
                    writeCommandFile(arrayOfStrings[1], arrayOfStrings[0]);
                    continue;
                }
                if (commandIn.Contains("help"))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("This is a custom console, it has only four built-in commands - new, printMyComandList, manyComand and help. \nAll other commands you create yourself through the command in this format: new [name your command], [original command name in console].\nIf your want know your added custom comand, enter printMyComandList.\nIf you want to run all entered commands at the same time write manyComand,\nto get out of it write stop, to run all commands write run.");
                    continue;
                }
                if(commandIn == "printMyComandList")
                {
                    using (StreamReader reader = new StreamReader("command.txt"))
                    {
                        Console.WriteLine(reader.ReadToEnd());
                        continue;
                    }
                }
                if(commandIn == "manyComand")
                {
                    Console.WriteLine("Your entered in manyComand mode");
                    string all = null;
                    while (true)
                    { 
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("$ - ");
                        Console.ResetColor();
                        commandIn = Console.ReadLine();
                        if (commandIn == "stop")
                        {
                            break;
                        }
                        if (commandIn == "run")
                        {
                            if(all == null)
                            {
                                Console.WriteLine("Don't enter any comand!");
                                continue;
                            }
                            using (StreamWriter writer = new StreamWriter("start.bat", true))
                            {
                                writer.WriteLine("@echo off\n" + all + "\ndel %~s0 /q");
                            }
                            try
                            {
                                Process.Start("start.bat");
                            }
                            catch (Exception e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Command not runing! Error:\n" + e);
                            }
                            Console.WriteLine("Your exit from manyComand mode");
                            break;
                        }
                        if (commandIn.Contains("cd"))
                        {
                            all = all + "\n" + commandIn;
                            continue;
                        }
                        using (StreamReader reader = new StreamReader("command.txt"))
                        {
                            string text = null;
                            int i = 0;
                            while ((text = reader.ReadLine()) != null)
                            {
                                var arrayOfStrings = text.Split(',');
                                if (arrayOfStrings[0].Contains(commandIn))
                                {
                                    all = all + "\n" + arrayOfStrings[1];
                                    i = 1;
                                }
                            }
                            if (i == 0)
                            {
                                Console.WriteLine("Command not found! Exit from this mode and create this command!");
                            }
                        }
                    } continue;
                }
                readCommandFile(commandIn);
            }
        }
        static void writeCommandFile(String Do, String name)
            {
            using (StreamWriter writer = new StreamWriter("command.txt", true))
            {
                writer.WriteLine(name + " , " + Do);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Create new command successful!");
            }
        }
        static void readCommandFile(String commandIn)
        {
            using (StreamReader reader = new StreamReader("command.txt"))
            {
                string text = null;
                int i = 0;
                while ((text = reader.ReadLine()) != null)
                {
                    var arrayOfStrings = text.Split(',');
                    if (arrayOfStrings[0].Contains(commandIn))
                    {
                        Console.WriteLine("");
                        using (StreamWriter writer = new StreamWriter("start.bat", true))
                        {
                            writer.WriteLine("@echo off\n" + arrayOfStrings[1] + "\ndel %~s0 /q");
                        }
                        try
                        {
                            Process.Start("start.bat");
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Command not runing! Error:\n" + e);
                        }
                        i = 1;
                    }
                }
                if (i == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Command not found! Create this command!");
                }
            }
        }
    }
}