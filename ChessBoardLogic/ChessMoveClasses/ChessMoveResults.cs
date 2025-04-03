using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic
{
    public class ChessMoveResults
    {
        /// <summary>
        /// Old Rook position
        /// </summary>
        public ChessPieceLocation StartLocation;

        /// <summary>
        /// New Rook position
        /// </summary>
        public ChessPieceLocation EndLocation;

        /// <summary>
        /// Result of specific move
        /// </summary>
        public MoveMessage Result;

        /// <summary>
        /// Location of piece that was killed
        /// </summary>
        public ChessPieceLocation PieceKilled;

        /// <summary>
        /// Location of piece that was killed
        /// </summary>
        public ColPos EnPassantColumn;

        /// <summary>
        /// True if pawn is being promoted
        /// </summary>
        public bool PawnPromoted;
    }
}
