using System;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    public class UI
    {
        public void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public void Strong(string message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Dump(Object o)
        {
            string json = JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.Indented);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(json);
            Console.ResetColor();
        }
    }
}