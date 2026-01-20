using System;
using System.Reflection.Emit;

namespace LargeFileSortingChecker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: LargeFileSortingChecker <fileName>");
                return;
            }
            string fileName = args[0];
            try
            {
                if(FileSortingChecker.Check(fileName))
                    Console.WriteLine($"File: {fileName} is sorted!");
                else
                    Console.WriteLine($"File: {fileName} is not sorted!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during generating file!");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}