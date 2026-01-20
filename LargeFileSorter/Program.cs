using System;
using System.Diagnostics;
using System.IO;

namespace LargeFileSorter
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: LargeFileSorter <inputFilePath> <outputFilePath>");
                return;
            }
            string input = args[0];
            string output = args[1];
            if (!File.Exists(input))
            {
                Console.WriteLine($"{input} doesn't exist");
                return;
            }

            try
            {
                LargeFileSorter sorter = new LargeFileSorter(input, output);
                sorter.SortFileAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sorting large file!.");
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
