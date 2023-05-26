using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneryLauncher
{
    internal class Scenery
    {
        public static void WriteLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" [");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Scenery");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("] ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Write(string text)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" [");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Scenery");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("] ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
