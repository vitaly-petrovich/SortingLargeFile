using System;

namespace LargeFileGenerator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: LargeFileGenerator <fileName> <fileLength>");
                return;
            }
            string fileName = args[0];
            long fileLength;
            if (!long.TryParse(args[1], out fileLength) || fileLength < 0)
            {
                Console.WriteLine("File length is not correct.");
                return;
            }
            try
            {
                FileGenerator.Generate(fileName, fileLength);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during generating file!");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}