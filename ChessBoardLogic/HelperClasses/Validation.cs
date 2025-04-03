//
//
namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic
{
    using System.Diagnostics.Contracts;

    public class ValidateBounds
    {
        [Pure]
        public static bool IsInBounds(ChessPieceLocation chessPosition)
        {
            return IsInBounds(chessPosition.ColLoc) && IsInBounds(chessPosition.RowLoc);
        }

        [Pure]
        public static bool IsInBounds(ColPos colPos)
        {
            if ((int)colPos < 1 || (int)colPos > 8)
                return false;
            else
                return true;
        }

        [Pure]
        public static bool IsInBounds(RowPos rowPos)
        {
            if ((int)rowPos < 1 || (int)rowPos > 8)
                return false;
            else
                return true;
        }
    }
}
