using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileSortingChecker
{
    internal class Row : IComparer<Row>, IComparable
    {
        static StringComparer _stringComparer = new StringComparer();

        public int Id { get; private set; }
        public string Text { get; private set; }
        public int Length {
            get { return this.ToString().Length; } }

        public static Row? Create(string text)
        {
            string txt = text.Trim();
            var index = txt.IndexOf('.');
            if (index == -1)
                return null;

            int id = 0;
            if(!int.TryParse(txt.Substring(0, index), out id))
                return null;

            if(txt.Length <= index + 2)
                return null;

            return new Row(id, txt.Substring(index + 1).Trim());
        }
        protected Row(int id, string text)
        {
            Id = id;
            Text = text;
        }

        public static bool operator <(Row left, Row right)
        {
            var res = _stringComparer.Compare(left.Text, right.Text);
            return res < 0 || (res == 0 && left.Id < right.Id);
        }

        public static bool operator >(Row left, Row right)
        {
            var res = _stringComparer.Compare(left.Text, right.Text);
            return res > 0 || (res == 0 && left.Id > right.Id);
        }

        public static bool operator ==(Row left, Row right)
        {
            return left.Id == right.Id && _stringComparer.Compare(left.Text, right.Text) == 0;
        }

        public static bool operator !=(Row left, Row right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Id + ". " + Text;
        }

        public int Compare(Row? x, Row? y)
        {
            int? result = null;
            result = x is null && y is null ? 0 :
                (x is not null && y is null ? 1 :
                (x is null && y is not null ? -1 : 
                null));
            if (result is not null)
                return result.Value;

            return x! > y! ? 1 :
                (x! == y! ? 0 :
                (x! < y! ? -1 : 0));
        }

        public int CompareTo(object? obj)
        {
            if(obj is null) return 1;
            if(obj is Row row)
            {
                return this > row ? 1 :
                (this == row ? 0 :
                (this < row ? -1 : 0));
            }
            return 1;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return true;
            else if (obj is Row row)
                return row == this;
            else return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Text);
        }        
    }
}
