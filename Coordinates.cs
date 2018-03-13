using System;
using System.Text;

namespace Gomoku
{
    public class Coordinates
    {
        public int col;
        public int row;
        public bool valid;
 
        public Coordinates(int col, int row)
        {
            this.col = col;
            this.row = row;
            valid = col >= 0 && col <  19 && row >= 0 && row < 19;
        }

        public Coordinates(Coordinates other)
        {
            this.col = other.col;
            this.row = other.row;
        }

        public bool CheckValidity()
        {
            valid = col >= 0 && col <  19 && row >= 0 && row < 19;
            return valid;
        }
 
        public override String ToString() {
            StringBuilder sb = new StringBuilder(" [");
            sb.Append(col);
            sb.Append("; ");
            sb.Append(row);
            sb.Append("]");
            return sb.ToString();
        }
    }
}