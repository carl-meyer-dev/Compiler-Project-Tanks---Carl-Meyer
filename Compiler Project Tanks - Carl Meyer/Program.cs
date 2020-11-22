using System;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Parser P = new Parser("a + ( b - c )");
            Console.ReadLine();

        }
    }
}