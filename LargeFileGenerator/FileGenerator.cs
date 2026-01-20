using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileGenerator
{
    internal static class FileGenerator
    {
        static Random _rand = new Random();
        public static void Generate(string outputFileName, long fileLength)
        {
            using (StreamWriter writer = new StreamWriter(outputFileName, false))
            {
                string text = "";
                while (fileLength > 0)
                {
                    if(fileLength % 40 != 0 || string.IsNullOrEmpty(text))
                        text = GenerateRandomString(); 
                    string num = _rand.Next(40000).ToString();
                    int totalLen = num.Length + 4 + text.Length;
                    StringBuilder sb = new StringBuilder(totalLen);
                    sb.Append(num).Append(". ").Append(text);

                    writer.WriteLine(sb);

                    fileLength -= totalLen;
                }
            }
        }

        static string GenerateRandomString()
        {
            int strLen = _rand.Next(3, 100);
            StringBuilder sb = new StringBuilder(strLen);
            for (int i = 0; i < strLen; i++)
            {
                sb.Append(GenerateSymbol());
            }
            return sb.ToString();
        }

        static char GenerateSymbol()
        {
            int method = _rand.Next(3);
            switch (method)
            {
                case 0:
                    return (char)_rand.Next(48, 58);
                case 1:
                    return (char)_rand.Next(65, 90);
                case 2:
                    return (char)_rand.Next(97, 123);
                default:
                    return (char)_rand.Next(48, 58);
            }
        }
    }
}
