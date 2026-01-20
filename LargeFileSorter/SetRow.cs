using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileSorter
{
    internal class SetRow
    {
        SortedSet<RowExtended> _rows = new SortedSet<RowExtended>();
        public int Count { get { return _rows.Count; } }

        public RowExtended? Min() => _rows.Min();

        public void Add(RowExtended newRow)
        {
            if (_rows.Contains(newRow))
            {
                RowExtended row;
                _rows.TryGetValue(newRow, out row!);
                row.Indexes.Add(newRow.Indexes[0]);
            }
            else
                _rows.Add(newRow);
        }

        public void Remove(RowExtended row)
        {
            if (row.Indexes.Count > 1)
                row.Indexes.RemoveAt(0);
            else
                _rows.Remove(row);
        }
    }
}
