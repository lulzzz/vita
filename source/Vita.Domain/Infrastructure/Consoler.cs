using System;
using System.Diagnostics;

namespace Vita.Domain.Infrastructure
{
    public static class Consoler
    {
        public static void ShowHeader(string text, string about = null)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("------------------------");
            Console.WriteLine("------------------------");
            Console.WriteLine("");
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");

            if (!string.IsNullOrEmpty(about))
            {
                Console.WriteLine("");
                Console.WriteLine(about);
                Console.WriteLine("");
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("------------------------");
            Console.WriteLine("------------------------");
            Console.WriteLine("");
        }

        public static void Title(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("------------------------");
            Console.WriteLine(text);
            Console.WriteLine("------------------------");
        }

        public static void TitleStart(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("");
            Console.WriteLine("------------------------");
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void TitleEnd(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.WriteLine("------------------------");
            Console.WriteLine("");
        }

        public static void Write(string text)
        {
            Console.WriteLine(text);
            Trace.WriteLine(text);
        }

        public static void Write(string format, object arg)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(format, arg);
        }

        public static void Warn(string text, string message = null)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("-- WARN --");
            Console.WriteLine(text);
            if (!string.IsNullOrEmpty(message)) Write(message);
        }

        public static void Error(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("-- ERROR --");
            Console.WriteLine(text);
        }

        public static void Success(bool prompt = false)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Success");
            Console.WriteLine("");
            Pause(prompt);
        }

        public static void ShowError(Exception e, bool prompt = false)
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Exception");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("");
            Console.WriteLine(e.Message);
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(e);
            Pause(prompt);
        }


        public static void Pause(bool prompt = false)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Write("");
            if (prompt)
            {
                Write("\nPress any key to exit.");
                Console.ReadLine();
            }
        }
    }
}