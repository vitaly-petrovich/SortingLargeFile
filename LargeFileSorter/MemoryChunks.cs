using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileSorter
{
    internal class MemoryChunks
    {
        readonly long _memChunkSize;
        int _currentMemChunk = 0;
        long _currentMemChunkSize = 0;
        List<Row>[] _rows;
        
        public int NumChunks { get; private set; }
        public MemoryChunks(int numChunks)
        {
            NumChunks = numChunks;
            _memChunkSize = Constants.DEFAULT_MEM_CHUNK_CAPACITY + Constants.DEFAULT_MEM_CHUNK_CAPACITY/10;
            _rows = new List<Row>[NumChunks];
            for (int i = 0; i < NumChunks; i++)
                _rows[i] = new List<Row>((int)(_memChunkSize * 2));
        }

        public void Add(Row row)
        {
            _rows[_currentMemChunk].Add(row);
            _currentMemChunkSize += row.Length + 2;
            if (_currentMemChunkSize >= _memChunkSize)
            {
                _currentMemChunk++;
                _currentMemChunkSize = 0;
            }
        }

        public void Sort(long chunk)
        {
            if (_rows[chunk].Count == 1
                || _rows[chunk].Count == 0)
                return;
            else if (_rows[chunk].Count == 2)
            {
                if (_rows[chunk][0] > _rows[chunk][1])
                {
                    var tmp = _rows[chunk][0];
                    _rows[chunk][0] = _rows[chunk][1];
                    _rows[chunk][1] = tmp;
                }
            }
            else
                _rows[chunk].Sort((r1, r2) => r1.CompareTo(r2));
        }

        public List<Row> this[int index]
        {
            get { return _rows[index]; }
        }
    }
}
