using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileSortingChecker
{
    internal static class FileSortingChecker
    {
        public static bool Check(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string? line;
                Row? lastRow = null;
                while ((line = reader.ReadLine()) != null)
                {
                    Row? row = Row.Create(line);
                    if (row is null)
                        return false;
                    if (lastRow is null)
                    {
                        lastRow = row;
                        continue;
                    }
                    if (lastRow > row)
                        return false;
                    lastRow = row;
                }
                return true;
            }
        }
    }
}
