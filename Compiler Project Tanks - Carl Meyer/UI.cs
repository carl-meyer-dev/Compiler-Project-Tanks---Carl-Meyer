using System;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    /**
     * This class is responsible for printing messages to the console
     * (this is to make it easy to print warnings, errors and info messages)
     */
    public static class UI
    {
        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public static void Strong(string message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Dump(Object o)
        {
            string json = JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.Indented);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(json);
            Console.ResetColor();
        }
    }
}