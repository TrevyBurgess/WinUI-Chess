//
//
namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic
{
    using System.Diagnostics;

    /// <summary>
    /// Store position of chess pieces
    /// </summary>
    public struct ChessPieceLocation : System.IEquatable<ChessPieceLocation>
    {
        public ColPos ColLoc;
        public RowPos RowLoc;

        [DebuggerHidden]
        public ChessPieceLocation(ColPos colLoc, RowPos rowLoc)
        {
            this.ColLoc = colLoc;
            this.RowLoc = rowLoc;
        }

        [DebuggerHidden]
        public bool Equals(ChessPieceLocation other)
        {
            if (other.ColLoc == this.ColLoc && other.RowLoc == this.RowLoc)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return ColLoc.ToString() + ((int)RowLoc).ToString();
        }
    }
}
