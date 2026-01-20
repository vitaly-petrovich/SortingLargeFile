using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileSorter
{
    internal class SortMemoryChunks
    {
        MemoryChunks _memChunks;
        Task[] sortMemChunks;
        public SortMemoryChunks(MemoryChunks memChunks)
        {
            _memChunks = memChunks;

            sortMemChunks = new Task[memChunks.NumChunks];
            for (int ch = 0; ch < memChunks.NumChunks; ch++)
            {
                sortMemChunks[ch] = new Task((chunk) => 
                { 
                    long ch = Convert.ToInt64(chunk); 
                    memChunks.Sort(ch); 
                }, ch);
                sortMemChunks[ch].Start();
            }
        }

        public async Task SortAll()
        {
            await Task.WhenAll(sortMemChunks);
        }
    }
}
