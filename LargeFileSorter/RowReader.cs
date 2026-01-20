using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileSorter
{    
    internal class RowReader : IDisposable
    {
        StreamReader? _reader;
        long _index;

        private RowReader(string file, long index)
        {
            _reader = new StreamReader(file, Encoding.ASCII, false, Constants.BUFF_SIZE);
            _index = index;
        }

        public static RowReader? Create(string file, long index)
        {
            RowReader? re = null;
            try
            {
                re = new RowReader(file, index);
            }
            catch { }

            return re;
        }

        public RowExtended? NextRow()
        {
            string? line = _reader!.ReadLine();
            if (line == null)
                return null;

            return RowExtended.Create(line!, _index);
        }

        public void Dispose()
        {
            if(_reader is not null)
                _reader.Dispose();
        }
    }
}
