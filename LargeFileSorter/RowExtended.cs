using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileSorter
{
    internal class RowExtended : Row
    {
        public List<long> Indexes {  get; private set; }
        public static RowExtended? Create(string text, long index)
        {
            var row = Row.Create(text);
            if(row is null)
                return null;

            RowExtended rowExtended = new RowExtended(row, index);
            return rowExtended;
        }

        private RowExtended(Row row, long index)
            : base(row.Id, row.Text)
        {
            Indexes = new List<long>();
            Indexes.Add(index);
        }

        public override string ToString() 
        {
            return base.ToString();
        }
    }
}
