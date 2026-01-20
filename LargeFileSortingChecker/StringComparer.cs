using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileSortingChecker
{
    internal class StringComparer : Comparer<string>
    {
        public override int Compare(string? inX, string? inY)
        {
            if (inX == null && inY == null) return 0;
            else if (inX != null && inY == null) return 1;
            else if (inX == null && inY != null) return -1;

            string x = inX!.Trim();
            string y = inY!.Trim();

            int length = x.Length > y.Length ? y.Length : x.Length;
            for (int i = 0; i < length; i++)
            {
                if (x[i] > y[i])
                    return 1;
                else if (x[i] < y[i])
                    return -1;
            }

            return x.Length > y.Length ? 1 : (x.Length < y.Length ? -1 : 0);
        }
    }
}
