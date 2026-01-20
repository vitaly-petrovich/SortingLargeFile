using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileSorter
{
    internal class Constants
    {
        public const long GIGABYTE = 1024 * 1024 * 1024;
        public const long MEGABYTE = 1024 * 1024;
        public const int DEFAULT_CHUNK_CAPACITY = 200000000;
        public const int DEFAULT_TASK_COUNT = 8;
        public const int DEFAULT_MEM_CHUNK_CAPACITY = DEFAULT_CHUNK_CAPACITY / DEFAULT_TASK_COUNT;
        public const int BUFF_SIZE = 4194304;
    }
}
