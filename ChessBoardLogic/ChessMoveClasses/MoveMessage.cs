//
//
namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum MoveMessage
    {
        /// <summary>
        /// Move failed
        /// </summary>
        MoveFailed,

        /// <summary>
        /// Move Succeeded, No piece captured
        /// </summary>
        MoveSucceeded,

        /// <summary>
        /// Move Succeeded, Piece Captured
        /// </summary>
        PieceCaptured,

        /// <summary>
        /// Pawn was captured En Passant, No piece captured
        /// </summary>
        PawnCapturedEnPassant,

        /// <summary>
        /// Move caused castling to take place, No piece captured
        /// </summary>
        CastlingSucceeded
    }
}
