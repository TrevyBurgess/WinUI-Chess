//
//
namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Describes what's living on the squares of the chess board
    /// </summary>
    public enum ChessPieceType
    {
        EmptySquare = 0,
        Pawn = 1, Rook = 2, Knight = 3, Bishop = 4, King = 5, Queen = 6
    }
}
